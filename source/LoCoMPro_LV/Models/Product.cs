using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Product
    {
        [Key]
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string NameProduct { get; set; }

        public ICollection<Record> Record { get; set; }

        public ICollection<Associated> Associated { get; set;}
    }
}
