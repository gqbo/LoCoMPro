using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Store
    {
        [Key]
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string NameStore { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public  string NameProvince { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public  string NameCanton { get; set; }

        public Canton Canton { get; set; }

        public ICollection<Record> Record { get; set; }
    }
}
