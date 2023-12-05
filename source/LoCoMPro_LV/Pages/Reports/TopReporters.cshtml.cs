using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Utils;
using System.Data.SqlClient;
using System.Data;

namespace LoCoMPro_LV.Pages.Reports
{
    public class TopReportersModel : PageModel
    {

        /// <summary>
        /// Estructura para almacenar el nombre de usuario y la cantidad de reportes relacionados.
        /// </summary>
        public class TopReporterModel
        {
            public string NameReporter { get; set; }
            public int ReportsMade { get; set; }
            public int UserRating { get; set; }
        }
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;

        /// <summary>
        /// Se utiliza para acceder a las utilidades de la base de datos.
        /// </summary>
        private readonly DatabaseUtils _databaseUtils;

        public TopReportersModel(LoComproContext context, DatabaseUtils databaseUtils)
        {
            _context = context;
            _databaseUtils = databaseUtils;
        }

        /// <summary>
        /// Lista que almacena información relevante a los reportes.
        /// </summary>
        public IList<InfoTopReportsUser> InfoTopReports { get; set; } = default!;


        /// <summary>
        /// Obtiene la información necesaria para determinar los usuarios que más reportan.
        /// </summary>
        public async Task OnGetAsync()
        {
            InfoTopReports = new List<InfoTopReportsUser>();
            var TopUsersReports = GetTopReportersUsers();
            foreach(var UserReport in TopUsersReports)
            {
                var RecordsCount = await GetRecordsCount(UserReport.NameReporter);
                var AcceptedReportsCount = await GetAcceptedReportsCount(UserReport.NameReporter);
                float AcceptedReportsPercentage = 100 * AcceptedReportsCount / UserReport.ReportsMade;
                var UserRating = GetUserRating(UserReport.NameReporter);
                InfoTopReportsUser infoTopReport = new InfoTopReportsUser(RecordsCount, 0, UserReport.ReportsMade, AcceptedReportsCount, AcceptedReportsPercentage, UserReport.NameReporter, UserRating);
                InfoTopReports.Add(infoTopReport);
            }
        }

        /// <summary>
        /// Llama a una función de la base de datos para obtener los datos de los usuarios que más reportan.
        /// </summary>
        private IEnumerable<TopReporterModel> GetTopReportersUsers()
        {
            string connectionString = _databaseUtils.GetConnectionString();
            string sqlQuery = "SELECT * FROM dbo.GetTopReporters()";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new TopReporterModel
                            {
                                NameReporter = reader.GetString(reader.GetOrdinal("NameReporter")),
                                ReportsMade = reader.GetInt32(reader.GetOrdinal("TotalReports")),
                            };
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene la cantidad de registros de un usuario.
        /// </summary>
        private async Task<int> GetRecordsCount(string Username)
        {
            int recordsCount = await _context.Records
                .Where(r => r.NameGenerator == Username)
                .CountAsync();

            return recordsCount;
        }

        /// <summary>
        /// Obtiene los reportes que ha realizado un usuario.
        /// </summary>
        public async Task<int> GetReportsCount(string Username)
        {
            int reportsCount = await _context.Reports
                .Where(r => r.NameGenerator == Username)
                .CountAsync();
            return reportsCount;
        }


        /// <summary>
        /// Obtiene los reportes de un usuario que han sido aceptados.
        /// </summary>
        public async Task<int> GetAcceptedReportsCount(string Username)
        {
            int acceptedReportsCount = await _context.Reports
                .Where(r => r.NameGenerator == Username && r.State == 1)
                .CountAsync();
            return acceptedReportsCount;
        }

        /// <summary>
        /// Este metodo se eutiliza para sacar la valoracion media de un usuario con base a las valoraciones de los registros del usuario.
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
    }
}
