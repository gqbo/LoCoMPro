using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LoCoMPro_LV.Pages.Records;

namespace LoCoMPro_LV.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly LoComproContext _context;

        public IndexModel(LoComproContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista de tipo "Record", que almacena los registros correspondientes al producto buscado.
        /// </summary>
        public IList<RecordStoreReportModel> recordStoreReports { get; set; } = default!;

        public IList<Report> tempReports { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var recordsQuery = from record in _context.Records
                               join store in _context.Stores on new { record.NameStore, record.Latitude, record.Longitude }
                               equals new { store.NameStore, store.Latitude, store.Longitude }
                               select new RecordStoreReportModel
                               {
                                   Record = record,
                                   Store = store
                               };

            List<RecordStoreReportModel> currentRecords = recordsQuery.ToList();

            foreach (var recordSRM in currentRecords)
            {
                var new_reports = from reports in _context.Reports
                                  where reports.NameGenerator == recordSRM.Record.NameGenerator &&
                                        reports.RecordDate == recordSRM.Record.RecordDate
                                  select reports;

                tempReports = await new_reports.ToListAsync();

                recordSRM.Reports = new List<Report>();
                if (new_reports.Any())
                {
                    recordSRM.Reports = tempReports;
                }
            }

            currentRecords.RemoveAll(m => m.Reports.Count == 0);

            var groupedRecordsQuery = from record in currentRecords
                                      group record by new
                                      { record.Record.NameProduct, record.Record.NameStore, record.Record.Latitude, record.Record.Longitude } into recordGroup
                                      orderby recordGroup.Key.NameProduct descending
                                      select recordGroup;
            recordStoreReports = groupedRecordsQuery
                .Select(group => group.FirstOrDefault())
                .ToList();
        }

    }
}


