using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class Category
    {
        [Key]
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string NameCategory { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string NameTopCategory { get; set; }

        public Category TopCategory { get; set; }

        public ICollection<Category> Categories { get; set;}
    }
}