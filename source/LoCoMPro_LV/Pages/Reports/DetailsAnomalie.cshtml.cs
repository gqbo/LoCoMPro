using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Utils;
using System.Data.SqlClient;

namespace LoCoMPro_LV.Pages.Reports
{
    public class DetailsAnomalieModel : PageModel
    {
        private readonly LoComproContext _context;

        /// <summary>
        /// Se utiliza para acceder a las utilidades de la base de datos.
        /// </summary>
        private readonly DatabaseUtils _databaseUtils;

        public DetailsAnomalieModel(LoComproContext context, DatabaseUtils databaseUtils)
        {
            _context = context;
            _databaseUtils = databaseUtils;
        }

        /// <summary>
        /// Lista de tipo "RecordStoreReportModel", que almacena los registros correspondientes al producto que se selecciono para ver en detalle, con su respectiva tienda.
        /// </summary>
        public RecordStoreAnomaliesModel recordStoreAnomalies { get; set; } = default!;

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
        /// de todos los registros con ese producto en esa tienda en específico. Además se saca el promedio de estrellas para cada registro asociado
        /// a un producto.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            var query = await (from r in _context.Records
                               join s in _context.Stores on new { r.Latitude, r.Longitude } equals new { s.Latitude, s.Longitude }
                               where r.NameGenerator == NameGenerator && r.RecordDate == RecordDate
                               select new
                               {
                                   Record = r,
                                   Store = s
                               })
                 .FirstOrDefaultAsync();

            RecordStoreAnomaliesModel temp = new RecordStoreAnomaliesModel
            {
                Record = query.Record,
                Store = query.Store,

                Anomalies = await GetAnomaliesForRecordAsync(NameGenerator, RecordDate),

                recordValoration = GetAverageRating(NameGenerator, RecordDate),

                generatorValoration = GetUserRating(NameGenerator)
            };

            temp.reporterValorations = new List<int>();

            recordStoreAnomalies = temp;

            return Page();
        }

        /// <summary>
        /// Este metodo busca los reportes asociados a un registro
        /// </summary>
        /// <param name="nameGenerator">Es el nombre del usuario que genero el registro al cual se le busca los reportes</param>
        /// <param name="recordDate">Es la fecha en la cual se genero el registro al cual se le busca los reportes</param>
        public async Task<List<Anomalie>> GetAnomaliesForRecordAsync(string nameGenerator, DateTime recordDate)
        {
            var new_reports = from Anomalie in _context.Anomalies
                              where Anomalie.NameGenerator == nameGenerator &&
                                    Anomalie.RecordDate == recordDate &&
                                    Anomalie.State == 0
                              select Anomalie;

            return await new_reports.ToListAsync();
        }

        /// <summary>
        /// Método utilizado para obtener el promedio de las valoraciones de estrellas de un registro en específico utilizando
        /// una función escalar creada en la base de datos.
        /// <param name="nameGenerator">Nombre del generador de un registro utilizado para utilizarlo como parámetro en la función escalar</param>
        /// <param name="recordDate">Fecha de un registro utilizado para utilizarlo como parámetro en la función escalar</param>
        /// </summary>
        private int GetAverageRating(string nameGenerator, DateTime recordDate)
        {
            string connectionString = _databaseUtils.GetConnectionString();
            string sqlQuery = "SELECT dbo.GetStarsAverage(@NameGenerator, @RecordDate)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NameGenerator", nameGenerator),
                new SqlParameter("@RecordDate", recordDate)
            };
            int averageRating = DatabaseUtils.ExecuteScalar<int>(connectionString, sqlQuery, parameters);
            return averageRating;
        }

        /// <summary>
        /// Este metodo se utiliza para sacar la valoracion media de un usuario con base a las valoraciones de los registros del usuario.
        /// </summary>
        /// <param name="nameGenerator">Nombre de usuario del usuario del que se requiere la valoracion.</param>
        /// <returns>Devuelve la valoracion del usuario deseado</returns>
        private int GetUserRating(string nameGenerator)
        {
            string connectionString = _databaseUtils.GetConnectionString();
            string sqlQuery = "SELECT dbo.GetUserRating(@NameGenerator)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NameGenerator", nameGenerator)
            };
            int userRating = DatabaseUtils.ExecuteScalar<int>(connectionString, sqlQuery, parameters);
            return userRating;
        }

        /// <summary>
        /// Este metodo recibe los datos obtenidos de la vista para poder acepar o rechazar los reportes que se estan manejando.
        /// </summary>
        /// <param name="action">Es el dato que especifica si el dato fue aceptado o rechazado</param>
        /// <param name="nameReporter">El nombre del usuario que realizo el reporte</param>
        /// <param name="reportDate">La fecha en la que se realizo el reporte</param>
        public async Task<IActionResult> OnPostAsync(string action, DateTime recordDate, string type)
        {
            var entities = await _context.Anomalies
                .Where(e => e.NameGenerator == NameGenerator && e.RecordDate == recordDate )
                .ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                return NotFound();
            }
           
            foreach (var entity in entities)
            {
                if (action == "accept")
                {
                    entity.State = 1;
                }
                else if (action == "reject")
                {
                    if (entity.Type == type)
                    {
                        entity.State = 2;
                    }                    
                }
            }

            await _context.SaveChangesAsync();
            await UpdateAnomaliesAndHide(entities);
            return RedirectToPage("./Anomalies");
        }

        private async Task UpdateAnomaliesAndHide(List<Anomalie> entities)
        {
            var recordsToUpdate = await _context.Records
                .Where(r => r.NameGenerator == NameGenerator && r.RecordDate == RecordDate)
                .ToListAsync();

            if (recordsToUpdate != null && recordsToUpdate.Count > 0)
            {
                foreach (var record in recordsToUpdate)
                {
                    if (entities.Any(e => e.State == 1))
                    {
                        record.Hide = true;
                    }
                    else
                    {
                        record.Hide = false;
                    }
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
