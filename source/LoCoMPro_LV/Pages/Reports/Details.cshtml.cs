﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Pages.Records;
using LoCoMPro_LV.Utils;
using System.Data.SqlClient;

namespace LoCoMPro_LV.Pages.Reports
{
    public class DetailsModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

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
        /// Lista de tipo "RecordStoreReportModel", que almacena los registros correspondientes al producto que se selecciono para ver en detalle, con su respectiva tienda.
        /// </summary>
        public RecordStoreReportModel recordStoreReports { get; set; } = default!;

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

            RecordStoreReportModel temp = new RecordStoreReportModel
            {
                Record = query.Record,
                Store = query.Store,

                Reports = await GetReportsForRecordAsync(NameGenerator, RecordDate),

                recordValoration = GetAverageRating(NameGenerator, RecordDate),

                generatorValoration = GetUserRating(NameGenerator)
            };

            temp.reporterValorations = new List<int>();

            foreach (Report report in temp.Reports)
            {
                int rating = GetUserRating(report.NameReporter);
                temp.reporterValorations.Add(rating);
            }

            recordStoreReports = temp;

            return Page();
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
                                    reports.RecordDate == recordDate
                              select reports;

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
        /// Este metodo utiliza la informacion recopilada en la vista para crear el reporte y enviarlo a la base de datos.
        /// </summary>
        public async Task<IActionResult> OnPostAsync(string action, int customNumber)
        {
            if (action == "accept")
            {
                
            }
            else if (action == "reject")
            {
                
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("../Index");
        }
    }
}
