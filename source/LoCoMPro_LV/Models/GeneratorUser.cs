using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con los usuarios generadores de la aplicación web. Este modelo se relaciona con la tabla GeneratorUser de la base de datos.
    /// </summary>
    public class GeneratorUser
    {
        [Key]
        [Required(ErrorMessage = "La nombre de usuario es obligatoria.")]
        [StringLength(256, MinimumLength = 2)]
        public string UserName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Record> Record { get; set;}
    }
}