using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Pages.Records;
using LoCoMPro_LV.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoCoMPro_LV.Pages.Reports
{
    /// <summary>
    /// Página que muestra las anomalias y genera reportes del sistema que indican la anomalía encontrada.
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
        /// Lista de tipo "Anomalie", que almacena los registros anómalos encontrados por el sistema.
        /// </summary>
        public List<Anomalie> Anomalies { get; set; }

        /// <summary>
        /// Método que se ejecuta cuando se carga la página y se realiza la búsqueda y paginación de registros anómalos.
        /// </summary>
        public async Task OnGetAsync()
        {
            var orderedRecordsQuery = BuildOrderedRecordsQuery();
            List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords = GroupRecords(orderedRecordsQuery);
            Anomalies = await _context.Anomalies
                .Where(a => a.State == 0)
                .ToListAsync();
        }

        /// <summary>
        /// Procesa y ejecuta la detección de anomalías relacionadas con los precios en los registros ordenados.
        /// </summary>
        /// <returns>Una acción que representa el resultado de la operación.</returns>
        public async Task<IActionResult> OnPostRunAnomaliesPrecio()
        {
            await LimpiarTablaAnomalies();
            var orderedRecordsQuery = BuildOrderedRecordsQuery();
            List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords = GroupRecords(orderedRecordsQuery);
            await ProcessGroupedRecordsPrice(groupedRecords);

            return new JsonResult(new { success = true });
        }

        /// <summary>
        /// Método que carga los datos anómalos por fecha.
        /// Realiza una serie de llamados que validan la consistencia de los datos que se desean añadir en la base de datos.
        /// </summary>
        public async Task<IActionResult> OnPostRunAnomaliesFecha()
        {
            await LimpiarTablaAnomalies();
            var orderedRecordsQuery = BuildOrderedRecordsQuery();
            List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords = GroupRecords(orderedRecordsQuery);
            await ProcessGroupedRecordsDate(groupedRecords);

            return new JsonResult(new { success = true });
        }

        /// <summary>
        /// Limpia la tabla de anomalías para eliminar los registros que dejaron de ser anómalos.
        /// </summary>
        private async Task LimpiarTablaAnomalies()
        {
            var allAnomalies = await _context.Anomalies.ToListAsync();
            _context.Anomalies.RemoveRange(allAnomalies);
            await _context.SaveChangesAsync();
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
                                                               select new RecordStoreModel
                                                               {
                                                                   Record = record,
                                                                   Store = store
                                                               };
            return orderedRecordsQuery;
        }

        /// <summary>
        /// Clase que representa una clave de agrupación para registros.
        /// Contiene propiedades que representan los atributos de agrupación.
        /// </summary>
        public class GroupingKey
        {
            public string NameProduct { get; set; }
            public string NameStore { get; set; }
            public string NameCanton { get; set; }
            public string NameProvince { get; set; }
        }

        /// <summary>
        /// Agrupa registros de tiendas utilizando una clave personalizada.
        /// </summary>
        /// <param name="orderedRecordsQuery">Consulta de registros ordenados.</param>
        /// <returns>Una lista de grupos de registros agrupados por una clave personalizada.</returns>
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

        /// <summary>
        /// Método que recorre los diferentes grupos para enviar a validar los datos anómalos en agrupamientos con más de 4 datos.
        /// Realiza llamados a un método que verifica los datos anómalos por fecha.
        /// <param name="groupedRecords">Datos agrupados.</param>
        /// </summary>
        public async Task ProcessGroupedRecordsDate(List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords)
        {
            List<RecordStoreModel> recordsGroupContainer = new List<RecordStoreModel>();
            foreach (var group in groupedRecords)
            {
                GroupingKey groupKey = group.Key;

                foreach (var record in group)
                {
                    recordsGroupContainer.Add(record);
                }
                if (recordsGroupContainer.Count() > 4)
                {
                    await AnomaliesDate(recordsGroupContainer);
                }

                recordsGroupContainer.Clear();
            }
        }

        /// <summary>
        /// Ejecuta la detección de anomalías relacionadas con los precios en los registros agrupados.
        /// </summary>
        /// <param name="groupedRecords">La lista de registros agrupados.</param>
        public async Task ProcessGroupedRecordsPrice(List<IGrouping<GroupingKey, RecordStoreModel>> groupedRecords)
        {
            List<RecordStoreModel> recordsGroupContainer = new List<RecordStoreModel>();
            foreach (var group in groupedRecords)
            {
                GroupingKey groupKey = group.Key;

                foreach (var record in group)
                {
                    recordsGroupContainer.Add(record);
                }
                if (recordsGroupContainer.Count() > 6)
                {
                    await AnomaliesPrice(recordsGroupContainer);
                }
                recordsGroupContainer.Clear();
            }
        }

        /// <summary>
        /// Método valida los agrupamientos que cumplen los requisitos para ser un dato anómalo.
        /// <param name="recordsGroupContainer">Grupo de los registros del mismo producto por tienda.</param>
        /// </summary>
        public async Task AnomaliesDate(List<RecordStoreModel> recordsGroupContainer)
        {
            List<RecordStoreModel> selectedRecords = new List<RecordStoreModel>();

            heuristicDate(recordsGroupContainer, out var sortedRecords, out var referenceDate);

            selectedRecords.AddRange(sortedRecords.Where(r => r.Record.RecordDate < referenceDate &&  r.Record.Hide == false ));

            foreach (var indice in selectedRecords)
            {
                if (!_context.Anomalies.Any(a =>
                a.NameGenerator == indice.Record.NameGenerator &&
                a.RecordDate == indice.Record.RecordDate &&
                a.Type == "Fecha"))
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
            }
            sortedRecords.Clear();
            selectedRecords.Clear();
        }

        /// <summary>
        /// Método que valida la heuristica de la fecha.
        /// <param name="recordsGroupContainer">Grupo de los registros del mismo producto por tienda.</param>
        /// <param name="sortedRecords">Posee una lista con los datos ordenados.</param>
        /// <param name="referenceDate">Dato que se utiliza para construir una fecha de referencia desde la cual considerar como datos anómalos.</param>
        /// </summary>
        public void heuristicDate(List<RecordStoreModel> recordsGroupContainer, out List<RecordStoreModel> sortedRecords, out DateTime referenceDate)
        {
            sortedRecords = recordsGroupContainer.OrderBy(r => r.Record.RecordDate).ToList();

            int startIndex = (int)(sortedRecords.Count * 0.30);

            DateTime startDate = sortedRecords[startIndex].Record.RecordDate;

            DateTime endDate = sortedRecords[sortedRecords.Count - 1].Record.RecordDate;

            int delta = 2;
            int daysDifference = (int)(endDate - startDate).Days * delta;
            referenceDate = startDate.AddDays(-daysDifference);
        }

        /// <summary>         
        /// Identifica y maneja las anomalías relacionadas con los precios en un grupo de registros.         
        /// </summary>         
        /// <param name="recordsGroupContainer">La lista de registros en el grupo.</param>
        public async Task AnomaliesPrice(List<RecordStoreModel> recordsGroupContainer)
        {
            List<RecordStoreModel> selectedRecords = new List<RecordStoreModel>();
            var sortedRecords = recordsGroupContainer.OrderBy(r => r.Record.Price).ToList();
            int q2Index = CalculateQ2Index(sortedRecords.Count);
            int q1Index = CalculateQ1Index(q2Index);
            int q3Index = CalculateQ3Index(q2Index, sortedRecords.Count);
            double? q1;
            double? q3;

            if (q2Index % 2 == 0 )
            {
                if (sortedRecords.Count % 2 != 0)
                {
                    q1 = (sortedRecords[q1Index].Record.Price);
                    q3 = (sortedRecords[q3Index].Record.Price);
                }
                else
                {
                    q1 = ((sortedRecords[q1Index].Record.Price) + (sortedRecords[q1Index+1].Record.Price))/2;
                    q3 = ((sortedRecords[q3Index].Record.Price) + (sortedRecords[q3Index-1].Record.Price))/2;
                }             
            }
            else
            {
                q1 = ((sortedRecords[q1Index].Record.Price) + (sortedRecords[q1Index + 1].Record.Price))/2;
                q3 = ((sortedRecords[q3Index].Record.Price) + (sortedRecords[q3Index - 1].Record.Price))/2;
            }

            double? ric = q3 - q1;

            double umbral = 1.5;

            double? lowerBound = q1 - umbral * ric;
            double? upperBound = q3 + umbral * ric;

            selectedRecords.AddRange(sortedRecords.Where(r =>  r.Record.Hide == false));

            foreach (var indice in selectedRecords)
            {
                if (indice.Record.Price < lowerBound || indice.Record.Price > upperBound)
                {
                    string coment;
                    if (indice.Record.Price < lowerBound)
                    {
                        coment = "El precio es muy bajo comparado a los demás.";
                    }
                    else
                    {
                        coment = "El precio es muy alto comparado a los demás.";
                    }
                    if (!_context.Anomalies.Any(a =>
                    a.NameGenerator == indice.Record.NameGenerator &&
                    a.RecordDate == indice.Record.RecordDate &&
                    a.Type == "Precio"))
                    {
                        Anomalie anomalie = new Anomalie
                        {
                            NameGenerator = indice.Record.NameGenerator,
                            RecordDate = indice.Record.RecordDate,
                            Type = "Precio",
                            Comment = coment,
                            State = 0
                        };
                        _context.Anomalies.Add(anomalie);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            sortedRecords.Clear();
            selectedRecords.Clear();
        }

        /// <summary>
        /// Calcula el índice para Q2 (mediana) en una lista ordenada de registros.
        /// </summary>
        /// <param name="recordCount">El número total de registros.</param>
        /// <returns>El índice para Q2.</returns>
        public int CalculateQ2Index(int recordCount)
        {
            if (recordCount % 2 == 0)
            {
                return recordCount / 2;
            }
            else
            {
                return ((recordCount - 1) / 2) + 1;
            }
        }

        /// <summary>
        /// Calcula el índice para Q1 (cuaril inferior) basado en el índice de Q2.
        /// </summary>
        /// <param name="q2">El índice para Q2 (mediana).</param>
        /// <returns>El índice para Q1.</returns>
        public int CalculateQ1Index(int q2)
        {
            if (q2 % 2 == 0)
            {
                return (int) ((q2 / 2) - 1);
            }
            else
            {
                return (int) (((q2-1) / 2) - 1);
            }
        }

        /// <summary>
        /// Calcula el índice para Q3 (cuaril superior) basado en el índice de Q2 y el número total de registros.
        /// </summary>
        /// <param name="q2">El índice para Q2 (mediana).</param>
        /// <param name="recordCount">El número total de registros.</param>
        /// <returns>El índice para Q3.</returns>
        public int CalculateQ3Index(int q2, int recordCount)
        {
            if (q2 % 2 == 0)
            {
                if ((recordCount % 2 == 0))
                {
                    return (int)(((q2 + 1) + (q2 / 2)) - 1);
                }
                else
                {
                    return (int)((q2 + (q2 / 2)) - 1);
                }
            }
            else
            {
                if ((recordCount % 2 == 0))
                {
                    return (int)((q2 + 1) + ((q2 + 1) / 2));
                }
                else
                {
                    return (int)((q2 + 1) + ((q2 - 1) / 2));
                }
            }
        }
    }
}

