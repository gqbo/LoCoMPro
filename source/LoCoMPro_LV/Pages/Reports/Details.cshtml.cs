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
    public class DetailsModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public DetailsModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

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
    }
}
