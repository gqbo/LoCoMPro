using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con la asociación entre los productos y la categorías de la aplicación web. Este modelo se relaciona con la tabla Associated de la base de datos.
    /// </summary>
    public class Associated
    {
        [Key]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^(?=.*[a-zA-ZáéíóúÁÉÍÓÚ])[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "El nombre del producto debe contener al menos una letra (mayúscula o minúscula), además de números, espacios y caracteres especiales básicos, así como letras acentuadas.")]
        public string NameProduct { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "La categoría debe contener solo letras (mayúsculas o minúsculas).")]
        [StringLength(50, MinimumLength = 3)]
        public string NameCategory { get; set; }

        public Product Product { get; set; }

        public Category Category { get; set; }
    }
}
