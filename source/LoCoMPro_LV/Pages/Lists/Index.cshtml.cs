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
        public async Task<IActionResult> OnGetAsync()
        {
            var listed = await GetListedItemsAsync(User.Identity.Name);

            Listed = listed;

            return Page();
        }

        public async Task<List<Listed>> GetListedItemsAsync(string userName)
        {
            var listed = from listedItem in _context.Listed
                         where listedItem.NameList == userName
                         select listedItem;

            return await listed.ToListAsync();
        }


        /// <summary>
        /// Este metodo se utiliza cuando se va a eliminar un producto de la lista para que entonces se bsuque el producto en la tabla
        /// de Listed y se elimine.
        /// </summary>
        public async Task<IActionResult> OnPostEliminarItem()
        {
            var listed = await GetListedItemAsync(User.Identity.Name);

            if (listed != null)
            {
                RemoveListedItem(listed);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public async Task<Listed> GetListedItemAsync(string userName)
        {

            return await _context.Listed
                .FirstOrDefaultAsync(m => m.NameProduct == NameProduct && m.NameList == userName);
        }

        private void RemoveListedItem(Listed listed)
        {
            if (listed != null)
            {
                _context.Listed.Remove(listed);
            }
        }
    }
}
