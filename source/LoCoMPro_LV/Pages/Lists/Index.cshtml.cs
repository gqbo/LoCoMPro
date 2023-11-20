using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using System.Collections;

namespace LoCoMPro_LV.Pages.Lists
{
    public class IndexModel : PageModel
    {
        private readonly LoComproContext _context;

        public IndexModel(LoComproContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string NameList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string NameProduct { get; set; }

        public IList<Listed> Listed { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var listed =  from listed_item in _context.Listed
                              where listed_item.NameList == User.Identity.Name
                              select listed_item;

            Listed = await listed.ToListAsync();
        }

        public async Task<IActionResult> OnPostEliminarItem()
        {
            var listed = await _context.Listed
                .FirstOrDefaultAsync(m => m.NameProduct == NameProduct && m.NameList == User.Identity.Name);

            if (listed != null)
            {
                _context.Listed.Remove(listed);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
