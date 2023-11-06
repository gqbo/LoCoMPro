using Microsoft.EntityFrameworkCore;

namespace LoCoMPro_LV.Utils
{
    /// <summary>
    /// Clase genérica para la creación de listas paginadas a partir de una lista de elementos y metadatos de paginación.
    /// </summary>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// Obtiene el índice de la página actual.
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Obtiene el número total de páginas disponibles.
        /// </summary>
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        /// <summary>
        /// Obtiene un valor que indica si hay una página anterior a la actual.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Obtiene un valor que indica si hay una página siguiente a la actual.
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Crea una instancia de PaginatedList de manera asincrónica a partir de una consulta IQueryable.
        /// </summary>
        /// <param name="source">Consulta IQueryable que contiene los elementos a paginar.</param>
        /// <param name="pageIndex">Índice de la página actual.</param>
        /// <param name="pageSize">Tamaño de página.</param>
        /// <returns>Una instancia de PaginatedList que representa la página actual.</returns>
        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip(
                (pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
