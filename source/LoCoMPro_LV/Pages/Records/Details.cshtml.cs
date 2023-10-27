using LoCoMPro_LV.Data;
using LoCoMPro_LV.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Página de detalles de producto, en donde se ven los registros relacionados a un mismo producto.
    /// </summary>
    public class DetailsModel : PageModel
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
        /// Constructor de la clase DetailsModel.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de LoCoMPro.</param>
        /// <param name="configuration">Proporciona acceso a la configuración de la aplicación, como valores definidos en appsettings.json.</param>
        public DetailsModel(LoComproContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        /// <summary>
        /// Representa una lista paginada de registros para su visualización en la página.
        /// </summary>
        public PaginatedList<RecordStoreModel> Records { get; set; }

        /// <summary>
        /// Nombre del usuario generador del registro seleccionado en la pantalla index, que se utiliza para buscar todos los registros relacionados.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameGenerator { get; set; }

        /// <summary>
        /// Fecha en la que se realizó el registro seleccionado en la pantalla index, que se utiliza para buscar todos los registros relacionados.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// Método utilizado cuando en la pantalla de resultados de la búsqueda se selecciona un producto para abrir el detalle. Este método
        /// utiliza el nombre del generador y la fecha de realización del registro más reciente del producto para realizar la consulta por todos
        /// de todos los registros con ese producto en esa tienda en específico.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {

            var FirstRecord = await _context.Records
                .FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator && m.RecordDate == RecordDate);

            var pageSize = Configuration.GetValue("PageSize", 10);

            if (FirstRecord != null)
            {

                var allRecords = from record in _context.Records
                                 join store in _context.Stores
                                 on new { record.NameStore, record.Latitude, record.Longitude }
                                 equals new { store.NameStore, store.Latitude, store.Longitude }
                                 where record.NameProduct.Contains(FirstRecord.NameProduct) &&
                                       record.Latitude == FirstRecord.Latitude &&
                                       record.Longitude == FirstRecord.Longitude
                                 orderby record.RecordDate descending
                                 select new RecordStoreModel
                                 {
                                     Record = record,
                                     Store = store
                                 };
                var totalCount = await allRecords.CountAsync(); // Contiene el número total de registros.

                Records = await PaginatedList<RecordStoreModel>.CreateAsync(
                    allRecords, pageIndex ?? 1, pageSize);
            }
            else
            {
                Records = new PaginatedList<RecordStoreModel>(new List<RecordStoreModel>(), 0, pageIndex ?? 1, pageSize);
            }

            return Page();
        }
    }
}
