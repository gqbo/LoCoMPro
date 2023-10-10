using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Página de Crear de Records para la creación de nuevos registros registros.
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

        /// <summary>
        /// Constructor de la clase CreateModel.
        /// </summary>
        /// <param name="context">Contexto de la base de datos de LoCoMPro.</param>
        public CreateModel(LoCoMPro_LV.Data.LoComproContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Lista de provincias para la selección.
        /// </summary>
        public SelectList Provinces { get; set; }

        /// <summary>
        /// Diccionario de cantones..
        /// </summary>
        public Dictionary<string, List<string>> Cantons { get; set; }

        /// <summary>
        /// Lista Has de Store para almacenar los locales..
        /// </summary>
        public HashSet<string> Stores { get; set; }

        /// <summary>
        /// Lista para almecenar productos.
        /// </summary>
        public List<string> Product { get; set; }

        /// <summary>
        /// Lista para almacenar las categorias.
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// String para almacenar el nombre de usuario autentificado.
        /// </summary>
        public string AuthenticatedUserName { get; set; }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET para la página Create de registros. 
        /// Realiza una serie de tareas que incluyen el agrupamiento en listas y diccionarios de registros y
        /// la carga de datos relacionados desde la base de datos para la representación en la página web.
        /// </summary>
        /// <param name="Provinces">Recibe las provincias y almacena.</param>
        /// <param name="Cantons">Recibe y almacena los cantones.</param>
        /// <param name="Stores">Recibe y almacena los Stores.</param>
        /// <param name="Product">Recibe y almacena los productos.</param>
        /// <param name="Categories">Recibe y almacena las categorias.</param>
        /// <param name="AuthenticatedUserName">Recibe y almacena el nombre de usuario.</param>
        public async Task OnGetAsync()
        {
            var provinces = await _context.Provinces.ToListAsync();
            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");

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

            var stores = await _context.Stores.ToListAsync();
            Stores = new HashSet<string>();
            foreach (var store in stores)
            {
                Stores.Add(store.NameStore);
            }
            stores = stores.ToList();

            var products = await _context.Products.ToListAsync();
            Product = new List<string>();
            foreach (var prod in products)
            {
                Product.Add(prod.NameProduct);
            }

            var category = await _context.Categories.ToListAsync();
            Categories = new List<string>();

            foreach (var cat in category)
            {
                Categories.Add(cat.NameCategory);
            }

            if (User.Identity.IsAuthenticated)
            {
                AuthenticatedUserName = User.Identity.Name;
            }
        }

        /// <summary>
        /// Construye un metodo de llamado a record para pasar datos del HTML 
        /// </summary>
        [BindProperty]
        public Record Record { get; set; }

        /// <summary>
        /// Metodo que carga los datos ingresados por el usuario en la página Create de registros. 
        /// Realiza una serie de tareas que incluyer, validar datos, asignacion de datos a sus respectibas tablas.
        /// en la base de datos
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Records/Create");
            }

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

            var categoryName = Request.Form["NameCategory"].FirstOrDefault();

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.NameCategory == categoryName);

            if (existingCategory == null)
            {
                var newCategory = new Category
                {
                    NameCategory = categoryName,
                    NameTopCategory = null
                };
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
            }

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

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime dateTimeConverted = DateTime.ParseExact(currentDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Record.RecordDate = dateTimeConverted;

            _context.Records.Add(Record);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
