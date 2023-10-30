using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Pages.Records;
using System.Globalization;


namespace LoCoMPro_LV.Pages.Reports
{
    /// <summary>
    /// Página Create de Records para la creación de nuevos los reportes relacionados a los registros.
    /// </summary>
    public class CreateModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;

        public CreateModel(LoComproContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista de tipo "RecordStoreModel", que almacena los registros correspondientes al producto que se selecciono para ver en detalle, con su respectiva tienda.
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
        /// Es un objeto de tipo Report, para guardar los datos y enviar el objeto a la base de datos.
        /// </summary>
        [BindProperty]
        public Report Report { get; set; }

        /// <summary>
        /// El metodo setea los datos del objeto RecordStoreModel, con la informacion del Record recopilada de la pantalla de detalles.
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

        /// <summary>
        /// Este metodo utiliza la informacion recopilada en la vista para crear el reporte y enviarlo a la base de datos.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            Report = new Report
            {
                NameReporter = User.Identity.Name,
                NameGenerator = NameGenerator,
                RecordDate = RecordDate,
                ReportDate = GetCurrentDateTime(),
                Comment = Report.Comment
            };

            _context.Reports.Add(Report);
            await _context.SaveChangesAsync();
            return RedirectToPage("../Index");
        }

        /// <summary>
        /// Método que verifica la hora actual para almacenarla en la BD.
        /// </summary>
        private static DateTime GetCurrentDateTime()
        {
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return DateTime.ParseExact(currentDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
