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

namespace LoCoMPro_LV.Pages.Records
{
    public class EditModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public EditModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Record Record { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string NameGenerator, DateTime RecordDate)
        {
            if (NameGenerator == null || _context.Records == null)
            {
                return NotFound();
            }

            var record = await _context.Records.FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator && m.RecordDate == RecordDate);
            if (record == null)
            {
                return NotFound();
            }
            Record = record;
           ViewData["NameGenerator"] = new SelectList(_context.GeneratorUsers, "UserName", "UserName");
           ViewData["NameProduct"] = new SelectList(_context.Products, "NameProduct", "NameProduct");
           ViewData["NameStore"] = new SelectList(_context.Stores, "NameStore", "NameStore");
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

            _context.Attach(Record).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordExists(Record.NameGenerator, Record.RecordDate))
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

        private bool RecordExists(string NameGenerator, DateTime RecordDate)
        {
            return _context.Records.Any(e => e.NameGenerator == NameGenerator && e.RecordDate == RecordDate);
        }
    }
}
