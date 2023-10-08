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

namespace LoCoMPro_LV.Pages.Records
{
    public class CreateModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public CreateModel(LoCoMPro_LV.Data.LoComproContext context, SignInManager<ApplicationUser> signInManager)
        {
             _context = context;
            _signInManager = signInManager;
        }


        // Listas para almacenar los datos de las tablas y validar las funciones nuevas
        public SelectList Provinces { get; set; }
        public Dictionary<string, List<string>> Cantons { get; set; }
        public HashSet<string> Stores { get; set; }
        public List<string> Product { get; set; }
        public List<string> Categories { get; set; }
        public string AuthenticatedUserName { get; set; }
        public async Task OnGetAsync()
        {
            // Se cargan las listas de Provincias
            var provinces = await _context.Provinces.ToListAsync();
            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");

            // Se cargan las listas de Cantones
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

            // Se cargan las listas de Store
            var stores = await _context.Stores.ToListAsync();
            Stores = new HashSet<string>();
            foreach (var store in stores)
            {
                Stores.Add(store.NameStore);
            }
            stores = stores.ToList();

            // Se cargan las listas de Product
            var products = await _context.Products.ToListAsync();
            Product = new List<string>();
            foreach (var prod in products)
            {
                Product.Add(prod.NameProduct);
            }

            // Se cargan las lista de Category
            var category = await _context.Categories.ToListAsync();
            Categories = new List<string>();

            foreach (var cat in category)
            {
                Categories.Add(cat.NameCategory);
            }

            //  -----------------------------------
            if (User.Identity.IsAuthenticated)
            {
                AuthenticatedUserName = User.Identity.Name;
            }

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
            

            // Permite guardar productos en la tabla de productos y en los registros nuevos.
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.NameProduct == Record.NameProduct);
            if (existingProduct != null)
            {
                // Si el producto ya existe, usa el producto existente en lugar de crear uno nuevo
                Record.Product = existingProduct;
            }
            else
            {
                // Si el producto no existe, crea uno nuevo y agrégalo al registro
                var newProduct = new Product
                {
                    NameProduct = Record.NameProduct
                };
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                Record.Product = newProduct;
            }

            // Captura las categorias y las almacena en associate y categories
            var categoryName = Request.Form["NameCategory"].FirstOrDefault();

            // Verificar si la categoría ya existe en la base de datos
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.NameCategory == categoryName);

            // Crear una nueva categoría solo si no existe
            if (existingCategory == null)
            {
                // La categoría no existe, crea una nueva categoría y guárdala en la base de datos
                var newCategory = new Category
                {
                    NameCategory = categoryName, // Usar la categoría ingresada por el usuario
                    NameTopCategory = null // Establecer NameTopCategory en null para la nueva categoría
                };
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync(); // Guardar cambios en la base de datos
            }

            // Crear una nueva asociación en la tabla Associated
           // arreglar los duplicados
           var existingAssociated = await _context.Associated.FirstOrDefaultAsync(a =>
                a.NameProduct == Record.NameProduct &&
                a.NameCategory == categoryName);

            // Crear un nuevo registro solo si no existe
            if (existingAssociated == null)
            {
                var newAssociated = new Associated
                {
                    NameProduct = Record.NameProduct,
                    NameCategory = categoryName
                };
                _context.Associated.Add(newAssociated);
                await _context.SaveChangesAsync(); // Guardar cambios en la base de datos
            }

            // Captura la hora automaticamente
            Record.RecordDate = DateTime.Now;

            _context.Records.Add(Record);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
