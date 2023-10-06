using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Associated
    {
        [Key]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s,./\-()%:#]+$", ErrorMessage = "La categoría debe contener solo letras (mayúsculas o minúsculas), espacios y caracteres especiales básicos.")]
        public string NameProduct { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "La categoría debe contener solo letras (mayúsculas o minúsculas).")]
        [StringLength(50, MinimumLength = 3)]
        public string NameCategory { get; set; }

        public Product Product { get; set; }
        
        public Category Category { get; set; }
    }
}
