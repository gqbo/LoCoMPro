using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con las categorías de la aplicación web. Este modelo se relaciona con la tabla Category de la base de datos.
    /// </summary>
    public class Category
    {
        [Key]
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "La categoría debe contener solo letras (mayúsculas o minúsculas).")]
        [StringLength(50, MinimumLength = 3)]
        public string NameCategory { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "La categoría debe contener solo letras (mayúsculas o minúsculas).")]
        [StringLength(50, MinimumLength = 3)]
        public string NameTopCategory { get; set; }

        public Category TopCategory { get; set; }

        public ICollection<Category> Categories { get; set;}

        public ICollection<Associated> Associated { get; set;}
    }
}