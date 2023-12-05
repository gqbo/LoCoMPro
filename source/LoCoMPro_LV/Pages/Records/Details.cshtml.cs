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
        /// Nombre del producto al cual se le presentan los registros.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameProduct { get; set; }

        /// <summary>
        /// Dice si el producto esta en la lista del usuario o si no esta.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int InList { get; set; }

        /// <summary>
        /// Método utilizado cuando en la pantalla de resultados de la búsqueda se selecciona un producto para abrir el detalle. Este método
        /// utiliza el nombre del generador y la fecha de realización del registro más reciente del producto para realizar la consulta por todos
        /// de todos los registros con ese producto en esa tienda en específico. Además se saca el promedio de estrellas para cada registro asociado
        /// a un producto.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {

            var FirstRecord = await _context.Records
                .FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator && m.RecordDate == RecordDate);

            if (FirstRecord != null)
            {
                var allRecords = GetCombinedRecordsAndStores(FirstRecord).ToList();
                List<RecordStoreModel> currentRecords = allRecords.Where(record => !record.Record.Hide).ToList();
                SetAverageAndCountRatings(currentRecords);
                Records = currentRecords;
            }
            else
            {
                Records = new List<RecordStoreModel>();
            }

            var listed = await _context.Listed
                .FirstOrDefaultAsync(m => m.NameProduct == Records.First().Record.NameProduct && m.NameList == User.Identity.Name);

            if (listed != null)
            {
                InList = 1;
            } 
            else
            {
                InList = 0;
            }

            return Page();
        }

        /// <summary>
        /// Método utilizado para el manejo de la solicitud POST que se realiza a la hora de valorar con estrellas
        /// un registro en específico donde se encarga de actualizar la base de datos con la nueva valoración.
        /// <param name="nameGenerator">Contiene el nombre del generador de un registro cuando se crea una solicitud POST a la hora de valorar un registro</param>
        /// <param name="recordDate">Contiene la fecha de un registro cuando se crea una solicitud POST a la hora de valorar un registro</param>
        /// <param name="rating">Contiene la valoración que se le da a un registro.</param>
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
                int newCountRating = GetCountRating(nameGenerator, recordDate);
                return new OkObjectResult(new { countRating = newCountRating });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Método utilizado para combinar registros de las tablas Records y Stores, creando así un nuevo tipo RecordStoreModel
        /// <param name="firstRecord">Información del registro más reciente del producto del cual se quieren ver los detalles</param>
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
        /// <param name="valoration">Valoración utilizada para verificar si ya existe en la base de datos.</param>
        /// </summary>
        private async Task<Evaluate> CheckExistingEvaluationAsync(Evaluate valoration)
        {
            return await _context.Valorations
                .FirstOrDefaultAsync(e => e.NameEvaluator == User.Identity.Name
                && e.NameGenerator == valoration.NameGenerator
                && e.RecordDate == valoration.RecordDate);
        }

        /// <summary>
        /// Método utilizado para modificar o agregar una nueva valoración de un usuario sobre un registro. 
        /// <param name="valoration">Valoración utilizada para crear una nueva valoración en la base de datos.</param>
        /// </summary>
        private async Task AddEvaluationAsync(Evaluate valoration)
        {
            var existingEvaluation = await CheckExistingEvaluationAsync(valoration);
            if (existingEvaluation != null)
            {
                existingEvaluation.StarsCount = valoration.StarsCount;
            }
            else
            {
                _context.Valorations.Add(valoration);
            }
            await _context.SaveChangesAsync();

            var user = await _context.ModeratorUsers
                .FirstOrDefaultAsync(m => m.UserName == valoration.NameGenerator);

            if (user == null)
                CallSetModeratorProcedure(valoration.NameGenerator);
        }

        /// <summary>
        /// Llama al procedimiento almacenado para designar a un usuario como moderador.
        /// </summary>
        /// <param name="username">El nombre de usuario del usuario que se asignara como moderador.</param>
        private void CallSetModeratorProcedure(string username)
        {

            string connectionString = _databaseUtils.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SetModerator", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NameGenerator", username);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
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
        /// Método utilizado para obtener la cantidad de valoraciones de un registro en específico utilizando
        /// una función escalar creada en la base de datos.
        /// <param name="nameGenerator">Nombre del generador de un registro utilizado para utilizarlo como parámetro en la función escalar</param>
        /// <param name="recordDate">Fecha de un registro utilizado para utilizarlo como parámetro en la función escalar</param>
        /// </summary>
        private int GetCountRating(string nameGenerator, DateTime recordDate)
        {
            string connectionString = _databaseUtils.GetConnectionString();
            string sqlQuery = "SELECT dbo.GetCountRating(@NameGenerator, @RecordDate)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NameGenerator", nameGenerator),
                new SqlParameter("@RecordDate", recordDate)
            };
            int countRating = DatabaseUtils.ExecuteScalar<int>(connectionString, sqlQuery, parameters);
            return countRating;
        }

        /// <summary>
        /// Método utilizado para definir el promedio de las valoraciones de estrellas de un registro en específico utilizando
        /// una función escalar creada en la base de datos.
        /// <param name="currentRecords">Lista de registros de un producto utilizada para agregarle los promedios en estrellas. </param>
        /// </summary>
        private void SetAverageAndCountRatings(List<RecordStoreModel> currentRecords)
        {
            List<int> averageRatings = new();
            List<int> countRatings = new();


            foreach (var recordStoreModel in currentRecords)
            {
                int averageRating = GetAverageRating(recordStoreModel.Record.NameGenerator, recordStoreModel.Record.RecordDate);
                int countRating = GetCountRating(recordStoreModel.Record.NameGenerator, recordStoreModel.Record.RecordDate);
                averageRatings.Add(averageRating);
                countRatings.Add(countRating);
                recordStoreModel.AverageRating = averageRatings.Last();
                recordStoreModel.CountRating = countRatings.Last();
            }
        }

        /// <summary>
        /// Este metodo se utiliza al presionar el boton de agaregar a lista o de eliminar de lista y dependiendo la opcion seleccionada va a agregar el 
        /// producto a la lista del usuario o la va eliminar de la lista.
        /// </summary>
        public async Task<IActionResult> OnPostManageListAsync()
        {
            if (InList == 3)
            {
                await AddToListAsync(User.Identity.Name);
            }
            else if (InList == 4)
            {
                await RemoveFromListAsync(User.Identity.Name);
            }

            await _context.SaveChangesAsync();

            string formattedDate = RecordDate.ToString("yyyy-MM-dd HH:mm:ss");
            string url = $"/Records/Details?NameGenerator={NameGenerator}&RecordDate={formattedDate}";
            return Redirect(url);
        }

        /// <summary>
        /// Este metodo se encarga de realizar el agregado del producto que tiene NameProduct de la pagina de detalles a la lista que recibe por linea de parametros.
        /// </summary>
        /// <param name="userName">Es el nombre de la lista al que se le va a agregar el item.</param>
        public async Task AddToListAsync(string userName)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.NameProduct == NameProduct);

            if (product != null)
            {
                Listed item = new Listed
                {
                    NameList = userName,
                    NameProduct = NameProduct,
                    UserName = userName,
                };

                _context.Listed.Add(item);
            }
        }


        /// <summary>
        /// Este metodo se encraga de eliminar el item de la lista que recibe por linea de parametros cuyo producto es el que tiene el NameProduct de la pagina de detalle.
        /// </summary>
        /// <param name="userName">Es el nombre de la lista al que se le va a eliminar el item.</param>
        public async Task RemoveFromListAsync(string userName)
        {
            var listed = await _context.Listed
                .FirstOrDefaultAsync(m => m.NameProduct == NameProduct && m.NameList == userName);

            if (listed != null)
            {
                _context.Listed.Remove(listed);
            }
        }

    }
}
