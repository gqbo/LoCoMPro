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

namespace LoCoMPro_LV.Pages.Records
{
    public class DetailsModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public DetailsModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        public IList<Record> Record { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string NameGenerator { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime RecordDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            /*            var recordsQuery = from m in _context.Records
                                           where m.NameGenerator.Contains(NameGenerator) && m.RecordDate == RecordDate
                                           select m;*/

            var FirstRecord = await _context.Records
                .FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator /*&& m.RecordDate == RecordDate*/);

            Console.WriteLine(NameGenerator);

            if (FirstRecord != null)
            {

                var allRecords = from m in _context.Records
                                 where m.NameProduct.Contains(FirstRecord.NameProduct) &&
                                       m.NameStore.Contains(FirstRecord.NameStore) &&
                                       m.NameProvince.Contains(FirstRecord.NameProvince) &&
                                       m.NameCanton.Contains(FirstRecord.NameCanton)
                                 orderby m.NameProduct descending
                                 select m;

                Record = await allRecords
                    .ToListAsync();
            }
            else
            {
                Record = new List<Record>();
            }



            return Page();
        }
    }
}
