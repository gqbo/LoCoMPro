using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Pages.Records;
using LoCoMPro_LV.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using F23.StringSimilarity;
using Microsoft.EntityFrameworkCore;
using Humanizer;

namespace LoCoMPro_LV.Pages.Reports
{
    public class AnomaliesNameModel : PageModel
    {
        private readonly LoComproContext _context;

        public AnomaliesNameModel(LoComproContext context)
        {
            _context = context;
            Anomalies = new List<Anomalie>();
        }

        public List<Anomalie> Anomalies { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<RecordStoreModel> AllRecordsQuery = from record in _context.Records
                                                           select new RecordStoreModel
                                                           {
                                                               Record = record,
                                                               Store = null
                                                           };

            List<RecordStoreModel> allRecords = AllRecordsQuery.ToList();

            // Utiliza el programa de detección de anomalías
            JaroWinklerSimilarity jaroWinkler = new JaroWinklerSimilarity();
            List<List<RecordStoreModel>> groupedRecords = jaroWinkler.GroupSimilarRecords(allRecords);
            groupedRecords = groupedRecords.Distinct(new ListComparer<RecordStoreModel>()).ToList();

        }
    }

    public class ListComparer<T> : IEqualityComparer<List<T>>
    {
        public bool Equals(List<T> x, List<T> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(List<T> obj)
        {
            return obj.Aggregate(17, (hash, item) => hash * 23 + item.GetHashCode());
        }
    }

    internal class JaroWinklerSimilarity
    {
        private JaroWinkler jwAlgorithm;

        public JaroWinklerSimilarity()
        {
            jwAlgorithm = new JaroWinkler();
        }

        public List<List<RecordStoreModel>> GroupSimilarRecords(List<RecordStoreModel> recordList)
        {
            List<List<RecordStoreModel>> groupedRecords = new List<List<RecordStoreModel>>();
            HashSet<RecordStoreModel> usedRecords = new HashSet<RecordStoreModel>();

            if (recordList != null)
            {
                var recordCount = recordList.Count;
                if (recordCount > 1)
                {
                    int index = 0;
                    while (index < recordCount)
                    {
                        if (!usedRecords.Contains(recordList[index]))
                        {
                            List<RecordStoreModel> currentGroup = new List<RecordStoreModel> { recordList[index] };

                            foreach (RecordStoreModel record in getRecords(recordList, index))
                            {
                                var distance = jwAlgorithm.Similarity(recordList[index].Record.NameProduct, record.Record.NameProduct);
                                Console.WriteLine("Similarity [" + recordList[index].Record.NameProduct + ", " + record.Record.NameProduct + "] : " + String.Format("{0:0.000}", distance));

                                if (distance > 0.7 && distance < 1.0)
                                {
                                    currentGroup.Add(record);
                                    usedRecords.Add(record);
                                }
                            }

                            if (currentGroup.Count > 1)
                            {
                                groupedRecords.Add(currentGroup);
                            }
                        }

                        index++;
                        Console.WriteLine("");
                    }
                }
            }

            return groupedRecords;
        }

        IEnumerable<RecordStoreModel> getRecords(List<RecordStoreModel> recordList, int startPosition)
        {
            var recordCount = recordList.Count;

            if (startPosition >= recordCount)
                yield break;

            for (int i = startPosition + 1; i < recordCount; i++)
            {
                yield return recordList[i];
            }
        }
    }
}


/*            foreach (var group in groupedRecords)
            {
                var firstRecord = group.First().Record;

                if (!_context.Anomalies.Any(a =>
                    a.NameGenerator == group.First().Record.NameGenerator &&
                    a.RecordDate == group.First().Record.RecordDate &&
                    a.Type == "Nombre"))
                {
                    Anomalie anomalie = new Anomalie
                    {
                        NameGenerator = firstRecord.NameProduct,
                        RecordDate = firstRecord.RecordDate,
                        Type = "Nombre",
                        Comment = "El nombre es anómalo",
                        State = 0
                    };
                    _context.Anomalies.Add(anomalie);
                    await _context.SaveChangesAsync();

                }
            }*/