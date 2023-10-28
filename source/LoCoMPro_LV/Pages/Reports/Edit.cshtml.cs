using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Pages.Reports
{
    public class EditModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public EditModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Report Report { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }

            var report =  await _context.Reports.FirstOrDefaultAsync(m => m.NameGenerator == id);
            if (report == null)
            {
                return NotFound();
            }
            Report = report;
           ViewData["NameReporter"] = new SelectList(_context.GeneratorUsers, "UserName", "UserName");
           ViewData["NameGenerator"] = new SelectList(_context.Records, "NameGenerator", "NameGenerator");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(Report.NameGenerator))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReportExists(string id)
        {
          return _context.Reports.Any(e => e.NameGenerator == id);
        }
    }
}
