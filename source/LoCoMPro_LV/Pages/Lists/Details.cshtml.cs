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
    public class StoreWithProductsModel
    {
        public string NameStore { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int ProductCount { get; set; }
    }

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
            var count = GetListedCount(User.Identity.Name);

            var storesWithProducts = GetStoresWithProducts();

            foreach (var storeWithProducts in storesWithProducts)
            {
                var result = await CreateListSearchResult(storeWithProducts, count);
                Results.Add(result);
            }

            Results = Results.OrderByDescending(r => r.productCount).ToList();

            return Page();
        }

        private int GetListedCount( string UserName)
        {
            return _context.Listed
                .Where(listed => listed.NameList == UserName)
                .Count();
        }

        private IEnumerable<StoreWithProductsModel> GetStoresWithProducts()
        {
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
                            yield return new StoreWithProductsModel
                            {
                                NameStore = reader.GetString(reader.GetOrdinal("NameStore")),
                                Longitude = reader.GetDouble(reader.GetOrdinal("Longitude")),
                                Latitude = reader.GetDouble(reader.GetOrdinal("Latitude")),
                                ProductCount = reader.GetInt32(reader.GetOrdinal("cantidad_productos"))
                            };
                        }
                    }
                }
            }
        }

        private async Task<ListSearchResults> CreateListSearchResult(StoreWithProductsModel storeWithProducts, int count)
        {
            ListSearchResults result = new ListSearchResults
            {
                productCount = storeWithProducts.ProductCount,
                percentageInList = 100 * storeWithProducts.ProductCount / count,
                Store = await GetStoreAsync(storeWithProducts),
                Distance = await CalculateDistanceAsync(storeWithProducts),
                Records = await GetRecordsAsync(storeWithProducts)
            };

            result.totalPrice = result.Records.Sum(record => record.Price);

            return result;
        }

        private async Task<Store> GetStoreAsync(StoreWithProductsModel storeWithProducts)
        {
            return await _context.Stores
                .FirstOrDefaultAsync(m => m.Latitude == storeWithProducts.Latitude &&
                                            m.Longitude == storeWithProducts.Longitude &&
                                            m.NameStore == storeWithProducts.NameStore);
        }

        private async Task<double> CalculateDistanceAsync(StoreWithProductsModel storeWithProducts)
        {
            var userLocation = User.Identity.IsAuthenticated ? await GetLocationUserAsync() : new double[] { 0, 0 };

            return (userLocation[0] != 0 && userLocation[1] != 0) ?
                   Geolocation.CalculateDistance(userLocation[0], userLocation[1], storeWithProducts.Latitude, storeWithProducts.Longitude) / 1000 : 0;
        }

        private async Task<List<Record>> GetRecordsAsync(StoreWithProductsModel storeWithProducts)
        {
            var query = from record in _context.Records
                        join listed in _context.Listed
                        on record.NameProduct equals listed.NameProduct
                        where record.NameStore == storeWithProducts.NameStore
                              && record.Latitude == storeWithProducts.Latitude
                              && record.Longitude == storeWithProducts.Longitude
                        orderby record.RecordDate descending
                        group record by record.NameProduct into grouped
                        select grouped.First();

            return await query.ToListAsync();
        }


        /// <summary>
        /// Obtiene las coordenadas del usuario que está realizando la consulta.
        /// </summary>
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
