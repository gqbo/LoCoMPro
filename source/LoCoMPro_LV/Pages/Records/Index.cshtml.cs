﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Utils;


namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Página de índice de Records para la búsqueda y visualización de registros.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;

        /// <summary>
        /// Administra a los usuarios de tipo ApplicationUser.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(LoComproContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Lista de tipo "Record", que almacena los registros correspondientes al producto buscado.
        /// </summary>
        public IList<RecordStoreModel> Record { get; set; } = default!;


        /// <summary>
        /// Cadena de caracteres que se utiliza para filtrar la búsqueda por nombre del producto.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        /// <summary>
        /// Provincia utilizada como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchProvince { get; set; }

        /// <summary>
        /// Lista de provincias para la selección.
        /// </summary>
        public SelectList Provinces { get; set; }

        /// <summary>
        /// Cantón utilizado como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchCanton { get; set; }

        /// <summary>
        /// Diccionario de cantones para la selección.
        /// </summary>
        public Dictionary<string, List<string>> Cantons { get; set; }

        /// <summary>
        /// Categoría utilizada como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchCategory { get; set; }

        /// <summary>
        /// Lista de categorías para la selección.
        /// </summary>
        public SelectList Categories { get; set; }

        /// <summary>
        /// Indica el orden en el que se deben mostrar los registros por fecha.
        /// </summary>
        public string DateTimeSort { get; set; }

        /// <summary>
        /// Indica el orden en el que se deben mostrar los registros por precio.
        /// </summary>
        public string PriceSort { get; set; }

        /// <summary>
        /// El filtro actual aplicado para la búsqueda de registros.
        /// </summary>
        public string CurrentFilter { get; set; }

        /// <summary>
        /// Lista de provincias filtradas después de aplicar la búsqueda.
        /// </summary>
        public List<string> FilteredProvinces { get; set; }

        /// <summary>
        /// Lista de cantones filtradas después de aplicar la búsqueda.
        /// </summary>
        public List<string> FilteredCantons { get; set; }

        /// <summary>
        /// Lista de establecimientos filtradas después de aplicar la búsqueda.
        /// </summary>
        public List<string> FilteredStores { get; set; }

        /// <summary>
        /// Método que se ejecuta cuando se carga la página, se realiza la búsqueda y paginación de registros.
        /// </summary>
        /// <param name="sortOrder">El orden en el que se deben mostrar los registros.</param>
        public async Task OnGetAsync(string sortOrder)
        {
            await InitializeSortingAndSearching(sortOrder);
            var userLocation = new double[] { 0, 0 };
            if (User.Identity.IsAuthenticated)
            {
                userLocation = await GetLocationUserAsync();
            }
            var orderedRecordsQuery = BuildOrderedRecordsQuery(userLocation);
            var orderedGroupsQuery = ApplySorting(orderedRecordsQuery);
            var totalCount = await orderedGroupsQuery.CountAsync();

            Record = await orderedGroupsQuery
                .Where(group => group.Any(record => record.Record.Hide == false))
                .Select(group => group.OrderByDescending(r => r.Record.RecordDate).FirstOrDefault())
                .ToListAsync();
        }

        /// <summary>
        /// Inicializa las opciones de ordenamiento y los datos de búsqueda avanzada.
        /// </summary>
        /// <param name="sortOrder">El orden en el que se deben mostrar los registros.</param>
        private async Task InitializeSortingAndSearching(string sortOrder)
        {
            DateTimeSort = sortOrder == "Date" ? "date_desc" : "Date";
            PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

            var provinces = await _context.Provinces.ToListAsync();
            var cantons = await _context.Cantons.ToListAsync();
            var categories = await _context.Associated
                .Select(a => a.NameCategory)
                .Distinct()
                .ToListAsync();

            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");
            Cantons = new Dictionary<string, List<string>>();
            foreach (var canton in cantons)
            {
                if (!Cantons.ContainsKey(canton.NameProvince))
                {
                    Cantons[canton.NameProvince] = new List<string>();
                }
                Cantons[canton.NameProvince].Add(canton.NameCanton);
            }
            Categories = new SelectList(categories);
        }

        /// <summary>
        /// Construye la consulta de registros ordenados basada en las opciones de búsqueda.
        /// <param name="LocationUser"> Contiene las coordenadas del usuario.</param>
        /// </summary>
        /// <returns>Consulta de registros ordenados.</returns>
        private IQueryable<RecordStoreModel> BuildOrderedRecordsQuery(double[] LocationUser)
        {
            var LatitudeUser = LocationUser[0];
            var LongitudeUser = LocationUser[1];
            IQueryable<RecordStoreModel> orderedRecordsQuery = from record in _context.Records
                                                               join store in _context.Stores on new { record.NameStore, record.Latitude, record.Longitude }
                                                               equals new { store.NameStore, store.Latitude, store.Longitude }
                                                               where record.Hide == false
                                                               select new RecordStoreModel
                                                               {
                                                                   Record = record,
                                                                   Store = store,
                                                                   Distance = (LatitudeUser != 0 && LongitudeUser != 0) ?
                                                                      Geolocation.CalculateDistance(LatitudeUser, LongitudeUser, store.Latitude, store.Longitude) / 1000 : 0
                                                               };

            if (!string.IsNullOrEmpty(SearchString))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Record.NameProduct.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(SearchProvince))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Store.NameProvince == SearchProvince);
            }

            if (!string.IsNullOrEmpty(SearchCanton))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Store.NameCanton == SearchCanton);
            }

            if (!string.IsNullOrEmpty(SearchCategory))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Record.Product.Associated.Any(c => c.NameCategory == SearchCategory));
            }

            return orderedRecordsQuery;
        }

        /// <summary>
        /// Aplica el ordenamiento a la consulta de registros.
        /// </summary>
        /// <param name="orderedRecordsQuery">Consulta de registros ordenados.</param>
        /// <returns>Consulta de registros ordenados con el orden especificado.</returns>
        private IOrderedQueryable<IGrouping<object, RecordStoreModel>> ApplySorting(IQueryable<RecordStoreModel> orderedRecordsQuery)
        {
            var groupedRecordsQuery = from record in orderedRecordsQuery
                                      group record by new
                                      {
                                          record.Record.NameProduct,
                                          record.Record.NameStore,
                                          record.Store.NameCanton,
                                          record.Store.NameProvince
                                      } into recordGroup
                                      orderby recordGroup.Key.NameProduct descending
                                      select recordGroup;
            
            FilteredProvinces = groupedRecordsQuery.Select(group => group.Key.NameProvince).Distinct().ToList();
            FilteredCantons = groupedRecordsQuery.Select(group => group.Key.NameCanton).Distinct().ToList();
            FilteredStores = groupedRecordsQuery.Select(group => group.Key.NameStore).Distinct().ToList();

            return groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.Record.RecordDate));
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

