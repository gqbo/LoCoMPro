using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con los registros de la aplicación web. Este modelo se relaciona con la tabla Records de la base de datos.
    /// </summary>
    public class Evaluate
    {
        [Key]
        [Required(ErrorMessage = "El nombre del evaluador es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del generador tener entre 2 y 256 caracteres.")]
        [Display(Name = "Nombre de usuario")]
        public string NameEvaluator { get; set; }

        [Required(ErrorMessage = "El nombre del generador es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del generador tener entre 2 y 256 caracteres.")]
        [Display(Name = "Nombre de usuario")]
        public string NameGenerator { get; set; }

        [Display(Name = "Fecha de Registro")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha es obligatorio.")]
        public DateTime RecordDate { get; set; }

        [Display(Name = "Valoración")]
        public int StarsCount { get; set; }
        public Record Record { get; set; }
        public GeneratorUser GeneratorUser { get; set; }
    }
}