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
    public class IndexModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public IndexModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        public IList<Record> Record { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public SelectList Provinces { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchProvince { get; set; }

        public SelectList Cantons { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchCanton { get; set; }

        public SelectList Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchCategory { get; set; }

        public async Task OnGetAsync()
        {
            // Se cargan las listas de provincias, cantones y las diferentes categorías
            var provinces = await _context.Provinces.ToListAsync();
            var cantons = await _context.Cantons.ToListAsync();
            var categories = await _context.Associated
                                    .Select(a => a.NameCategory)
                                    .Distinct()
                                    .ToListAsync();

            // Se crea un SelectList para provincias y categorías
            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");
            /*Cantons = new SelectList(cantons, "NameCanton", "NameCanton");*/
            Categories = new SelectList(categories);

            // Resto de la lógica de búsqueda
            var recordsQuery = from m in _context.Records
                               select m;

            // Se solicitan los distintos datos a la base de datos, dependiendo de lo buscado.
            if (!string.IsNullOrEmpty(SearchString))
            {
                recordsQuery = recordsQuery.Where(s => s.NameProduct.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(SearchProvince))
            {
                recordsQuery = recordsQuery.Where(s => s.NameProvince == SearchProvince);
            }

            if (!string.IsNullOrEmpty(SearchCanton))
            {
                recordsQuery = recordsQuery.Where(s => s.NameCanton == SearchCanton);
            }

            if (!string.IsNullOrEmpty(SearchCategory))
            {
                recordsQuery = recordsQuery.Where(s => s.Product.Associated.Any(c => c.NameCategory == SearchCategory));
            }

            Record = await recordsQuery
                .Include(r => r.GeneratorUser)
                .Include(r => r.Product)
                .Include(r => r.Store)
                .Include(r => r.Store.Canton.Province)
                .Include(r => r.Product.Associated)
                .ToListAsync();
        }
    }
}
