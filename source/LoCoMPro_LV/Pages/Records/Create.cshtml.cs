using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Página Create de Records para la creación de nuevos registros registros.
    /// </summary>
    public class CreateModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoCoMPro_LV.Data.LoComproContext _context;
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro sección de registros.
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CreateModel(LoCoMPro_LV.Data.LoComproContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET para crear registros. 
        /// Realiza una serie de llamados a los diferentes métodos encargados de obtener la información de la base de datos.
        /// </summary>
        public async Task OnGetAsync()
        {
            await LoadProvincesAsync();
            await LoadCantonsAsync();
            await LoadStoresAsync();
            await LoadProductsAsync();
            await LoadCategoriesAsync();
            LoadAuthenticatedUserName();
        }

        /// <summary>
        /// Enlazar los valores de las propiedades en un objeto con los datos provenientes de una solicitud HTTP.
        /// </summary>
        [BindProperty]
        public Record Record { get; set; }

        /// <summary>
        /// Método que carga los datos ingresados por el usuario a los registros y a las diferentes tablas de la base de datos. 
        /// Realiza una serie de llamados que validan la consistencia de los datos que se desean añadir en la base de datos.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Record.NameGenerator = User.Identity.Name;
                return Page();
            }
            await ProcessStore();
            await ProcessProduct();
            await ProcessCategory();
            await ProcessAssociated();
            Record.RecordDate = Record.RecordDate = GetCurrentDateTime();

            _context.Records.Add(Record);
            await _context.SaveChangesAsync();
            return RedirectToPage("../Index");
        }

        /// <summary>
        /// Lista que donde se almacena las provincias que se encuentran en la BD.
        /// </summary>
        public SelectList Provinces { get; set; }

        /// <summary>
        /// Diccionario donde se almacena los cantones  asociados a las provincias que se encuentran en la BD.
        /// </summary>
        public Dictionary<string, List<string>> Cantons { get; set; }

        /// <summary>
        /// Colección de datos donde se almacena los locales que se encuentran en la BD.
        /// </summary>
        public HashSet<string> Stores { get; set; }

        /// <summary>
        /// Lista donde se almacena los productos que se encuentran en la BD.
        /// </summary>
        public List<string> Product { get; set; }

        /// <summary>
        /// Lista donde se almacena las categorías que se encuentran en la BD.
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// String donde se almacenar el usuario que se encuentran autenticado.
        /// </summary>
        public string AuthenticatedUserName { get; set; }

        /// <summary>
        /// Permite extraer las provincias y almacenarlas en una lista.
        /// </summary>
        private async Task LoadProvincesAsync()
        {
            var provinces = await _context.Provinces.ToListAsync();
            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");
        }

        /// <summary>
        /// Permite extraer los cantones y almacenarlos en un diccionario de provincias con los respectivos cantones .
        /// </summary>
        private async Task LoadCantonsAsync()
        {
            var cantons = await _context.Cantons.ToListAsync();
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
        /// Permite almacenar los locales en una colección de datos.
        /// </summary>
        private async Task LoadStoresAsync()
        {
            var stores = await _context.Stores.ToListAsync();
            Stores = new HashSet<string>(stores.Select(store => store.NameStore));
        }

        /// <summary>
        /// Permite obtener y almacenarlos productos en una lista.
        /// </summary>
        private async Task LoadProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            Product = products.Select(prod => prod.NameProduct).ToList();
        }

        /// <summary>
        /// Permite obtener y almacenar las categorías en una lista.
        /// </summary>
        private async Task LoadCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            Categories = categories.Select(cat => cat.NameCategory).ToList();
        }

        /// <summary>
        /// Permite obtener en un String el nombre del usuario autentificado.
        /// </summary>
        private void LoadAuthenticatedUserName()
        {
            if (User.Identity.IsAuthenticated)
            {
                AuthenticatedUserName = User.Identity.Name;
            }
        }

        /// <summary>
        /// Valida que repita un local a la hora de almacenarlo en la BD.
        /// </summary>
        private async Task ProcessStore()
        {
            var existingStore = await _context.Stores.FirstOrDefaultAsync(s =>
                s.NameStore == Record.NameStore &&
                s.NameProvince == Record.NameProvince &&
                s.NameCanton == Record.NameCanton);

            if (existingStore != null)
            {
                Record.Store = existingStore;
            }
            else
            {
                var newStore = new Store
                {
                    NameStore = Record.NameStore,
                    NameProvince = Record.NameProvince,
                    NameCanton = Record.NameCanton
                };
                _context.Stores.Add(newStore);
                await _context.SaveChangesAsync();
                Record.NameStore = newStore.NameStore;
            }
        }

        /// <summary>
        /// Valida que no se repita el producto a almacenar en la BD.
        /// </summary>
        private async Task ProcessProduct()
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.NameProduct == Record.NameProduct);

            if (existingProduct != null)
            {
                Record.Product = existingProduct;
            }
            else
            {
                var newProduct = new Product
                {
                    NameProduct = Record.NameProduct
                };
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                Record.Product = newProduct;
            }
        }

        /// <summary>
        /// Valida que no se repita una categoría a la hora de almacenarlo en la BD.
        /// </summary>
        private async Task ProcessCategory()
        {
            var categoryName = Request.Form["NameCategory"].FirstOrDefault();
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.NameCategory == categoryName);

            if (existingCategory == null)
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    return;
                }
                else
                {
                    var newCategory = new Category
                    {
                        NameCategory = categoryName,
                        NameTopCategory = null
                    };
                    _context.Categories.Add(newCategory);
                    await _context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Valida que no repita un asociación entre categoría y producto a la hora de almacenarlo en la BD.
        /// </summary>
        private async Task ProcessAssociated()
        {
            var categoryName = Request.Form["NameCategory"].FirstOrDefault();
            var existingAssociated = await _context.Associated.FirstOrDefaultAsync(a =>
                a.NameProduct == Record.NameProduct &&
                a.NameCategory == categoryName);

            if (existingAssociated == null)
            {
                var newAssociated = new Associated
                {
                    NameProduct = Record.NameProduct,
                    NameCategory = categoryName
                };
                _context.Associated.Add(newAssociated);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Método que verifica la hora actual para almacenarla en la BD.
        /// </summary>
        private DateTime GetCurrentDateTime()
        {
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return DateTime.ParseExact(currentDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
