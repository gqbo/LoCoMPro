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


namespace LoCoMPro_LV.Pages.Records
{
    public class CreateModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public CreateModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        //  public IList<Record> Record { get; set; } = default!;
        public SelectList Provinces { get; set; }


        //public List<string> Cantons { get; set; }
        public Dictionary<string, List<string>> Cantons { get; set; }
        //public SelectList Cantons { get; set; }


        public async Task OnGetAsync()
        {

            // Se cargan las listas de provincias, cantones y las diferentes categorías
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

            // Captura la hora automaticamente
            Record.RecordDate = DateTime.Now;

            // Solicitar info del usuario registrado

                     // Record.GeneratorUser = XXXXXXXXXX

            _context.Records.Add(Record);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}