using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Pages.Records
{
    public class IndexModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public IndexModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        public IList<Record> Record { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var recordsQuery = from m in _context.Records
                               select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                recordsQuery = recordsQuery.Where(s => s.NameProduct.Contains(SearchString));
            }

            var groupedRecordsQuery = from record in recordsQuery
                                      group record by new
                                      { record.NameProduct, record.NameStore, record.NameCanton, record.NameProvince } into recordGroup
                                      orderby recordGroup.Key.NameProduct descending
                                      select recordGroup;

            Record = await groupedRecordsQuery
                .Select(group => group.OrderByDescending(r => r.RecordDate).FirstOrDefault())
                .ToListAsync();

        }


    }
}

