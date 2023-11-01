using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Data;

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
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        /// <summary>
        /// Se utiliza para acceder a los valores de configuración.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor de la clase DetailsModel.
        /// </summary>
        /// <param name="context">Contexto de la base de datos de LoCoMPro.</param>
        public DetailsModel(LoCoMPro_LV.Data.LoComproContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            Console.WriteLine(NameGenerator);

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
                                     Store = store,
                                 };

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
            int averageRating = 0;
            string connectionString = _configuration.GetConnectionString("LoCoMProContextRemote");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using var command = new SqlCommand("SELECT dbo.GetStarsAverage(@NameGenerator, @RecordDate)", connection);
                command.Parameters.Add(new SqlParameter("@NameGenerator", nameGenerator));
                command.Parameters.Add(new SqlParameter("@RecordDate", recordDate));
                connection.Open();

                var resultOfFunction = command.ExecuteScalar();
                if (resultOfFunction != DBNull.Value && resultOfFunction != null)
                {
                    averageRating = Convert.ToInt32(resultOfFunction);
                }
            }
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
