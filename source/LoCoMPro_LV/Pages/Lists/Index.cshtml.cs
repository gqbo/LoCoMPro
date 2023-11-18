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
    public class IndexModel : PageModel
    {
        private readonly LoComproContext _context;

        public IndexModel(LoComproContext context)
        {
            _context = context;
        }

        public IList<List> List { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.List != null)
            {
                List = await _context.List
                .Include(l => l.User).ToListAsync();
            }
        }
    }
}
