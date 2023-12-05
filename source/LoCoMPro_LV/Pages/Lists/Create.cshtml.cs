using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Pages.Lists
{
    public class CreateModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public CreateModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserName"] = new SelectList(_context.GeneratorUsers, "UserName", "UserName");
            return Page();
        }

        [BindProperty]
        public List List { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.List.Add(List);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
