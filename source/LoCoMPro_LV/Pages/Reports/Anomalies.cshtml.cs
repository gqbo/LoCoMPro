using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Pages.Records;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoCoMPro_LV.Pages.Reports
{
    public class AnomaliesModel : PageModel
    {
        private readonly LoComproContext _context;

        public AnomaliesModel(LoComproContext context)
        {
            _context = context;
        }

        public List<Anomalie> Anomalies { get; set; }

        public async Task OnGetAsync()
        {
            var orderedRecordsQuery = BuildOrderedRecordsQuery();
            List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords = GroupRecords(orderedRecordsQuery);
            Anomalies = await _context.Anomalies
                .Where(a => a.State == 0)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostRunAnomaliesPrecio()
        {
            var orderedRecordsQuery = BuildOrderedRecordsQuery();
            List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords = GroupRecords(orderedRecordsQuery);
            await ProcessGroupedRecordsPrice(groupedRecords);

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostRunAnomaliesFecha()
        {
            var orderedRecordsQuery = BuildOrderedRecordsQuery();
            List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords = GroupRecords(orderedRecordsQuery);
            await ProcessGroupedRecordsDate(groupedRecords);

            return new JsonResult(new { success = true });
        }

        private IQueryable<RecordStoreModel> BuildOrderedRecordsQuery()
        {
            IQueryable<RecordStoreModel> orderedRecordsQuery = from record in _context.Records
                                                               join store in _context.Stores on new { record.NameStore, record.Latitude, record.Longitude }
                                                               equals new { store.NameStore, store.Latitude, store.Longitude }
                                                               select new RecordStoreModel
                                                               {
                                                                   Record = record,
                                                                   Store = store
                                                               };
            return orderedRecordsQuery;
        }

        private IOrderedQueryable<IGrouping<object, RecordStoreModel>> ApplySorting(IQueryable<RecordStoreModel> orderedRecordsQuery, string sortOrder)
        {
            var groupedRecordsQuery = from record in orderedRecordsQuery
                                      group record by new
                                      {
                                          record.Record.NameProduct,
                                          record.Record.NameStore,
                                          record.Store.NameCanton,
                                          record.Store.NameProvince
                                      } into recordGroup
                                      orderby recordGroup.Key.NameProduct descending
                                      select recordGroup;
            
            return groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.Record.RecordDate));
        }

        public class GroupingKey
        {
            public string NameProduct { get; set; }
            public string NameStore { get; set; }
            public string NameCanton { get; set; }
            public string NameProvince { get; set; }
        }

        private List<IGrouping<GroupingKey, RecordStoreModel>> GroupRecords(IQueryable<RecordStoreModel> orderedRecordsQuery)
        {
            return orderedRecordsQuery.GroupBy(
                record => new GroupingKey
                {
                    NameProduct = record.Record.NameProduct,
                    NameStore = record.Record.NameStore,
                    NameCanton = record.Store.NameCanton,
                    NameProvince = record.Store.NameProvince
                }
            ).ToList();
        }

        private async Task ProcessGroupedRecordsDate(List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords)
        {
            List<RecordStoreModel> recordsGroupContainer = new List<RecordStoreModel>();
            foreach (var group in groupedRecords)
            {
                GroupingKey groupKey = group.Key;

                foreach (var record in group)
                {
                    recordsGroupContainer.Add(record);
                }
                if (recordsGroupContainer.Count() > 4)
                {
                    await AnomaliesDate(recordsGroupContainer);
                }

                recordsGroupContainer.Clear();
            }
        }

        private async Task ProcessGroupedRecordsPrice(List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords)
        {
            List<RecordStoreModel> recordsGroupContainer = new List<RecordStoreModel>();
            foreach (var group in groupedRecords)
            {
                GroupingKey groupKey = group.Key;

                foreach (var record in group)
                {
                    recordsGroupContainer.Add(record);
                }
                if (recordsGroupContainer.Count() > 6)
                {
                    await AnomaliesPrice(recordsGroupContainer);
                }
                recordsGroupContainer.Clear();
            }
        }

        private async Task AnomaliesDate(List<RecordStoreModel> recordsGroupContainer)
        {
            List<RecordStoreModel> selectedRecords = new List<RecordStoreModel>();

            var sortedRecords = recordsGroupContainer.OrderBy(r => r.Record.RecordDate).ToList();

            int startIndex = (int)(sortedRecords.Count * 0.30);

            DateTime startDate = sortedRecords[startIndex].Record.RecordDate;

            DateTime endDate = sortedRecords[sortedRecords.Count - 1].Record.RecordDate;

            int delta = 2;
            int daysDifference = (((int)(endDate - startDate).TotalDays)) * delta;
            DateTime referenceDate = startDate.AddDays(-daysDifference);

            selectedRecords.AddRange(sortedRecords.Where(r => r.Record.RecordDate < referenceDate &&  r.Record.Hide == false ));

            foreach (var indice in selectedRecords)
            {
                if (!_context.Anomalies.Any(a =>
                a.NameGenerator == indice.Record.NameGenerator &&
                a.RecordDate == indice.Record.RecordDate &&
                a.Type == "Fecha"))
                {
                    Anomalie anomalie = new Anomalie
                    {
                        NameGenerator = indice.Record.NameGenerator,
                        RecordDate = indice.Record.RecordDate,
                        Type = "Fecha",
                        Comment = "La fecha es muy antigua",
                        State = 0
                    };
                    _context.Anomalies.Add(anomalie);
                    await _context.SaveChangesAsync();
                }
            }
            sortedRecords.Clear();
            selectedRecords.Clear();
        }

        private async Task AnomaliesPrice(List<RecordStoreModel> recordsGroupContainer)
        {
            List<RecordStoreModel> selectedRecords = new List<RecordStoreModel>();
            var sortedRecords = recordsGroupContainer.OrderBy(r => r.Record.Price).ToList();  // Ordena por precio

            int q2Index = CalculateQ2Index(sortedRecords.Count);

            int q1Index = CalculateQ1Index(q2Index);
            int q3Index = CalculateQ3Index(q2Index, sortedRecords.Count);
            double? q1;
            double? q3;

            if (q2Index % 2 == 0 )
            {
                if (sortedRecords.Count % 2 != 0)
                {
                    q1 = (sortedRecords[q1Index].Record.Price);
                    q3 = (sortedRecords[q3Index].Record.Price);
                }
                else
                {
                    q1 = ((sortedRecords[q1Index].Record.Price) + (sortedRecords[q1Index+1].Record.Price))/2;
                    q3 = ((sortedRecords[q3Index].Record.Price) + (sortedRecords[q3Index-1].Record.Price))/2;
                }             
            }
            else
            {
                q1 = ((sortedRecords[q1Index].Record.Price) + (sortedRecords[q1Index + 1].Record.Price))/2;
                q3 = ((sortedRecords[q3Index].Record.Price) + (sortedRecords[q3Index - 1].Record.Price))/2;
            }

            double? ric = q3 - q1;

            double umbral = 1.5;

            double? lowerBound = q1 - umbral * ric;
            double? upperBound = q3 + umbral * ric;

            foreach (var indice in sortedRecords)
            {
                if (indice.Record.Price < lowerBound || indice.Record.Price > upperBound)
                {
                    string coment;
                    if (indice.Record.Price < lowerBound)
                    {
                        coment = "El precio es muy bajo comparado a los demás.";
                    }
                    else
                    {
                        coment = "El precio es muy alto comparado a los demás.";
                    }
                    if (!_context.Anomalies.Any(a =>
                    a.NameGenerator == indice.Record.NameGenerator &&
                    a.RecordDate == indice.Record.RecordDate &&
                    a.Type == "Precio"))
                    {
                        Anomalie anomalie = new Anomalie
                        {
                            NameGenerator = indice.Record.NameGenerator,
                            RecordDate = indice.Record.RecordDate,
                            Type = "Precio",
                            Comment = coment,
                            State = 0
                        };
                        _context.Anomalies.Add(anomalie);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            sortedRecords.Clear();
            selectedRecords.Clear();
        }

        private int CalculateQ2Index(int recordCount)
        {
            if (recordCount % 2 == 0)
            {
                return recordCount / 2;
            }
            else
            {
                return ((recordCount - 1) / 2) + 1;
            }
        }

        private int CalculateQ1Index(int q2)
        {
            if (q2 % 2 == 0)
            {
                return (int) ((q2 / 2) - 1);
            }
            else
            {
                return (int) (((q2-1) / 2) - 1);
            }
        }

        private int CalculateQ3Index(int q2, int recordCount)
        {
            if (q2 % 2 == 0)
            {
                if ((recordCount % 2 == 0))
                {
                    return (int)(((q2 + 1) + (q2 / 2)) - 1);
                }
                else
                {
                    return (int)((q2 + (q2 / 2)) - 1);
                }
            }
            else
            {
                if ((recordCount % 2 == 0))
                {
                    return (int)((q2 + 1) + ((q2 + 1) / 2));
                }
                else
                {
                    return (int)((q2 + 1) + ((q2 - 1) / 2));
                }
            }
        }
    }
}

