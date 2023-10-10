using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con los usuarios de la aplicación web. Este modelo se relaciona con la tabla AspNetUsers de la base de datos.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El nombre debe contener solo letras (mayúsculas o minúsculas).")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [PersonalData]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El apellido debe contener solo letras (mayúsculas o minúsculas).")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string NameProvince { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string NameCanton { get; set; }

        public Canton Canton { get; set; }

        public ICollection<GeneratorUser> GeneratorUser { get; set; }
    }
}
