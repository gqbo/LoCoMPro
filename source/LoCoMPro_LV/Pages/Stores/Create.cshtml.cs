using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LoCoMPro_LV.Pages.Stores
{
    public class CreateStoreModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        /// <summary>
        /// Constructor de la clase IndexModel.
        /// </summary>
         public CreateStoreModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Construye un objeto de tipo Store para almacenar el contenido del HTML. 
        /// </summary>
        [BindProperty]
        public Store Store { get; set; }

        [BindProperty]
        public string NameProvinceInput { get; set; }

        [BindProperty]
        public string NameCantonInput { get; set; }

        /// <summary>
        /// Lista que donde se almacena las provincias que se encuentran en la BD.
        /// </summary>
        public SelectList Provinces { get; set; }

        /// <summary>
        /// Diccionario donde se almacena los cantones  asociados a las provincias que se encuentran en la BD.
        /// </summary>
        public Dictionary<string, List<string>> Cantons { get; set; }

        /// <summary>
        /// Lista Hash de Store para almacenar los locales.
        /// </summary>
        public HashSet<string> Stores { get; set; }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET para la página de "crear tienda". 
        /// Carga las tiendas encontradas en la base de datos y las almacena en una estructura de datos.
        /// </summary>
        public async Task OnGetAsync()
        {
            await LoadProvincesAsync();
            await LoadCantonsAsync();
            await LoadStoresAsync();
        }
        /// <summary>
        /// Método invocado cuando se realiza una solicitud POST para la página de "crear tienda". 
        /// Recupera la información relacionada a la geolocalización y la transfiere a la página "crear registro".
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
        /// Permite extraer las provincias y almacenarlas en una lista.
        /// </summary>
        private async Task LoadProvincesAsync()
        {
            var provinces = await _context.Provinces.OrderBy(c => c.NameProvince).ToListAsync();
            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");
        }

        /// <summary>
        /// Permite extraer los cantones y almacenarlos en un diccionario de provincias con los respectivos cantones .
        /// </summary>
        private async Task LoadCantonsAsync()
        {
            var cantons = await _context.Cantons.OrderBy(c => c.NameCanton).ToListAsync();
            Cantons = new Dictionary<string, List<string>>();
            foreach (var canton in cantons)
            {
                if (!Cantons.ContainsKey(canton.NameProvince))
                {
                    Cantons[canton.NameProvince] = new List<string>();
                }
                Cantons[canton.NameProvince].Add(canton.NameCanton);
            }
        }

        /// <summary>
        /// Almacena en una estructuras de datos las tiendas encontradas en la base de datos.
        /// </summary>
        private async Task LoadStoresAsync()
        {
            var stores = await _context.Stores.ToListAsync();
            Stores = new HashSet<string>(stores.Select(store => store.NameStore));
        }
    }
}
