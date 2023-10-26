using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con los registros de la aplicación web. Este modelo se relaciona con la tabla Records de la base de datos.
    /// </summary>
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
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "El precio debe ser un número entero mayor que 0.")]
        public double? Price { get; set; }

        [Display(Name = "Establecimiento")]
        [Required(ErrorMessage = "El nombre del establecimiento es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre del establecimiento debe tener entre 2 y 100 caracteres.")]
        [RegularExpression(@"^[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "El nombre del establecimiento debe contener al menos una letra (mayúscula o minúscula).")]
        public string NameStore { get; set; }

        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "Debe seleccionar una provincia.")]
        public string NameProvince { get; set; }

        [Display(Name = "Cantón")]
        [Required(ErrorMessage = "Debe seleccionar un cantón.")]
        public string NameCanton { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del producto debe tener entre 2 y 50 caracteres.")]
        [RegularExpression(@"^[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "El nombre del producto no es valido.")]
        public string NameProduct { get; set; }

        public GeneratorUser GeneratorUser { get; set; }

        public Store Store { get; set; }

        public Product Product { get; set; }
    }
}