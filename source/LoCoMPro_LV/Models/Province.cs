using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con las provincias de la aplicación web. Este modelo se relaciona con la tabla Province de la base de datos.
    /// </summary>
    public class Province
    {
        [Key]
        [Required(ErrorMessage = "El nombre de la provincia es obligatorio.")]
        public required string NameProvince { get; set; }
        public ICollection<Canton> Cantons { get; set; }

    }
}
