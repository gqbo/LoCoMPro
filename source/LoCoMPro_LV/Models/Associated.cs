using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Associated
    {
        [Key]
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string NameProduct { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string NameCategory { get; set; }

        public Product Product { get; set; }
        
        public Category Category { get; set; }
    }
}
