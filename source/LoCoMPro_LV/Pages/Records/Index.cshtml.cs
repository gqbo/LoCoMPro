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
    public class IndexModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public IndexModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        public IList<Record> Record { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Records != null)
            {
                Record = await _context.Records
                .Include(r => r.GeneratorUser)
                .Include(r => r.Product)
                .Include(r => r.Store).ToListAsync();
            }
        }
    }
}
