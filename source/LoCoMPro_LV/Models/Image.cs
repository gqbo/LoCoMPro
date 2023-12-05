using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Image
    {
        [Key]
        [Required(ErrorMessage = "El nombre del generador es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del generador tener entre 2 y 50 caracteres.")]
        [Display(Name = "Nombre de usuario")]
        public string NameGenerator { get; set; }

        [Display(Name = "Fecha de Registro")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha es obligatorio.")]
        public DateTime RecordDate { get; set; }

        [Required(ErrorMessage = "El nombre de la imagen es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre de la imagen debe tener tener entre 2 y 50 caracteres.")]
        public string NameImage { get; set; }

        public byte[] DataImage { get; set; }
        
        public Record Record { get; set; }
    }
}
