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
        
        /// <summary>
        /// Proporciona acceso a la configuración de la aplicación, como valores definidos en appsettings.json.
        /// </summary>
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
        /// Representa una lista paginada de registros para su visualización en la página.
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
        /// Representa el orden actual en el que se deben mostrar los registros en la página.
        /// </summary>
        public string CurrentSort { get; set; }

        /// <summary>
        /// Método que se ejecuta cuando se carga la página y se realiza la búsqueda y paginación de registros.
        /// </summary>
        /// <param name="sortOrder">El orden en el que se deben mostrar los registros.</param>
        /// <param name="pageIndex">El número de página actual.</param>
        public async Task OnGetAsync(string sortOrder, int? pageIndex)
        {
            await InitializeSortingAndSearching(sortOrder);
            var orderedRecordsQuery = BuildOrderedRecordsQuery();
            var orderedGroupsQuery = ApplySorting(orderedRecordsQuery, sortOrder);
            var totalCount = await orderedGroupsQuery.CountAsync();
            var pageSize = Configuration.GetValue("PageSize", 10);
            Records = await PaginatedList<Record>.CreateAsync(
                orderedGroupsQuery.Select(group => group.OrderByDescending(r => r.RecordDate).FirstOrDefault()), pageIndex ?? 1, pageSize);
        }
        
        /// <summary>
        /// Inicializa las opciones de ordenamiento y los datos de búsqueda avanzada.
        /// </summary>
        /// <param name="sortOrder">El orden en el que se deben mostrar los registros.</param>
        private async Task InitializeSortingAndSearching(string sortOrder)
        {
            CurrentSort = sortOrder;
            DateTimeSort = sortOrder == "Date" ? "date_desc" : "Date";
            PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

            var provinces = await _context.Provinces.ToListAsync();
            var cantons = await _context.Cantons.ToListAsync();
            var categories = await _context.Associated
                .Select(a => a.NameCategory)
                .Distinct()
                .ToListAsync();

            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");
            Cantons = new SelectList(cantons, "NameCanton", "NameCanton");
            Categories = new SelectList(categories);
        }

        /// <summary>
        /// Construye la consulta de registros ordenados basada en las opciones de búsqueda.
        /// </summary>
        /// <returns>Consulta de registros ordenados.</returns>
        private IQueryable<Record> BuildOrderedRecordsQuery()
        {
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

            return orderedRecordsQuery;
        }

        /// <summary>
        /// Aplica el ordenamiento a la consulta de registros.
        /// </summary>
        /// <param name="orderedRecordsQuery">Consulta de registros ordenados.</param>
        /// <param name="sortOrder">El orden en el que se deben mostrar los registros.</param>
        /// <returns>Consulta de registros ordenados con el orden especificado.</returns>
        private IOrderedQueryable<IGrouping<object, Record>> ApplySorting(IQueryable<Record> orderedRecordsQuery, string sortOrder)
        {
            var groupedRecordsQuery = from record in orderedRecordsQuery
                                      group record by new
                                      {
                                          record.NameProduct,
                                          record.NameStore,
                                          record.NameCanton,
                                          record.NameProvince
                                      } into recordGroup
                                      orderby recordGroup.Key.NameProduct descending
                                      select recordGroup;

            switch (sortOrder)
            {
                case "Date":
                    return groupedRecordsQuery.OrderBy(group => group.Max(record => record.RecordDate));
                case "Price":
                    return groupedRecordsQuery.OrderBy(group => group.Max(record => record.Price));
                case "price_desc":
                    return groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.Price));
                default:
                    return groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.RecordDate));
            }
        }
    }
}
