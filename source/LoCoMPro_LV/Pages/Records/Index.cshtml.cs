using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Página de índice de Records para la búsqueda y visualización de registros.
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
        /// Indica el orden en el que se deben mostrar los registros por fecha.
        /// </summary>
        public string DateTimeSort { get; set; }

        /// <summary>
        /// Indica el orden en el que se deben mostrar los registros por precio.
        /// </summary>
        public string PriceSort { get; set; }

        /// <summary>
        /// El filtro actual aplicado para la búsqueda de registros.
        /// </summary>
        public string CurrentFilter { get; set; }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET para la página de índice de registros. 
        /// Realiza una serie de tareas que incluyen el ordenamiento y agrupamiento de registros y
        /// la carga de datos relacionados desde la base de datos para la representación en la página web.
        /// </summary>
        /// <param name="sortOrder">Indica el orden en el que se deben mostrar los registros (por fecha o precio).</param>
        /// <param name="currentFilter">El filtro actual aplicado para la búsqueda de registros.</param>
        /// <param name="searchString">Indica la búsqueda introducida por el usuario para filtrar los registros por nombre de producto.</param>
        /// <param name="pageIndex">El índice de la página actual en caso de paginación.</param>
        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            DateTimeSort = sortOrder == "Date" ? "date_desc" : "Date";
            PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            CurrentFilter = searchString;

            var provinces = await _context.Provinces.ToListAsync();
            var cantons = await _context.Cantons.ToListAsync();
            var categories = await _context.Associated
                                    .Select(a => a.NameCategory)
                                    .Distinct()
                                    .ToListAsync();

            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");
            Cantons = new SelectList(cantons, "NameCanton", "NameCanton");
            Categories = new SelectList(categories);

            IQueryable<Record> orderedRecordsQuery = from m in _context.Records
                               select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.NameProduct.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(SearchProvince))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.NameProvince == SearchProvince);
            }

            if (!string.IsNullOrEmpty(SearchCanton))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.NameCanton == SearchCanton);
            }

            if (!string.IsNullOrEmpty(SearchCategory))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Product.Associated.Any(c => c.NameCategory == SearchCategory));
            }

            var groupedRecordsQuery = from record in orderedRecordsQuery
                                      group record by new
                                      { record.NameProduct, record.NameStore, record.NameCanton, record.NameProvince } into recordGroup
                                      orderby recordGroup.Key.NameProduct descending
                                      select recordGroup;

            var orderedGroupsQuery = groupedRecordsQuery;

            switch (sortOrder)
            {
                case "Date":
                    orderedGroupsQuery = groupedRecordsQuery.OrderBy(group => group.Max(record => record.RecordDate));
                    break;
                case "Price":
                    orderedGroupsQuery = groupedRecordsQuery.OrderBy(group => group.Max(record => record.Price));
                    break;
                case "price_desc":
                    orderedGroupsQuery = groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.Price));
                    break;
                default:
                    orderedGroupsQuery = groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.RecordDate));
                    break;
            }

            Record = await orderedGroupsQuery
                .Select(group => group.OrderByDescending(r => r.RecordDate).FirstOrDefault())
                .ToListAsync();

            SearchCanton = Request.Query["SearchCanton"];
        }
    }
}
