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
        public DateTime RecordDate { get; set; }

        [BindProperty]
        public string Username { get; set; }

        /// <summary> 
        /// Método invocado cuando se realiza una solicitud GET para mostrar los registros de un usuario.
        /// </summary>
        public async Task OnGetAsync()
        {
            Records = await _context.Records
                .Include(r => r.Store)
                .Where(r => r.NameGenerator == User.Identity.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Elimina un registro realizado por un usuario registrado.
        /// </summary>
        public async Task<IActionResult> OnPostDeleteRecord()
        {
            var record = await _context.Records
                .FirstOrDefaultAsync(r => r.NameGenerator == Username && r.RecordDate == RecordDate);
            if (record != null)
            {
                await DeleteReports(Username);
                await DeleteValorations(Username);
                _context.Records.Remove(record);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("./MyRecords");
        }

        /// <summary>
        /// Elimina los reportes relacionados al registro a eliminar.
        /// </summary>
        public async Task DeleteReports(string Username)
        {
            var reports = await GetReports(Username);
            if (reports != null)
            {
                foreach (var report in reports)
                {
                    _context.Reports.Remove(report);
                }
            }
        }

        /// <summary>
        /// Elimina las valoraciones relacionadas al registro a eliminar.
        /// </summary>
        public async Task DeleteValorations(string Username)
        {
            var valorations = await GetValorations(Username);
            if (valorations != null)
            {
                foreach (var valoration in valorations)
                {
                    _context.Valorations.Remove(valoration);
                }
            }
        }

        /// <summary>
        /// Obtiene las valoraciones relacionadas al registro a eliminar.
        /// </summary>
        public async Task<List<Evaluate>> GetValorations(string Username)
        {
            var valorations = await _context.Valorations
                .Where(v => v.NameGenerator == Username && v.RecordDate == RecordDate)
                .ToListAsync();
            return valorations;
        }

        /// <summary>
        /// Obtiene los reportes relacionadas al registro a eliminar.
        /// </summary>
        public async Task<List<Report>> GetReports(string Username)
        {
            var reports = await _context.Reports
                .Where(r => r.NameGenerator == Username && r.RecordDate == RecordDate)
                .ToListAsync();
            return reports;
        }
    }
}