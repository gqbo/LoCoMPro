using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;

namespace LoCoMPro_LV.Pages.Stores
{
    /// <summary>
    /// P�gina Create de Stores para la creaci�n de nuevas tiendas relacionadas a un registro.
    /// </summary>
    public class CreateStoreModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;

         public CreateStoreModel(LoComproContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Construye un objeto de tipo Store para almacenar el contenido del HTML. 
        /// </summary>
        [BindProperty]
        public Store Store { get; set; }

        /// <summary>
        /// Lista Hash de Store para almacenar los locales.
        /// </summary>
        public HashSet<string> Stores { get; set; }

        /// <summary>
        /// M�todo invocado cuando se realiza una solicitud GET para la p�gina de "crear tienda". 
        /// Carga las tiendas encontradas en la base de datos y las almacena en una estructura de datos.
        /// </summary>
        public async Task OnGetAsync()
        {
            await LoadStoresAsync();
        }

        /// <summary>
        /// M�todo invocado cuando se realiza una solicitud POST para la p�gina de "crear tienda". 
        /// Recupera la informaci�n relacionada a la geolocalizaci�n y la transfiere a la p�gina "crear registro".
        /// </summary>
        public IActionResult OnPostAsync()
        {
            if (Store.NameCanton == "N/A" || Store.NameProvince == "N/A")
            {
                return Page();
            }
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
        /// Almacena en una estructuras de datos las tiendas encontradas en la base de datos.
        /// </summary>
        public async Task LoadStoresAsync()
        {
            var stores = await _context.Stores.ToListAsync();
            Stores = new HashSet<string>(stores.Select(store => store.NameStore));
        }
    }
}
