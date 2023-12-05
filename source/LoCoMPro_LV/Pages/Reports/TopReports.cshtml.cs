using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Utils;
using System.Data.SqlClient;
using System.Data;

namespace LoCoMPro_LV.Pages.Reports
{
    public class TopReportsModel : PageModel
    {

        /// <summary>
        /// Estructura para almacenar el nombre de usuario y la cantidad de reportes relacionados.
        /// </summary>
        public class TopReportModel
        {
            public string NameGenerator { get; set; }
            public int ReportsReceived { get; set; }
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

        public TopReportsModel(LoComproContext context, DatabaseUtils databaseUtils)
        {
            _context = context;
            _databaseUtils = databaseUtils;
        }

        /// <summary>
        /// Lista que almacena información relevante a los reportes.
        /// </summary>
        public IList<InfoTopReportsUser> InfoTopReports { get; set; } = default!;


        /// <summary>
        /// Obtiene la información necesaria para determinar los usuarios más reportados.
        /// </summary>
        public async Task OnGetAsync()
        {
            InfoTopReports = new List<InfoTopReportsUser>();
            var TopUsersReports = GetTopReportUsers();
            foreach(var UserReport in TopUsersReports)
            {
                var RecordsCount = await GetRecordsCount(UserReport.NameGenerator);
                var AcceptedReportsCount = await GetAcceptedReportsCount(UserReport.NameGenerator);
                float AcceptedReportsPercentage = 100 * AcceptedReportsCount / UserReport.ReportsReceived;
                var UserRating = GetUserRating(UserReport.NameGenerator);
                InfoTopReportsUser infoTopReport = new InfoTopReportsUser(RecordsCount, UserReport.ReportsReceived, 0, AcceptedReportsCount, AcceptedReportsPercentage, UserReport.NameGenerator, UserRating);
                InfoTopReports.Add(infoTopReport);
            }
        }

        /// <summary>
        /// Llama a una función de la base de datos para obtener los datos de los usuarios más reportados.
        /// </summary>
        private IEnumerable<TopReportModel> GetTopReportUsers()
        {
            string connectionString = _databaseUtils.GetConnectionString();
            string sqlQuery = "SELECT * FROM dbo.GetTopReports()";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new TopReportModel
                            {
                                NameGenerator = reader.GetString(reader.GetOrdinal("NameGenerator")),
                                ReportsReceived = reader.GetInt32(reader.GetOrdinal("TotalReports")),
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
        /// Obtiene los reportes de un usuario que han sido aceptados.
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
