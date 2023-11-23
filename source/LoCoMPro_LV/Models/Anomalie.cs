using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo con las anomalias asociadas a un registro dentro de la aplicacion.
    /// </summary>
    public class Anomalie
    {
        [Key]
        [Required(ErrorMessage = "El nombre del generador es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del generador tener entre 2 y 256 caracteres.")]
        [Display(Name = "Nombre de usuario")]
        public string NameGenerator { get; set; }

        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha es obligatorio.")]
        public DateTime RecordDate { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "El tipo es obligatorio.")]
        [StringLength(10, MinimumLength = 2)]
        public string Type { get; set; }

        [Display(Name = "Comentario")]
        [StringLength(256, MinimumLength = 2, ErrorMessage = "El comentario debe tener entre 2 y 256 caracteres.")]
        public string Comment { get; set; }
        public int State { get; set; }

        public Record Record { get; set; }
    }
}