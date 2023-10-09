using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Category
    {
        [Key]
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [RegularExpression(@"^(?=.*[a-zA-ZáéíóúÁÉÍÓÚ])[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "La categoría debe contener al menos una letra (mayúscula o minúscula), además de números, espacios y caracteres especiales básicos, así como letras acentuadas.")]
        [StringLength(50, MinimumLength = 3)]
        public string NameCategory { get; set; }

        [RegularExpression(@"^(?=.*[a-zA-ZáéíóúÁÉÍÓÚ])[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "La categoría debe contener al menos una letra (mayúscula o minúscula), además de números, espacios y caracteres especiales básicos, así como letras acentuadas.")]
        [StringLength(50, MinimumLength = 3)]
        public string NameTopCategory { get; set; }

        public Category TopCategory { get; set; }

        public ICollection<Category> Categories { get; set;}

        public ICollection<Associated> Associated { get; set;}
    }
}