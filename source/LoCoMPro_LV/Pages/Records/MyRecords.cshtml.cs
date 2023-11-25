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

        [BindProperty]
        public string AuthenticatedUserName { get; set; }

        [BindProperty]
        public DateTime RecordDate { get; set; }

        /// <summary> 
        /// Método invocado cuando se realiza una solicitud GET para mostrar los registros de un usuario.
        /// </summary>
        public async Task OnGetAsync()
        {
            AuthenticatedUserName = User.Identity.Name;
            Records = await _context.Records
                .Include(r => r.Store)
                .Where(r => r.NameGenerator == AuthenticatedUserName)
                .ToListAsync();
        }

        /// <summary>
        /// Este metodo se utiliza cuando se va a eliminar un registro realizado por un usuario registrado.
        /// </summary>
        public async Task<IActionResult> OnPostEliminarRegistro()
        {
            var record = await _context.Records
                .FirstOrDefaultAsync(r => r.NameGenerator == User.Identity.Name && r.RecordDate == RecordDate);
            if (record != null)
            {
                var reports = await _context.Reports
                    .Where(r => r.NameGenerator == User.Identity.Name && r.RecordDate == RecordDate)
                    .ToListAsync();
                if (reports != null)
                {
                    foreach (var report in reports)
                    {
                        _context.Reports.Remove(report);
                    }
                }
                var valorations = await _context.Valorations
                    .Where(v => v.NameGenerator == User.Identity.Name && v.RecordDate == RecordDate)
                    .ToListAsync();

                if (valorations != null)
                {
                    foreach (var valoration in valorations)
                    {
                        _context.Valorations.Remove(valoration);
                    }
                }
                _context.Records.Remove(record);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("./MyRecords");
        }
    }
}