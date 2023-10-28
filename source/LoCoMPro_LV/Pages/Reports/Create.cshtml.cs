using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Pages.Records;

namespace LoCoMPro_LV.Pages.Reports
{
    /// <summary>
    /// Página de detalles de producto, en donde se ven los registros relacionados a un mismo producto.
    /// </summary>
    public class CreateModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;

        /// <summary>
        /// Constructor de la clase DetailsModel.
        /// </summary>
        /// <param name="context">Contexto de la base de datos de LoCoMPro.</param>
        public CreateModel(LoComproContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista de tipo "Record", que almacena los registros correspondientes al producto que se selecciono para ver en detalle.
        /// </summary>
        public List<RecordStoreModel> Records { get; set; } = new List<RecordStoreModel>();

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
        public async Task<IActionResult> OnGetAsync()
        {
            var firstRecordQuery = from record in _context.Records
                                   where record.NameGenerator == NameGenerator && record.RecordDate == RecordDate
                                   join store in _context.Stores on new { record.NameStore, record.Latitude, record.Longitude }
                                                              equals new { store.NameStore, store.Latitude, store.Longitude }
                                   select new RecordStoreModel { Record = record, Store = store };

            Records = await firstRecordQuery.ToListAsync();

            return Page();
        }
    }
}
