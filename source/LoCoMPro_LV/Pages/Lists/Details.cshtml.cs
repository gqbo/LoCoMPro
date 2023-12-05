using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
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

        /// <summary>
        /// Maneja las solicitudes GET para la página actual.
        /// Obtiene y muestra resultados de búsqueda para cada tienda con productos del usuario actual.
        /// </summary>
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

        /// <summary>
        /// Obtiene la cantidad de listas creadas por un usuario específico.
        /// </summary>
        /// <param name="UserName">Nombre de usuario para el cual se desea obtener la cantidad de listas.</param>
        public int GetListedCount( string UserName)
        {
            return _context.Listed
                .Where(listed => listed.NameList == UserName)
                .Count();
        }

        /// <summary>
        /// Obtiene las tiendas con la cantidad de productos asociados a un usuario específico
        /// utilizando una función de la base de datos.
        /// </summary>
        public IEnumerable<StoreWithProductsModel> GetStoresWithProducts()
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


        /// <summary>
        /// Crea un resultado de búsqueda de lista a partir de un modelo de tienda con productos.
        /// </summary>
        /// <param name="storeWithProducts">Modelo de tienda con productos.</param>
        /// <param name="count">Cantidad de productos.</param>
        public async Task<ListSearchResults> CreateListSearchResult(StoreWithProductsModel storeWithProducts, int count)
        {
            if (count == 0)
            {
                return null;
            } 
            else
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
        }

        /// <summary>
        /// Obtiene una tienda según la información proporcionada en el modelo de tienda con productos.
        /// </summary>
        /// <param name="storeWithProducts">Modelo de tienda con productos.</param>
        public async Task<Store> GetStoreAsync(StoreWithProductsModel storeWithProducts)
        {
            return await _context.Stores
                .FirstOrDefaultAsync(m => m.Latitude == storeWithProducts.Latitude &&
                                            m.Longitude == storeWithProducts.Longitude &&
                                            m.NameStore == storeWithProducts.NameStore);
        }

        /// <summary>
        /// Calcula la distancia entre la ubicación del usuario y una tienda con productos.
        /// </summary>
        /// <param name="storeWithProducts">Modelo de tienda con productos.</param>
        public async Task<double> CalculateDistanceAsync(StoreWithProductsModel storeWithProducts)
        {
            var userLocation = User.Identity.IsAuthenticated ? await GetLocationUserAsync() : new double[] { 0, 0 };

            return (userLocation[0] != 0 && userLocation[1] != 0) ?
                   Geolocation.CalculateDistance(userLocation[0], userLocation[1], storeWithProducts.Latitude, storeWithProducts.Longitude) / 1000 : 0;
        }

        /// <summary>
        /// Obtiene registros asociados a una tienda con productos, seleccionando el primer registro para cada producto.
        /// </summary>
        /// <param name="storeWithProducts">Modelo de tienda con productos.</param>
        public async Task<List<Record>> GetRecordsAsync(StoreWithProductsModel storeWithProducts)
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
        public async Task<double[]> GetLocationUserAsync()
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
