using LoCoMPro_LV.Models;
using LoCoMPro_LV.Utils;
using LoCoMPro_LV.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        private readonly LoComproContext _context;

        private readonly IConfiguration Configuration;


        /// <summary>
        /// Constructor de la clase IndexModel.
        /// </summary>
        /// <param name="context">Contexto de la base de datos de LoCoMPro.</param>
        public IndexModel(LoComproContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        /// <summary>
        /// Lista de tipo "Records", que almacena los registros correspondientes al producto buscado.
        /// </summary>
        public PaginatedList<Record> Records { get; set; }

        /// <summary>
        /// Cadena de caracteres que se utiliza para filtrar la búsqueda por nombre del producto.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        /// <summary>
        /// Provincia utilizada como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchProvince { get; set; }

        /// <summary>
        /// Lista de provincias para la selección.
        /// </summary>
        public SelectList Provinces { get; set; }

        /// <summary>
        /// Cantón utilizado como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchCanton { get; set; }

        /// <summary>
        /// Lista de cantones para la selección.
        /// </summary>
        public SelectList Cantons { get; set; }

        /// <summary>
        /// Categoría utilizada como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchCategory { get; set; }

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

        public string CurrentSort { get; set; }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET para la página de índice de registros. 
        /// Realiza una serie de tareas que incluyen el ordenamiento y agrupamiento de registros y
        /// la carga de datos relacionados desde la base de datos para la representación en la página web.
        /// </summary>
        /// <param name="sortOrder">Indica el orden en el que se deben mostrar los registros (por fecha o precio).</param>
        /// <param name="currentFilter">El filtro actual aplicado para la búsqueda de registros.</param>
        /// <param name="searchString">Indica la búsqueda introducida por el usuario para filtrar los registros por nombre de producto.</param>
        /// <param name="pageIndex">El índice de la página actual en caso de paginación.</param>
        public async Task OnGetAsync(string sortOrder, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            DateTimeSort = sortOrder == "Date" ? "date_desc" : "Date";
            PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

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
                    orderedGroupsQuery = orderedGroupsQuery.OrderBy(group => group.Max(record => record.RecordDate));
                    break;
                case "Price":
                    orderedGroupsQuery = orderedGroupsQuery.OrderBy(group => group.Max(record => record.Price));
                    break;
                case "price_desc":
                    orderedGroupsQuery = orderedGroupsQuery.OrderByDescending(group => group.Max(record => record.Price));
                    break;
                default:
                    orderedGroupsQuery = orderedGroupsQuery.OrderByDescending(group => group.Max(record => record.RecordDate));
                    break;
            }
            var totalCount = await orderedGroupsQuery.CountAsync(); // Contiene el número total de registros.
            
            var pageSize = Configuration.GetValue("PageSize", 10);
            Records = await PaginatedList<Record>.CreateAsync(
                orderedGroupsQuery.Select(group => group.OrderByDescending(r => r.RecordDate).FirstOrDefault()), pageIndex ?? 1, pageSize);
        }
    }
}
