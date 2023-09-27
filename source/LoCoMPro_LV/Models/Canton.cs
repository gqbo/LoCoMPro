using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoCoMPro_LV.Models
{
    public class Canton
    {
        [Key]
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string NameCanton { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string NameProvince { get; set; }
        public Province Province { get; set; }
        public ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}
