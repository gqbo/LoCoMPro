using LoCoMPro_LV.Models;
using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Pages.Records
{
    /// <summary>
    /// Clase que representa un modelo que combina información de un registro (Record) y una tienda (Store),
    /// junto con el promedio de calificación de ese registro en esa tienda.
    /// </summary>
    public class RecordStoreModel
    {
        public Record Record { get; set; }
        public Store Store { get; set; }
        public int AverageRating { get; set; }
        [Display(Name = "Distancia")]
        public double Distance { get; set; }
    }
}