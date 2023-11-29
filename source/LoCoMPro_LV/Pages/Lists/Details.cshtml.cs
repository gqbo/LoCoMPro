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
    public class DetailsModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public DetailsModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

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
    }
}
