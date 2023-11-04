using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Models;
using LoCoMPro_LV.Data;

namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Página para visualizar mis aportes.
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
        /// Construye un método de llamado a record para pasar datos del HTML 
        /// </summary>
        public List<Record> Records { get; set; }

        /// <summary>
        /// String que permite elegir entorno a que atributo se ordenara.
        /// </summary>
        public string eleccion { get; set; }

        /// <summary>
        /// String que permite elegir el orden.
        /// </summary>
        public string sortOrder { get; set; }

        /// <summary>
        /// Permite mostrar los resultados de Mis Aportes en la vista del mismo nombre. 
        /// </summary>
        public async Task OnGet(string sortOrder, string eleccion)
        {
            this.eleccion = eleccion;
            this.sortOrder = sortOrder;
            string authenticatedUserName = User.Identity.Name;

            IQueryable<Record> query = _context.Records.Include(r => r.Store).Where(r => r.NameGenerator == authenticatedUserName);

            if (eleccion == "fecha")
            {
                query = ApplyDateSorting(query, sortOrder);
            }
            else if (eleccion == "precio")
            {
                query = ApplyPriceSorting(query, sortOrder);
            }
            Records = await query.ToListAsync();
        }

        /// <summary> 
        /// Permite realizar un ordenamiento por medio de la fecha ya sea en forma ascendente o descendente.
        /// </summary>
        private IQueryable<Record> ApplyDateSorting(IQueryable<Record> query, string sortOrder)
        {
            bool isDateDescending = string.Equals(sortOrder, "Date_desc", StringComparison.OrdinalIgnoreCase);
            query = isDateDescending ? query.OrderByDescending(r => r.RecordDate) : query.OrderBy(r => r.RecordDate);
            ViewData["DateSort"] = isDateDescending ? "Date" : "Date_desc";
            return query;
        }

        /// <summary> 
        /// Permite realizar un ordenamiento por medio del precio ya sea en forma ascendente o descendente.
        /// </summary>
        private IQueryable<Record> ApplyPriceSorting(IQueryable<Record> query, string sortOrder)
        {
            bool isPriceDescending = string.Equals(sortOrder, "Price_desc", StringComparison.OrdinalIgnoreCase);
            query = isPriceDescending ? query.OrderByDescending(r => r.Price) : query.OrderBy(r => r.Price);
            ViewData["PriceSort"] = isPriceDescending ? "Price" : "Price_desc";
            return query;
        }
    }
}