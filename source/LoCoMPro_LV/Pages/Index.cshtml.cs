using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoCoMPro_LV.Pages
{
    /// <summary>
    /// Página de inicio de LoCoMPro.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        /// <summary>
        /// Constructor de la clase IndexModel.
        /// </summary>
        /// <param name="context">Contexto de la base de datos de LoCoMPro.</param>
        public IndexModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista de tipo "Record", que almacena los registros correspondientes al producto buscado.
        /// </summary>
        public IList<Record> Record { get; set; } = default!;

        /// <summary>
        /// Cadena de caracteres que se utiliza para filtrar la búsqueda por nombre del producto.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        /// <summary>
        /// Provincia utilizada como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? SearchProvince { get; set; }

        /// <summary>
        /// Lista de provincias para la selección.
        /// </summary>
        public SelectList Provinces { get; set; }

        /// <summary>
        /// Cantón utilizado como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? SearchCanton { get; set; }

        /// <summary>
        /// Lista de cantones para la selección.
        /// </summary>
        public SelectList Cantons { get; set; }

        /// <summary>
        /// Categoría utilizada como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? SearchCategory { get; set; }

        /// <summary>
        /// Lista de categorías para la selección.
        /// </summary>
        public SelectList Categories { get; set; }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET que redirige a la página de índice de registros. 
        /// Realiza la carga de datos con los parámetros deseados relacionados en la base de datos.
        /// </summary>
        public async Task OnGetAsync()
        {
            var provinces = await _context.Provinces.ToListAsync();
            /*var cantons = await _context.Cantons.ToListAsync();*/
            var categories = await _context.Associated
                                    .Select(a => a.NameCategory)
                                    .Distinct()
                                    .ToListAsync();

            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");
            /*Cantons = new SelectList(cantons, "NameCanton", "NameCanton");*/
            Categories = new SelectList(categories);

            var recordsQuery = from m in _context.Records
                               select m;

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