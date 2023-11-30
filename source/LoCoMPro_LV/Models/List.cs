using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class List
    {
        [Key]
        [Required(ErrorMessage = "El nombre de la lista es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del establecimiento debe tener entre 2 y 50 caracteres.")]
        [RegularExpression(@"^(?=.*[a-zA-ZáéíóúÁÉÍÓÚ])[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "El nombre del establecimiento debe contener al menos una letra (mayúscula o minúscula), además de números, espacios y caracteres especiales básicos, así como letras acentuadas.")]
        public string NameList { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 2)]
        public string UserName { get; set; }

        public GeneratorUser User { get; set; }
        public ICollection<Listed> Listed { get; set; }
    }
}
