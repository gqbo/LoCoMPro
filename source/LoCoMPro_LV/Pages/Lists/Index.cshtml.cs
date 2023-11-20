using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Pages.Lists
{
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;

        public IndexModel(LoComproContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Es el nombre de la lista del ususrio que se esta presentando
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameList { get; set; }

        /// <summary>
        /// Es un atributo que se utiliza al eliminar un producto de la lista para almacxenar el nombre de ese producto.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameProduct { get; set; }

        /// <summary>
        /// Es una lista de productos listados para almacenar los productos que pertenecen a los usuarios de la lista.
        /// </summary>
        public IList<Listed> Listed { get;set; } = default!;

        /// <summary>
        /// En este metodo se buscan los productos que estan listados en la lista del usuario para presentarlo al usuario.
        /// </summary>
        public async Task OnGetAsync()
        {
            var listed =  from listed_item in _context.Listed
                              where listed_item.NameList == User.Identity.Name
                              select listed_item;

            Listed = await listed.ToListAsync();
        }

        /// <summary>
        /// Este metodo se utiliza cuando se va a eliminar un producto de la lista para que entonces se bsuque el producto en la tabla
        /// de Listed y se elimine.
        /// </summary>
        public async Task<IActionResult> OnPostEliminarItem()
        {
            var listed = await _context.Listed
                .FirstOrDefaultAsync(m => m.NameProduct == NameProduct && m.NameList == User.Identity.Name);

            if (listed != null)
            {
                _context.Listed.Remove(listed);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
