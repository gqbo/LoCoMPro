using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Product
    {
        [Key]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^(?=.*[a-zA-ZáéíóúÁÉÍÓÚ])[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "El nombre del producto debe contener al menos una letra (mayúscula o minúscula), además de números, espacios y caracteres especiales básicos, así como letras acentuadas.")]
        public string NameProduct { get; set; }

        public ICollection<Record> Record { get; set; }

        public ICollection<Associated> Associated { get; set;}
    }
}
