using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Utils;
using Microsoft.AspNetCore.Identity;

namespace LoCoMPro_LV.Pages.Lists
{
    public class ResultListModel : PageModel
    {
        private readonly LoComproContext _context;

        /// <summary>
        /// Administra a los usuarios de tipo ApplicationUser.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;


        public ResultListModel(LoComproContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List List { get; set; }

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

        public ListSearchResults Result { get; set; } = new ListSearchResults();

        public async Task<IActionResult> OnGetAsync()
        {
            var firstRecord = await GetFirstRecordAsync();

            Result.Store = await GetStoreForRecordAsync(firstRecord);

            if (User.Identity.IsAuthenticated)
            {
                Result.Distance = await CalculateDistanceToStoreAsync(firstRecord);
            }

            Result.Records = await GetRecordsForStoreAsync(firstRecord);

            Result.totalPrice = CalculateTotalPrice(Result.Records);

            return Page();
        }

        /// <summary>
        /// Obtiene el primer registro de la base de datos.
        /// </summary>
        public async Task<Record> GetFirstRecordAsync()
        {
            return await _context.Records
                .FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator && m.RecordDate == RecordDate);
        }

        /// <summary>
        /// Obtiene la tienda asociada a un registro.
        /// </summary>
        public async Task<Store> GetStoreForRecordAsync(Record record)
        {
            return await _context.Stores
                .FirstOrDefaultAsync(m => m.Latitude == record.Latitude && m.Longitude == record.Longitude && m.NameStore == record.NameStore);
        }

        /// <summary>
        /// Calcula la distancia entre la ubicación del usuario y la tienda asociada a un registro.
        /// </summary>
        public async Task<double> CalculateDistanceToStoreAsync(Record record)
        {
            var userLocation = new double[] { 0, 0 };
            if (User.Identity.IsAuthenticated)
            {
                userLocation = await GetLocationUserAsync();
            }

            return (userLocation[0] != 0 && userLocation[1] != 0) ?
                   Geolocation.CalculateDistance(userLocation[0], userLocation[1], record.Latitude, record.Longitude) / 1000 : 0;
        }

        /// <summary>
        /// Obtiene los registros asociados a la tienda de un registro.
        /// </summary>
        public async Task<IList<Record>> GetRecordsForStoreAsync(Record record)
        {
            var query = from r in _context.Records
                        join listed in _context.Listed
                        on r.NameProduct equals listed.NameProduct
                        where r.NameStore == record.NameStore
                              && r.Latitude == record.Latitude
                              && r.Longitude == record.Longitude
                        orderby r.RecordDate descending
                        group r by r.NameProduct into grouped
                        select grouped.First();

            return await query.ToListAsync();
        }

        /// <summary>
        /// Calcula el precio total de una lista de registros.
        /// </summary>
        public double? CalculateTotalPrice(IList<Record> records)
        {
            double? totalPrice = 0;
            foreach (var record in records)
            {
                totalPrice += record.Price;
            }

            return totalPrice;
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
