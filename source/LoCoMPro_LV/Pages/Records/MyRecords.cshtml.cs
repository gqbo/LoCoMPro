using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Data;

namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Página Create de Records para la creación de nuevos registros registros.
    /// </summary>
    public class MyRecordsModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;
        public MyRecordsModel(LoComproContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Enlazar los valores de las propiedades en un objeto con los datos provenientes de una solicitud HTTP.
        /// </summary>
        [BindProperty]
        public List<Record> Records { get; set; }

        /// <summary>
        /// Método invocado cuando se realiza una solicitud GET para mostrar los registros de un usuario.
        /// </summary>
        public async Task OnGetAsync()
        {
            string authenticatedUserName = User.Identity.Name;
            Records = await _context.Records
                .Include(r => r.Store)
                .Where(r => r.NameGenerator == authenticatedUserName)
                .ToListAsync();
        }
    }
}