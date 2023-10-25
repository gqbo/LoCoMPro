using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoCoMPro_LV.Data;
using LoCoMPro_LV.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Configuration;
using LoCoMPro_LV.Utils;
using Microsoft.Data.SqlClient;

namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Página de detalles de producto, en donde se ven los registros relacionados a un mismo producto.
    /// </summary>
    public class DetailsModel : PageModel
    {
        /// <summary>
        /// Contexto de la base de datos de LoCoMPro.
        /// </summary>
        private readonly LoComproContext _context;

        private readonly IConfiguration Configuration;

        /// <summary>
        /// Constructor de la clase DetailsModel.
        /// </summary>
        /// <param name="context">Contexto de la base de datos de LoCoMPro.</param>
        public DetailsModel(LoComproContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        /// <summary>
        /// Lista de tipo "Record", que almacena los registros correspondientes al producto que se selecciono para ver en detalle.
        /// </summary>
        public PaginatedList<Record> Records { get; set; }
        /*public IList<Record> Record { get; set; } = default!;*/

        /// <summary>
        /// Nombre del usuario generador del registro seleccionado en la pantalla index, que se utiliza para buscar todos los registros relacionados.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string NameGenerator { get; set; }

        /// <summary>
        /// Fecha en la que se realizó el registro seleccionado en la pantalla index, que se utiliza para buscar todos los registros relacionados.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// Método utilizado cuando en la pantalla de resultados de la búsqueda se selecciona un producto para abrir el detalle. Este método
        /// utiliza el nombre del generador y la fecha de realización del registro más reciente del producto para realizar la consulta por todos
        /// de todos los registros con ese producto en esa tienda en específico.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            var FirstRecord = await _context.Records
                .FirstOrDefaultAsync(m => m.NameGenerator == NameGenerator && m.RecordDate == RecordDate);

            Console.WriteLine(NameGenerator);

            if (FirstRecord != null)
            {
                var allRecords = from m in _context.Records
                                 where m.NameProduct.Contains(FirstRecord.NameProduct) &&
                                       m.NameStore.Contains(FirstRecord.NameStore) &&
                                       m.NameProvince.Contains(FirstRecord.NameProvince) &&
                                       m.NameCanton.Contains(FirstRecord.NameCanton)
                                 orderby m.RecordDate descending
                                 select m;

                var totalCount = await allRecords.CountAsync(); // Contiene el número total de registros.

                var pageSize = Configuration.GetValue("PageSize", 10);
                Records = await PaginatedList<Record>.CreateAsync(
                    allRecords, pageIndex ?? 1, pageSize);
            }
            else
            {
                Records = new PaginatedList<Record>(new List<Record>(), 0, 1, 10);
            }

            return Page();
        }
    }
}
