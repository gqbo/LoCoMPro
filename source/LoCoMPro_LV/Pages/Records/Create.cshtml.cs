using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoCoMPro_LV.Pages.Records
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
            return Page();
        }

        [BindProperty]
        public Record Record { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            

            _context.Records.Add(Record);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}