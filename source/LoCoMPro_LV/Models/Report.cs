using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo con los reportes asociados a un registro dentro de la aplicacion.
    /// </summary>
    public class Report
    {
        [Key]
        [Required(ErrorMessage = "El nombre del generador es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del generador tener entre 2 y 256 caracteres.")]
        [Display(Name = "Nombre de usuario")]
        public string NameGenerator { get; set; }

        [Display(Name = "Fecha de Registro")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha es obligatorio.")]
        public DateTime RecordDate { get; set; }

        [Required(ErrorMessage = "La nombre de usuario es obligatoria.")]
        [StringLength(50, MinimumLength = 2)]
        public string NameReporter { get; set; }

        [Display(Name = "Fecha de Reporte")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha es obligatorio.")]
        public DateTime ReportDate { get; set; }

        [StringLength(500, MinimumLength = 2, ErrorMessage = "El comentario debe tener entre 2 y 512 caracteres.")]
        [Display(Name = "Comentario")]
        public string Comment { get; set; }
        public int State { get; set; }

        public Record Record { get; set; }

        public GeneratorUser GeneratorUser { get; set; }
    }
}