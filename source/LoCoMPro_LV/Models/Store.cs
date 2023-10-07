using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Store
    {
        [Key]
        [Required(ErrorMessage = "El nombre del establecimiento es obligatorio.")]
        [StringLength(100, MinimumLength = 2)]
        public string NameStore { get; set; }

        [Required(ErrorMessage = "El nombre de la provincia es obligatorio.")]
        [StringLength(50, MinimumLength = 2)]
        public  string NameProvince { get; set; }

        [Required(ErrorMessage = "El nombre del cantón es obligatorio.")]
        [StringLength(50, MinimumLength = 2)]
        public  string NameCanton { get; set; }

        public Canton Canton { get; set; }

        public ICollection<Record> Record { get; set; }
    }
}
