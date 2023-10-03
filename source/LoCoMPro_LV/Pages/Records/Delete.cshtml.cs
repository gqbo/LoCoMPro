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
    public class DeleteModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public DeleteModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Record Record { get; set; }

        public async Task<IActionResult> OnGetAsync(string NameGenerator, DateTime RecordDate)
        {
            if (NameGenerator == null || RecordDate == null || _context.Records == null)
            {
                return NotFound();
            }

            var record = await _context.Records.FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator && m.RecordDate == RecordDate);

            if (record == null)
            {
                return NotFound();
            }
            else
            {
                Record = record;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string NameGenerator, DateTime RecordDate)
        {
            if (NameGenerator == null || RecordDate == null || _context.Records == null)
            {
                return NotFound();
            }
            var record = await _context.Records.FindAsync(NameGenerator, RecordDate);

            if (record != null)
            {
                Record = record;
                _context.Records.Remove(Record);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
