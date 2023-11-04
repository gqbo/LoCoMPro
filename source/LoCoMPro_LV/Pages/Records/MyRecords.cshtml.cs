using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Data;

namespace LoCoMPro_LV.Pages.Records
{
    public class MyRecordsModel : PageModel
    {   
        private readonly LoComproContext _context;
        public MyRecordsModel(LoComproContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Record> Records { get; set; }

        public async Task OnGetAsync()
        {
            string authenticatedUserName = User.Identity.Name;
            Records = await _context.Records
                .Include(r => r.Store)
                .Where(r => r.NameGenerator == authenticatedUserName)
                .ToListAsync();
        }
    }
}