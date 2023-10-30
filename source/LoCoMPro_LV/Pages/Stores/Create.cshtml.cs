using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LoCoMPro_LV.Pages.Stores
{
    public class CreateStoreModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        /// <summary>
        /// Constructor de la clase IndexModel.
        /// </summary>
        /// <param name="context">Contexto de la base de datos de LoCoMPro.</param>
        public CreateStoreModel(LoCoMPro_LV.Data.LoComproContext context)
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
            await LoadStoresAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("../Records/Create", new
            {
                latitude = Store.Latitude,
                longitude = Store.Longitude,
                nameStore = Store.NameStore,
                nameProvince = Store.NameProvince,
                nameCanton = Store.NameCanton
            });
        }


        /// <summary>
        /// Permite almacenar los locales en una colección de datos.
        /// </summary>
        private async Task LoadStoresAsync()
        {
            var stores = await _context.Stores.ToListAsync();
            Stores = new HashSet<string>(stores.Select(store => store.NameStore));
        }
    }
}
