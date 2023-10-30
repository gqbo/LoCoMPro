using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LoCoMPro_LV.Pages.Records
{
    public class MyRecordsModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public MyRecordsModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        public List<Record> Records { get; set; }

        public string eleccion { get; set; }
        public string sortOrder { get; set; }

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



        private IQueryable<Record> ApplyDateSorting(IQueryable<Record> query, string sortOrder)
        {
            bool isDateDescending = string.Equals(sortOrder, "Date_desc", StringComparison.OrdinalIgnoreCase);
            query = isDateDescending ? query.OrderByDescending(r => r.RecordDate) : query.OrderBy(r => r.RecordDate);
            ViewData["DateSort"] = isDateDescending ? "Date" : "Date_desc";
            return query;
        }
        private IQueryable<Record> ApplyPriceSorting(IQueryable<Record> query, string sortOrder)
        {
            bool isPriceDescending = string.Equals(sortOrder, "Price_desc", StringComparison.OrdinalIgnoreCase);
            query = isPriceDescending ? query.OrderByDescending(r => r.Price) : query.OrderBy(r => r.Price);
            ViewData["PriceSort"] = isPriceDescending ? "Price" : "Price_desc";
            return query;
        }
    }
}