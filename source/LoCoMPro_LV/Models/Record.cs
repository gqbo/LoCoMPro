using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Record
    {
        [Key]
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string NameRecord { get; set; }

        [Required]
        public DateTime RecordDate { get; set; }

        [StringLength(256, MinimumLength = 2)]
        public string Descripcion { get; set; }

        public float Price { get; set; }

        [StringLength(256, MinimumLength = 2)]
        public string NameGenerator { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string NameStore { get; set; }

        public GeneratorUser GeneratorUser { get; set; }

        public Store Store { get; set; }
    }
}