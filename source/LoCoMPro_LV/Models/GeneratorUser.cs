using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    public class GeneratorUser
    {
        [Key]
        [Required(ErrorMessage = "La nombre de usuario es obligatoria.")]
        [StringLength(256, MinimumLength = 2)]
        public string UserName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Record> Record { get; set;}
    }
}