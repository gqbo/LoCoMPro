using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Record
    {
        [Key]
        [Required]
        [StringLength(256, MinimumLength = 2)]
        public string NameGenerator { get; set; }

        [Required]
        public DateTime RecordDate { get; set; }

        [StringLength(256, MinimumLength = 2)]
        public string Description { get; set; }

        public float Price { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string NameStore { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public  string NameProvince { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public  string NameCanton { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string NameProduct { get; set; }

        public GeneratorUser GeneratorUser { get; set; }

        public Store Store { get; set; }

        public Product Product { get; set; }
    }
}