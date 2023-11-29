using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Pages.Lists
{
    public class DeleteModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public DeleteModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        [BindProperty]
      public List List { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.List == null)
            {
                return NotFound();
            }

            var list = await _context.List.FirstOrDefaultAsync(m => m.NameList == id);

            if (list == null)
            {
                return NotFound();
            }
            else 
            {
                List = list;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.List == null)
            {
                return NotFound();
            }
            var list = await _context.List.FindAsync(id);

            if (list != null)
            {
                List = list;
                _context.List.Remove(List);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
