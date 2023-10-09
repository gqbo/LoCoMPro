using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Record
    {
        [Key]
        [Required(ErrorMessage = "El nombre del generador es obligatorio.")]
        [StringLength(256, MinimumLength = 2, ErrorMessage = "El nombre del generador tener entre 2 y 256 caracteres.")]
        [Display(Name = "Nombre de usuario")]
        public string NameGenerator { get; set; }

        [Display(Name = "Fecha de Registro")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha es obligatorio.")]
        public DateTime RecordDate { get; set; }

        [StringLength(512, MinimumLength = 2, ErrorMessage = "La descripción tener entre 2 y 512 caracteres.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [RegularExpression(@"^[0-9.]+$", ErrorMessage = "Solo se permiten números y puntos.")]
        public double Price { get; set; }

        [Display(Name = "Establecimiento")]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"^(?=.*[a-zA-ZáéíóúÁÉÍÓÚ])[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "El nombre del establecimiento debe contener al menos una letra (mayúscula o minúscula), además de números, espacios y caracteres especiales básicos, así como letras acentuadas.")]
        public string NameStore { get; set; }

        [Display(Name = "Provincia")]
        [StringLength(50, MinimumLength = 2)]
        public  string NameProvince { get; set; }

        [Display(Name = "Cantón")]
        [StringLength(50, MinimumLength = 2)]
        public  string NameCanton { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^(?=.*[a-zA-ZáéíóúÁÉÍÓÚ])[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "El nombre del producto debe contener al menos una letra (mayúscula o minúscula), además de números, espacios y caracteres especiales básicos, así como letras acentuadas.")]
        public string NameProduct { get; set; }

        public GeneratorUser GeneratorUser { get; set; }

        public Store Store { get; set; }

        public Product Product { get; set; }
    }
}