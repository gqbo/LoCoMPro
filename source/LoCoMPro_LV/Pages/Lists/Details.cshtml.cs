using System;
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
using Microsoft.AspNetCore.Identity;

namespace LoCoMPro_LV.Pages.Lists
{
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
        /// <summary>
        /// Administra a los usuarios de tipo ApplicationUser.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(LoComproContext context, DatabaseUtils databaseUtils, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _databaseUtils = databaseUtils;
            _userManager = userManager;
        }

        public List List { get; set; }

        /// <summary>
        /// Es el nombre de la lista del ususrio que se esta presentando
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameList { get; set; }

        public IList<ListSearchResults> Results { get; set; } = new List<ListSearchResults>();


        public async Task<IActionResult> OnGetAsync()
        {
            var count = _context.Listed
                .Where(listed => listed.NameList == User.Identity.Name)
                .Count();

            string connectionString = _databaseUtils.GetConnectionString();
            string sqlQuery = "SELECT * FROM dbo.GetStoresWithProducts(@NameList)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NameList", User.Identity.Name)
            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddRange(parameters);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nameStore = reader.GetString(reader.GetOrdinal("NameStore"));
                            double longitude = reader.GetDouble(reader.GetOrdinal("Longitude"));
                            double latitude = reader.GetDouble(reader.GetOrdinal("Latitude"));
                            int productCount = reader.GetInt32(reader.GetOrdinal("cantidad_productos"));

                            ListSearchResults result = new ListSearchResults();

                            result.productCount = productCount;

                            result.percentageInList = 100 * productCount / count;

                            result.Store = await _context.Stores
                                .FirstOrDefaultAsync(m => m.Latitude == latitude && m.Longitude == longitude && m.NameStore == nameStore);

                            var userLocation = new double[] { 0, 0 };
                            if (User.Identity.IsAuthenticated)
                            {
                                userLocation = await GetLocationUserAsync();
                            }

                            result.Distance = (userLocation[0] != 0 && userLocation[1] != 0) ?
                                              Geolocation.CalculateDistance(userLocation[0], userLocation[1], latitude, longitude) / 1000 : 0;

                            var query = from record in _context.Records
                                        join listed in _context.Listed
                                        on record.NameProduct equals listed.NameProduct
                                        where record.NameStore == nameStore
                                        && record.Latitude == latitude
                                        && record.Longitude == longitude
                                        orderby record.RecordDate descending
                                        group record by record.NameProduct into grouped
                                        select grouped.First();

                            result.Records = await query.ToListAsync();

                            foreach(var record in result.Records)
                            {
                                result.totalPrice += record.Price;
                            }

                            Results.Add(result);
                        }
                    }
                }
            }

            Results = Results.OrderByDescending(r => r.productCount).ToList();

            return Page();
        }

        /// <summary>
        /// Obtiene las coordenadas del usuario que está realizando la consulta.
        /// </summary>
        /// <returns></returns>
        private async Task<double[]> GetLocationUserAsync()
        {
            var authenticatedUserName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(authenticatedUserName);

            if (user != null)
            {
                var latitude = user.Latitude;
                var longitude = user.Longitude;
                var location = new double[] { latitude, longitude };
                return location;
            }
            else
            {
                var latitude = 0;
                var longitude = 0;
                var location = new double[] { latitude, longitude };
                return location;
            }
        }
    }
}
