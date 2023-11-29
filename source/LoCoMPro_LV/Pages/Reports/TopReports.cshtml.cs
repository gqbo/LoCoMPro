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
            public int TotalReports { get; set; }
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
            var topUsersReports = GetTopReportUsers();
            foreach(var UserReport in topUsersReports)
            {
                var RecordsUser = await GetRecordsCount(UserReport.NameGenerator);
                var acceptedReportsCount = await GetReportsCount(UserReport.NameGenerator);
                float PorcentageReports = 100 * acceptedReportsCount / UserReport.TotalReports;

                InfoTopReportsUser infoTopReport = new InfoTopReportsUser(RecordsUser, acceptedReportsCount, PorcentageReports, UserReport.NameGenerator, UserReport.TotalReports);
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
                                TotalReports = reader.GetInt32(reader.GetOrdinal("TotalReports")),
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
        /// Obtiene los reportes relacionadas al registro a eliminar.
        /// </summary>
        public async Task<int> GetReportsCount(string Username)
        {
            int reportsCount = await _context.Reports
                .Where(r => r.NameGenerator == Username && r.State == 2)
                .CountAsync();
            return reportsCount;
        }
    }
}
