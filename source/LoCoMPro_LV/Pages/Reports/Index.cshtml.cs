using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Utils;
using System.Data.SqlClient;

namespace LoCoMPro_LV.Pages.Reports
{
    /// <summary>
    /// Página que los registros con reportes  a los moderadores
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;

        /// <summary>
        /// Se utiliza para acceder a las utilidades de la base de datos.
        /// </summary>
        private readonly DatabaseUtils _databaseUtils;

        public IndexModel(LoComproContext context, DatabaseUtils databaseUtils)
        {
            _context = context;
            _databaseUtils = databaseUtils;
        }

        /// <summary>
        /// Lista de tipo "Record", que almacena los registros correspondientes al producto buscado.
        /// </summary>
        public IList<RecordStoreReportModel> recordStoreReports { get; set; } = default!;

        /// <summary>
        /// Metodo que realiza la busqueda de los reportes con su respectiva informacion, que se van a desplegar en la pantalla de reportes. Para ello se va a crear una estructura
        /// que alamacena un registro, la tienda asociada al mismo y una lista con los reportes relacionados al mismo.
        /// </summary>
        public async Task OnGetAsync()
        {
            var reports = await GetRecordsWithReportsAsync();

            List<RecordStoreReportModel> currentReports = reports.ToList();

            currentReports.RemoveAll(m => m.Reports.Count == 0);

            SetAverageRatings(currentReports);

            SetCountActiveReports(currentReports);

            recordStoreReports = GroupRecords(currentReports);
        }

        /// <summary>
        /// Este metodo realiza una consulta de todos los registros y luego los junta con la lista de reportes que se realizaron al registro.
        /// </summary>
        /// <returns> Devuelve una lista de "RecordStoreReportModel" con todos los registros con sus respectivos reportes.</returns>
        private async Task<List<RecordStoreReportModel>> GetRecordsWithReportsAsync()
        {
            var recordsQuery = from record in _context.Records
                               join store in _context.Stores on new { record.NameStore, record.Latitude, record.Longitude }
                               equals new { store.NameStore, store.Latitude, store.Longitude }
                               where record.Hide == false
                               select new RecordStoreReportModel
                               {
                                   Record = record,
                                   Store = store
                               };

            List<RecordStoreReportModel> currentRecords = recordsQuery.ToList();

            foreach (var recordSRM in currentRecords)
            {
                var tempReports = await GetReportsForRecordAsync(recordSRM.Record.NameGenerator, recordSRM.Record.RecordDate);

                recordSRM.Reports = new List<Report>();
                if (tempReports.Any())
                {
                    recordSRM.Reports = tempReports;
                }
            }

            return currentRecords;
        }

        /// <summary>
        /// Este metodo busca los reportes asociados a un registro
        /// </summary>
        /// <param name="nameGenerator">Es el nombre del usuario que genero el registro al cual se le busca los reportes</param>
        /// <param name="recordDate">Es la fecha en la cual se genero el registro al cual se le busca los reportes</param>
        private async Task<List<Report>> GetReportsForRecordAsync(string nameGenerator, DateTime recordDate)
        {
            var new_reports = from reports in _context.Reports
                              where reports.NameGenerator == nameGenerator &&
                                    reports.RecordDate == recordDate &&
                                    reports.State == 0
                              select reports;

            return await new_reports.ToListAsync();
        }

        /// <summary>
        /// Este metodo recibe una lista de "RecordStoreReportModel", y los agrupa a todos que son del mismo registro.
        /// </summary>
        /// <param name="currentReports">Lista de "RecordStoreReportModel" que van a ser agrupados</param>
        private List<RecordStoreReportModel> GroupRecords(List<RecordStoreReportModel> currentReports)
        {
            var groupedRecordsQuery = from record in currentReports
                                      group record by new
                                      { record.Record.NameGenerator, record.Record.RecordDate, record.Record.NameStore, record.Record.Latitude, record.Record.Longitude } into recordGroup
                                      orderby recordGroup.Key.RecordDate descending
                                      select recordGroup;

            return groupedRecordsQuery
                .Select(group => group.FirstOrDefault())
                .ToList();
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
            SqlParameter[] parameters = BuildSqlParameterArray(nameGenerator, recordDate);
            int averageRating = DatabaseUtils.ExecuteScalar<int>(connectionString, sqlQuery, parameters);
            return averageRating;
        }

        /// <summary>
        /// Método utilizado para definir el promedio de las valoraciones de estrellas de un registro en específico utilizando
        /// una función escalar creada en la base de datos.
        /// <param name="currentReports">Lista de registros de un producto utilizada para agregarle los promedios en estrellas. </param>
        /// </summary>
        private void SetAverageRatings(List<RecordStoreReportModel> currentReports)
        {
            List<int> averageRatings = new List<int>();

            foreach (var recordStoreModel in currentReports)
            {
                int averageRating = GetAverageRating(recordStoreModel.Record.NameGenerator, recordStoreModel.Record.RecordDate);
                averageRatings.Add(averageRating);
                recordStoreModel.recordValoration = averageRatings.Last();
            }
        }

        /// <summary>
        /// Método utilizado para obtener la cantidad de reportes que posee un registro en específico utilizando
        /// una función escalar creada en la base de datos.
        /// <param name="nameGenerator">Nombre del generador de un registro utilizado para utilizarlo como parámetro en la función escalar</param>
        /// <param name="recordDate">Fecha de un registro utilizado para utilizarlo como parámetro en la función escalar</param>
        /// </summary>
        private int GetCountActiveReports(string nameGenerator, DateTime recordDate)
        {
            string connectionString = _databaseUtils.GetConnectionString();
            string sqlQuery = "SELECT dbo.GetCountActiveReports(@NameGenerator, @RecordDate)";
            SqlParameter[] parameters = BuildSqlParameterArray(nameGenerator, recordDate);
            int countReports = DatabaseUtils.ExecuteScalar<int>(connectionString, sqlQuery, parameters);
            return countReports;
        }

        /// <summary>
        /// Método utilizado para definir la cantidad de reportes en específico utilizando
        /// una función escalar creada en la base de datos.
        /// <param name="currentReports">Lista de registros de un producto utilizada para agregarle los promedios en estrellas. </param>
        /// </summary>
        private void SetCountActiveReports(List<RecordStoreReportModel> currentReports)
        {
            foreach (var recordStoreModel in currentReports)
            {
                int countReports = GetCountActiveReports(recordStoreModel.Record.NameGenerator, recordStoreModel.Record.RecordDate);
                recordStoreModel.recordValoration = countReports;
            }
        }

        /// <summary>
        /// Método utilizado para construir una matriz de objetos SqlParameter que se utilizan en consultas SQL. 
        /// <param name="nameGenerator">Nombre del generador de un registro utilizado como parámetro en la consulta SQL.</param>
        /// <param name="recordDate">Fecha de un registro utilizada como parámetro en la consulta SQL.</param>
        /// </summary>
        private SqlParameter[] BuildSqlParameterArray(string nameGenerator, DateTime recordDate)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NameGenerator", nameGenerator),
                new SqlParameter("@RecordDate", recordDate)
            };
            return parameters;
        }
    }
}


