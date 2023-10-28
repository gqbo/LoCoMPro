using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Pages.Reports
{
    public class DeleteModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public DeleteModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Report Report { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FirstOrDefaultAsync(m => m.NameGenerator == id);

            if (report == null)
            {
                return NotFound();
            }
            else 
            {
                Report = report;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }
            var report = await _context.Reports.FindAsync(id);

            if (report != null)
            {
                Report = report;
                _context.Reports.Remove(Report);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
