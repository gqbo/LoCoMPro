using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Pages.Lists
{
    public class DeleteModel : PageModel
    {
        private readonly LoComproContext _context;

        public DeleteModel(LoComproContext context)
        {
            _context = context;
        }

        [BindProperty]
      public List List { get; set; }

        /// <summary>
        /// Maneja las solicitudes GET para la página actual.
        /// Recupera una lista según el id y la asigna a "List".
        /// </summary>
        /// <param name="id">El identificador de la lista a recuperar.</param>
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.List == null)
            {
                return NotFound();
            }

            var list = await _context.List.FirstOrDefaultAsync(m => m.NameList == id);

            if (list == null)
            {
                return NotFound();
            }
            else 
            {
                List = list;
            }
            return Page();
        }

        /// <summary>
        /// Maneja las solicitudes POST para la página actual.
        /// Elimina una lista según el id y redirige a la página de índice.
        /// </summary>
        /// <param name="id">El identificador de la lista a eliminar.</param>
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.List == null)
            {
                return NotFound();
            }
            var list = await _context.List.FindAsync(id);

            if (list != null)
            {
                List = list;
                _context.List.Remove(List);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
