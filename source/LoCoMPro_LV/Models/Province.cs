using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoCoMPro_LV.Models
{
    public class Province
    {
        [Key]
        [Required] 
        [StringLength(50, MinimumLength = 2)]
        public required string NameProvince { get; set; }
        public ICollection<Canton> Cantons { get; set; }

    }
}
