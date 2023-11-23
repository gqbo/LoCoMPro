﻿using Microsoft.AspNetCore.Mvc.RazorPages;
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
            await ProcessGroupedRecordsPrice(groupedRecords);
            await ProcessGroupedRecordsDate(groupedRecords);
            Anomalies = await _context.Anomalies.ToListAsync();
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
                AnomaliesDate(recordsGroupContainer);

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
                AnomaliesPrice(recordsGroupContainer);

                recordsGroupContainer.Clear();
            }
        }

        private async Task AnomaliesDate(List<RecordStoreModel> recordsGroupContainer)
        {
            List<RecordStoreModel> selectedRecords = new List<RecordStoreModel>();
            var sortedRecords = recordsGroupContainer.OrderBy(r => r.Record.RecordDate).ToList();
            int endIndex = (int)(sortedRecords.Count * 0.2);
            var selectedRecordsSubset = sortedRecords.Take(endIndex).ToList();

            selectedRecords.AddRange(selectedRecordsSubset.Where(r => r.Record.Hide == false));

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
            selectedRecordsSubset.Clear();
            endIndex = 0;
            sortedRecords.Clear();
            selectedRecords.Clear();
        }

        private async Task AnomaliesPrice(List<RecordStoreModel> recordsGroupContainer)
        {
            List<RecordStoreModel> selectedRecords = new List<RecordStoreModel>();
            var sortedRecords = recordsGroupContainer.OrderBy(r => r.Record.Price).ToList();  // Ordena por precio

            // Calcula Q2
            int q2Index = CalculateQ2Index(sortedRecords.Count);

            // Calcula Q1 y Q3 utilizando los índices de Q2
            int q1Index = CalculateQ1Index(q2Index);
            int q3Index = CalculateQ3Index(q2Index);
            double? q1;
            double? q3;

            // Obtiene los valores reales de Q1, Q2 y Q3
            if (q2Index % 2 == 0)
            {
                q1 = ((sortedRecords[q1Index].Record.Price) + (sortedRecords[q1Index + 1].Record.Price)) / 2;
                q3 = ((sortedRecords[q3Index].Record.Price) + (sortedRecords[q3Index - 1].Record.Price)) / 2;
            }
            else
            {
                q1 = sortedRecords[q1Index].Record.Price;
                q3 = sortedRecords[q3Index].Record.Price;
            }

            // Calcula el RIC
            double? ric = q3 - q1;

            // Define el umbral (puedes ajustar este valor según tus necesidades)
            double umbral = 1.5;

            // Establece el rango para valores no atípicos
            double? lowerBound = q1 - umbral * ric;
            double? upperBound = q3 + umbral * ric;

            // Identifica los valores atípicos y agrégales a selectedRecords
            foreach (var indice in sortedRecords)
            {
                if (indice.Record.Price < lowerBound || indice.Record.Price > upperBound)
                {
                    string coment;
                    if (indice.Record.Price < lowerBound)
                    {
                        coment = "El precio es muy bajo comparado a los demás";
                    }
                    else
                    {
                        coment = "El precio es muy alto comparado a los demás";
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

            // Limpia la lista de registros seleccionados
            sortedRecords.Clear();
            selectedRecords.Clear();
        }

        private int CalculateQ2Index(int recordCount)
        {
            // La mediana siempre es el valor en el centro o el promedio de los dos valores centrales
            if (recordCount % 2 == 0)
            {
                // Si la cantidad de datos es par, la mediana es el índice del valor en el centro
                return recordCount / 2;
            }
            else
            {
                // Si la cantidad de datos es impar, la mediana es el índice del valor en el centro
                return ((recordCount - 1) / 2) + 1;
            }
        }

        private int CalculateQ1Index(int q2)
        {
            if (q2 % 2 == 0)
            {
                return (int)((q2 / 2) - 1);
            }
            else
            {
                return (int)(((q2 - 1) / 2));
            }
        }

        private int CalculateQ3Index(int q2)
        {
            if (q2 % 2 == 0)
            {
                return (int)((q2 + (q2 / 2)) - 1);
            }
            else
            {
                return (int)(q2 + ((q2 - 1) / 2));
            }
        }
    }
}