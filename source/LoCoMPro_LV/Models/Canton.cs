using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con los cantones de la aplicación web. Este modelo se relaciona con la tabla Cantons de la base de datos.
    /// </summary>
    public class Canton
    {
        [Key]
        [Required(ErrorMessage = "El nombre del cantón es obligatorio.")]
        public string NameCanton { get; set; }

        [Required(ErrorMessage = "El nombre de la provincia es obligatorio.")]
        public string NameProvince { get; set; }
        public Province Province { get; set; }
        public ICollection<ApplicationUser> ApplicationUser { get; set; }
        public ICollection<Store> Store { get; set; }
    }
}
