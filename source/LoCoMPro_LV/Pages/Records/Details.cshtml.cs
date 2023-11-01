using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using System.Data.SqlClient;
using System.Data;
using LoCoMPro_LV.Utils;

namespace LoCoMPro_LV.Pages.Records
{
    public class RecordStoreModel
    {
        public Record Record { get; set; }
        public Store Store { get; set; }
        public int AverageRating { get; set; }
    }

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
        /// Se utiliza para acceder a las utilidades de la base de datos.
        /// </summary>
        private readonly DatabaseUtils _databaseUtils;

        public DetailsModel(LoComproContext context, DatabaseUtils databaseUtils)
        {
            _context = context;
            _databaseUtils = databaseUtils;
        }

        /// <summary>
        /// Lista de tipo "RecordStoreModel", que almacena los registros correspondientes al producto que se selecciono para ver en detalle, con su respectiva tienda.
        /// </summary>
        public IList<RecordStoreModel> Records { get; set; } = default!;

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

            var FirstRecord = await _context.Records
                .FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator && m.RecordDate == RecordDate);

            if (FirstRecord != null)
            {
                var allRecords = GetCombinedRecordsAndStores(FirstRecord).ToList();
                List<RecordStoreModel> updatedRecords = allRecords.ToList();
                SetAverageRatings(updatedRecords);
                Records = updatedRecords;
            }
            else
            {
                Records = new List<RecordStoreModel>();
            }

            return Page();
        }

        /// <summary>
        /// Método utilizado para el manejo de la solicitud POST que se realiza a la hora de valorar con estrellas
        /// un registro en específico donde se encarga de actualizar la base de datos con la nueva valoración.
        /// </summary>
        public async Task<IActionResult> OnPostSubmitRating(string nameGenerator, DateTime recordDate, int rating)
        {
            if (string.IsNullOrEmpty(nameGenerator) || recordDate == DateTime.MinValue || rating < 1 || rating > 5)
            {
                return BadRequest();
            }

            try
            {
                var evaluate = new Evaluate
                {
                    NameGenerator = nameGenerator,
                    RecordDate = recordDate,
                    StarsCount = rating,
                    NameEvaluator = User.Identity.Name
                };

                await AddEvaluationAsync(evaluate);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Método utilizado para combinar registros de las tablas Records y Stores, creando así un nuevo tipo RecordStoreModel
        /// </summary>
        private IQueryable<RecordStoreModel> GetCombinedRecordsAndStores(Record firstRecord)
        {
            var query = from record in _context.Records
                        join store in _context.Stores
                        on new { record.NameStore, record.Latitude, record.Longitude }
                        equals new { store.NameStore, store.Latitude, store.Longitude }
                        where record.NameProduct.Contains(firstRecord.NameProduct) &&
                              record.Latitude == firstRecord.Latitude &&
                              record.Longitude == firstRecord.Longitude
                        orderby record.RecordDate descending
                        select new RecordStoreModel
                        {
                            Record = record,
                            Store = store,
                        };
            return query;
        }

        /// <summary>
        /// Método utilizado para verificar si una valoración ya existe en la base de datos, con el objetivo de evitar crear una nueva tupla, 
        /// solo modificar la valoración de estrellas.
        /// </summary>
        private async Task<Evaluate> CheckExistingEvaluationAsync(Evaluate evaluate)
        {
            return await _context.Valorations
                .FirstOrDefaultAsync(e => e.NameEvaluator == User.Identity.Name
                && e.NameGenerator == evaluate.NameGenerator
                && e.RecordDate == evaluate.RecordDate);
        }

        /// <summary>
        /// Método utilizado para modificar o agregar una nueva valoración de un usuario sobre un registro. 
        /// </summary>
        private async Task AddEvaluationAsync(Evaluate evaluate)
        {
            var existingEvaluation = await CheckExistingEvaluationAsync(evaluate);
            if (existingEvaluation != null)
            {
                existingEvaluation.StarsCount = evaluate.StarsCount;
            }
            else
            {
                _context.Valorations.Add(evaluate);
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Método utilizado para obtener el promedio de las valoraciones de estrellas de un registro en específico utilizando
        /// una función escalar creada en la base de datos.
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
        /// Método utilizado para definir el promedio de las valoraciones de estrellas de un registro en específico utilizando
        /// una función escalar creada en la base de datos.
        /// </summary>
        private void SetAverageRatings(List<RecordStoreModel> updatedRecords)
        {
            List<int> averageRatings = new List<int>();

            foreach (var recordStoreModel in updatedRecords)
            {
                int averageRating = GetAverageRating(recordStoreModel.Record.NameGenerator, recordStoreModel.Record.RecordDate);
                averageRatings.Add(averageRating);
                recordStoreModel.AverageRating = averageRatings.Last();
            }
        }
    }
}
