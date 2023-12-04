using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Pages.Lists
{
    public class EditModel : PageModel
    {
        private readonly LoComproContext _context;

        public EditModel(LoComproContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List List { get; set; } = default!;

        /// <summary>
        /// Maneja las solicitudes GET para la página actual.
        /// Recupera y muestra los detalles de una lista según el identificador proporcionado.
        /// </summary>
        /// <param name="id">Identificador de la lista.</param>
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.List == null)
            {
                return NotFound();
            }

            var list =  await _context.List.FirstOrDefaultAsync(m => m.NameList == id);
            if (list == null)
            {
                return NotFound();
            }
            List = list;
           ViewData["UserName"] = new SelectList(_context.GeneratorUsers, "UserName", "UserName");
            return Page();
        }

        /// <summary>
        /// Maneja las solicitudes POST para la página actual.
        /// Actualiza la lista en la base de datos después de una modificación.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(List).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(List.NameList))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        /// <summary>
        /// Verifica si una lista con el identificador dado existe en la base de datos.
        /// </summary>
        /// <param name="id">Identificador de la lista.</param>
        private bool ListExists(string id)
        {
          return _context.List.Any(e => e.NameList == id);
        }
    }
}
