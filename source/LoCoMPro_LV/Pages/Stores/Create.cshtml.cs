using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LoCoMPro_LV.Pages.Stores
{
    public class CreateModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        /// <summary>
        /// Constructor de la clase IndexModel.
        /// </summary>
        /// <param name="context">Contexto de la base de datos de LoCoMPro.</param>
        public CreateModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Construye un método de llamado a record para pasar datos del HTML 
        /// </summary>
        [BindProperty]
        public Store Store { get; set; }


        /// <summary>
        /// Lista Hash de Store para almacenar los locales.
        /// </summary>
        public HashSet<string> Stores { get; set; }


        public async Task OnGetAsync()
        {
            var stores = await _context.Stores.ToListAsync();
            Stores = new HashSet<string>();
            foreach (var store in stores)
            {
                Stores.Add(store.NameStore);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
/*            var existingStore = await _context.Stores.FirstOrDefaultAsync(s =>
                s.NameStore == Store.NameStore &&
                s.NameProvince == Store.NameProvince &&
                s.NameCanton == Store.NameCanton &&
                s.Latitude == Store.Latitude &&
                s.Longitude == Store.Longitude);*/

            var newStore = new Store
                {
                    NameStore = Store.NameStore,
                    NameProvince = Store.NameProvince,
                    NameCanton = Store.NameCanton,
                    Latitude = Store.Latitude,
                    Longitude = Store.Longitude
                };
            _context.Stores.Add(newStore);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Records/Index");
        }
    }
}
