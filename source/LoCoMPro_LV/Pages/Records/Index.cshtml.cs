using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoCoMPro_LV.Pages.Records
{
    public class IndexModel : PageModel
    {
        private readonly LoCoMPro_LV.Data.LoComproContext _context;

        public IndexModel(LoCoMPro_LV.Data.LoComproContext context)
        {
            _context = context;
        }

        public IList<Record> Record { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchProvince { get; set; }
        public SelectList Provinces { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCanton { get; set; }
        public SelectList Cantons { get; set; }
        
        public SelectList Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchCategory { get; set; }

        public string DateTimeSort { get; set; }
        public string PriceSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            DateTimeSort = sortOrder == "Date" ? "date_desc" : "Date";
            PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            CurrentFilter = searchString;

            var provinces = await _context.Provinces.ToListAsync();
            var cantons = await _context.Cantons.ToListAsync();
            var categories = await _context.Associated
                                    .Select(a => a.NameCategory)
                                    .Distinct()
                                    .ToListAsync();

            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");
            Cantons = new SelectList(cantons, "NameCanton", "NameCanton");
            Categories = new SelectList(categories);

            IQueryable<Record> orderedRecordsQuery = from m in _context.Records
                               select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.NameProduct.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(SearchProvince))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.NameProvince == SearchProvince);
            }

            if (!string.IsNullOrEmpty(SearchCanton))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.NameCanton == SearchCanton);
            }

            if (!string.IsNullOrEmpty(SearchCategory))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Product.Associated.Any(c => c.NameCategory == SearchCategory));
            }

            var groupedRecordsQuery = from record in orderedRecordsQuery
                                      group record by new
                                      { record.NameProduct, record.NameStore, record.NameCanton, record.NameProvince } into recordGroup
                                      orderby recordGroup.Key.NameProduct descending
                                      select recordGroup;

            var orderedGroupsQuery = groupedRecordsQuery;

            switch (sortOrder)
            {
                case "Date":
                    orderedGroupsQuery = groupedRecordsQuery.OrderBy(group => group.Max(record => record.RecordDate));
                    break;
                case "Price":
                    orderedGroupsQuery = groupedRecordsQuery.OrderBy(group => group.Max(record => record.Price));
                    break;
                case "price_desc":
                    orderedGroupsQuery = groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.Price));
                    break;
                default:
                    orderedGroupsQuery = groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.RecordDate));
                    break;
            }

            Record = await orderedGroupsQuery
                .Select(group => group.OrderByDescending(r => r.RecordDate).FirstOrDefault())
                .ToListAsync();

            SearchCanton = Request.Query["SearchCanton"];
        }
    }
}
