using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var FirstRecord = await _context.Records
                .FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator && m.RecordDate == RecordDate);

            Result.Store = await _context.Stores
                .FirstOrDefaultAsync(m => m.Latitude == FirstRecord.Latitude && m.Longitude == FirstRecord.Longitude && m.NameStore == FirstRecord.NameStore);

            var userLocation = new double[] { 0, 0 };
            if (User.Identity.IsAuthenticated)
            {
                userLocation = await GetLocationUserAsync();
            }

            Result.Distance = (userLocation[0] != 0 && userLocation[1] != 0) ?
                              Geolocation.CalculateDistance(userLocation[0], userLocation[1], FirstRecord.Latitude, FirstRecord.Longitude) / 1000 : 0;

            var query = from record in _context.Records
                        join listed in _context.Listed
                        on record.NameProduct equals listed.NameProduct
                        where record.NameStore == FirstRecord.NameStore
                        && record.Latitude == FirstRecord.Latitude
                        && record.Longitude == FirstRecord.Longitude
                        orderby record.RecordDate descending
                        group record by record.NameProduct into grouped
                        select grouped.First();

            Result.Records = await query.ToListAsync();

            foreach (var record in Result.Records)
            {
                Result.totalPrice += record.Price;
            }

            return Page();
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
