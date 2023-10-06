using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Record
    {
        [Key]
        [Required]
        
        [StringLength(256, MinimumLength = 2)]
        [Display(Name = "Nombre de usuario")]
        public string NameGenerator { get; set; }

        [Display(Name = "Fecha de Registro")]
        [Required]
        public DateTime RecordDate { get; set; }

        [StringLength(256, MinimumLength = 2)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Precio")]
        public float Price { get; set; }

        [Display(Name = "Establecimiento")]
        [StringLength(100, MinimumLength = 2)]
        public string NameStore { get; set; }

        [Display(Name = "Provincia")]
        [StringLength(50, MinimumLength = 2)]
        public  string NameProvince { get; set; }

        [Display(Name = "Cantón")]
        [StringLength(50, MinimumLength = 2)]
        public  string NameCanton { get; set; }

        [Display(Name = "Producto")]
        [StringLength(100, MinimumLength = 2)]
        public string NameProduct { get; set; }

        public GeneratorUser GeneratorUser { get; set; }

        public Store Store { get; set; }

        public Product Product { get; set; }
    }
}