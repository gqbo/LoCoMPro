using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Store
    {
        [Key]
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string NameStore { get; set; }

        public  string NameProvince { get; set; }

        public  string NameCanton { get; set; }

        public Canton Canton { get; set; }

        public ICollection<Record> Record { get; set; }
    }
}
