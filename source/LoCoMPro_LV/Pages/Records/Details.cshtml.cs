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
    public class DetailsModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public DetailsModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        public Record Record { get; set; }

        public async Task<IActionResult> OnGetDetails(string NameGenerator)
        {
            if (NameGenerator == null || _context.Records == null)
            {
                return NotFound();
            }

            var record = await _context.Records.FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator);

            if (record == null)
            {
                return NotFound();
            }

            Record = record;
            return Page();
        }
    }
}
