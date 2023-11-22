using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using LoCoMPro_LV.Pages.Records;
using LoCoMPro_LV.Models;
using System.Collections.Generic;

namespace LoCoMPro_LV.Pages.Reports
{
    /// <summary>
    /// Página de índice de Records para la búsqueda y visualización de registros.
    /// </summary>
    public class AnomaliesModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;

        public AnomaliesModel(LoComproContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista de tipo "Record", que almacena los registros correspondientes al producto buscado.
        /// </summary>
        public IList<RecordStoreModel> Record { get; set; } = default!;

        /// <summary>
        /// Cadena de caracteres que se utiliza para filtrar la búsqueda por nombre del producto.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        /// <summary>
        /// Provincia utilizada como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchProvince { get; set; }

        /// <summary>
        /// Lista de provincias para la selección.
        /// </summary>
        public SelectList Provinces { get; set; }

        /// <summary>
        /// Cantón utilizado como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchCanton { get; set; }

        /// <summary>
        /// Diccionario de cantones para la selección.
        /// </summary>
        public Dictionary<string, List<string>> Cantons { get; set; }

        /// <summary>
        /// Categoría utilizada como filtro de búsqueda.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchCategory { get; set; }

        /// <summary>
        /// Lista de categorías para la selección.
        /// </summary>
        public SelectList Categories { get; set; }

        /// <summary>
        /// Indica el orden en el que se deben mostrar los registros por fecha.
        /// </summary>
        public string DateTimeSort { get; set; }

        /// <summary>
        /// Indica el orden en el que se deben mostrar los registros por precio.
        /// </summary>
        public string PriceSort { get; set; }

        /// <summary>
        /// El filtro actual aplicado para la búsqueda de registros.
        /// </summary>
        public string CurrentFilter { get; set; }

        /// <summary>
        /// Método que se ejecuta cuando se carga la página y se realiza la búsqueda y paginación de registros.
        /// </summary>
        /// <param name="sortOrder">El orden en el que se deben mostrar los registros.</param>
        public async Task OnGetAsync(string sortOrder)
        {
            await InitializeSortingAndSearching(sortOrder);
            var orderedRecordsQuery = BuildOrderedRecordsQuery();
            List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords = GroupRecords(orderedRecordsQuery);
            await ProcessGroupedRecords(groupedRecords);

            var orderedGroupsQuery = ApplySorting(orderedRecordsQuery, sortOrder);
            var totalCount = await orderedGroupsQuery.CountAsync();
            Record = await orderedGroupsQuery
                .Where(group => group.Any(record => record.Record.Hide == false))
                .Select(group => group.OrderByDescending(r => r.Record.RecordDate).FirstOrDefault())
                .ToListAsync();
        }

        /// <summary>
        /// Inicializa las opciones de ordenamiento y los datos de búsqueda avanzada.
        /// </summary>
        /// <param name="sortOrder">El orden en el que se deben mostrar los registros.</param>
        private async Task InitializeSortingAndSearching(string sortOrder)
        {
            DateTimeSort = sortOrder == "Date" ? "date_desc" : "Date";
            PriceSort = sortOrder == "Price" ? "price_desc" : "Price";

            var provinces = await _context.Provinces.ToListAsync();
            var cantons = await _context.Cantons.ToListAsync();
            var categories = await _context.Associated
                .Select(a => a.NameCategory)
                .Distinct()
                .ToListAsync();

            Provinces = new SelectList(provinces, "NameProvince", "NameProvince");
            Cantons = new Dictionary<string, List<string>>();
            foreach (var canton in cantons)
            {
                if (!Cantons.ContainsKey(canton.NameProvince))
                {
                    Cantons[canton.NameProvince] = new List<string>();
                }
                Cantons[canton.NameProvince].Add(canton.NameCanton);
            }
            Categories = new SelectList(categories);
        }

        /// <summary>
        /// Construye la consulta de registros ordenados basada en las opciones de búsqueda.
        /// </summary>
        /// <returns>Consulta de registros ordenados.</returns>
        private IQueryable<RecordStoreModel> BuildOrderedRecordsQuery()
        {
            IQueryable<RecordStoreModel> orderedRecordsQuery = from record in _context.Records
                                                               join store in _context.Stores on new { record.NameStore, record.Latitude, record.Longitude }
                                                               equals new { store.NameStore, store.Latitude, store.Longitude }
                                                            //   where record.Hide == false
                                                               select new RecordStoreModel
                                                               {
                                                                   Record = record,
                                                                   Store = store
                                                               };

            if (!string.IsNullOrEmpty(SearchString))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Record.NameProduct.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(SearchProvince))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Store.NameProvince == SearchProvince);
            }

            if (!string.IsNullOrEmpty(SearchCanton))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Store.NameCanton == SearchCanton);
            }

            if (!string.IsNullOrEmpty(SearchCategory))
            {
                orderedRecordsQuery = orderedRecordsQuery.Where(s => s.Record.Product.Associated.Any(c => c.NameCategory == SearchCategory));
            }

            return orderedRecordsQuery;
        }

        /// <summary>
        /// Aplica el ordenamiento a la consulta de registros.
        /// </summary>
        /// <param name="orderedRecordsQuery">Consulta de registros ordenados.</param>
        /// <param name="sortOrder">El orden en el que se deben mostrar los registros.</param>
        /// <returns>Consulta de registros ordenados con el orden especificado.</returns>
        private IOrderedQueryable<IGrouping<object, RecordStoreModel>> ApplySorting(IQueryable<RecordStoreModel> orderedRecordsQuery, string sortOrder)
        {
            var groupedRecordsQuery = from record in orderedRecordsQuery
                                      group record by new
                                      {
                                          record.Record.NameProduct,
                                          record.Record.NameStore,
                                          record.Store.NameCanton,
                                          record.Store.NameProvince
                                      } into recordGroup
                                      orderby recordGroup.Key.NameProduct descending
                                      select recordGroup;

            switch (sortOrder)
            {
                case "Date":
                    return groupedRecordsQuery.OrderBy(group => group.Max(record => record.Record.RecordDate));
                case "Price":
                    return groupedRecordsQuery.OrderBy(group => group.Max(record => record.Record.Price));
                case "price_desc":
                    return groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.Record.Price));
                default:
                    return groupedRecordsQuery.OrderByDescending(group => group.Max(record => record.Record.RecordDate));
            }
        }

        public class GroupingKey
        {
            public string NameProduct { get; set; }
            public string NameStore { get; set; }
            public string NameCanton { get; set; }
            public string NameProvince { get; set; }
        }

        private List<IGrouping<GroupingKey, RecordStoreModel>> GroupRecords(IQueryable<RecordStoreModel> orderedRecordsQuery)
        {
            return orderedRecordsQuery.GroupBy(
                record => new GroupingKey
                {
                    NameProduct = record.Record.NameProduct,
                    NameStore = record.Record.NameStore,
                    NameCanton = record.Store.NameCanton,
                    NameProvince = record.Store.NameProvince
                }
            ).ToList();
        }

        private async Task ProcessGroupedRecords(List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords)
        {
            List<RecordStoreModel> recordsGroupContainer = new List<RecordStoreModel>();
            
            foreach (var group in groupedRecords)
            {
                GroupingKey groupKey = group.Key;

                foreach (var record in group)
                {
                    recordsGroupContainer.Add(record);
                }
                AnomaliesDate(recordsGroupContainer);
                AnomaliesPrice(recordsGroupContainer);
                recordsGroupContainer.Clear();
            }
        }

        private async Task AnomaliesDate(List<RecordStoreModel> recordsGroupContainer)
        {
            List<RecordStoreModel> selectedRecords = new List<RecordStoreModel>();
            var sortedRecords = recordsGroupContainer.OrderBy(r => r.Record.RecordDate).ToList();
            int endIndex = (int)(sortedRecords.Count * 0.5);
            var selectedRecordsSubset = sortedRecords.Take(endIndex).ToList();

            selectedRecords.AddRange(selectedRecordsSubset.Where(r => r.Record.Hide == false));

            foreach (var indice in selectedRecords)
            {
                Anomalie anomalie = new Anomalie
                {
                    NameGenerator = indice.Record.NameGenerator,
                    RecordDate = indice.Record.RecordDate,
                    Type = "Fecha",
                    Comment = "La fecha es muy antigua",
                    State = 0
                };
                _context.Anomalies.Add(anomalie);
                await _context.SaveChangesAsync();
            }
            selectedRecordsSubset.Clear();
            endIndex = 0;
            sortedRecords.Clear();
            selectedRecords.Clear();
        }

        private async Task AnomaliesPrice(List<RecordStoreModel> recordsGroupContainer)
        {
            List<RecordStoreModel> selectedRecords = new List<RecordStoreModel>();
            var sortedRecords = recordsGroupContainer.OrderBy(r => r.Record.Price).ToList();  // Ordena por precio

            // Calcula Q2
            int q2Index = CalculateQ2Index(sortedRecords.Count);

            // Calcula Q1 y Q3 utilizando los índices de Q2
            int q1Index = CalculateQ1Index(q2Index);
            int q3Index = CalculateQ3Index(q2Index);
            double? q1;
            double? q3;

            // Obtiene los valores reales de Q1, Q2 y Q3
            if (q2Index % 2 == 0)
            {
                q1 = ((sortedRecords[q1Index].Record.Price) + (sortedRecords[q1Index + 1].Record.Price)) / 2;
                q3 = ((sortedRecords[q3Index].Record.Price) + (sortedRecords[q3Index - 1].Record.Price)) / 2;
            }
            else
            {
                q1 = sortedRecords[q1Index].Record.Price;
                q3 = sortedRecords[q3Index].Record.Price;
            }

            // Calcula el RIC
            double? ric = q3 - q1;

            // Define el umbral (puedes ajustar este valor según tus necesidades)
            double umbral = 1.5;

            // Establece el rango para valores no atípicos
            double? lowerBound = q1 - umbral * ric;
            double? upperBound = q3 + umbral * ric;

            // Identifica los valores atípicos y agrégales a selectedRecords
            foreach (var indice in sortedRecords)
            {
                if (indice.Record.Price < lowerBound || indice.Record.Price > upperBound)
                {
                    Anomalie anomalie = new Anomalie
                    {
                        NameGenerator = indice.Record.NameGenerator,
                        RecordDate = indice.Record.RecordDate,
                        Type = "Precio",
                        Comment = "El precio es anómalo",
                        State = 0
                    };
                    _context.Anomalies.Add(anomalie);
                    await _context.SaveChangesAsync();
                }
            }

            // Limpia la lista de registros seleccionados
            selectedRecords.Clear();
        }

        private int CalculateQ2Index(int recordCount)
        {
            // La mediana siempre es el valor en el centro o el promedio de los dos valores centrales
            if (recordCount % 2 == 0)
            {
                // Si la cantidad de datos es par, la mediana es el índice del valor en el centro
                return recordCount / 2;
            }
            else
            {
                // Si la cantidad de datos es impar, la mediana es el índice del valor en el centro
                return ((recordCount - 1) / 2) + 1;
            }
        }

        private int CalculateQ1Index(int q2)
        {
            if (q2 % 2 == 0)
            {
                return (int)((q2 / 2) - 1);
            }
            else
            {
                return (int)(((q2 - 1) / 2));
            }
        }

        private int CalculateQ3Index(int q2)
        {
            if (q2 % 2 == 0)
            {
                return (int)((q2 + (q2 / 2)) - 1);
            }
            else
            {
                return (int)(q2 + ((q2 - 1) / 2));
            }
        }
    }
}
