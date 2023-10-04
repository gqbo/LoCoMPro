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

            // Permite guardar locales en la tabla de locales donde se incluye el local, provincia y canton.
            var newStore = new Store
            {
                NameStore = Record.NameStore,
                NameProvince = Record.NameProvince,
                NameCanton = Record.NameCanton
            };

            _context.Stores.Add(newStore);
            await _context.SaveChangesAsync();


            Record.NameStore = newStore.NameStore;

            // Permite guardar productos en la tabla de productos y en los registros nuevos.
            var newProduct = new Product
            {
                NameProduct = Record.NameProduct
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            Record.NameProduct = newProduct.NameProduct;



            _context.Records.Add(Record);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}