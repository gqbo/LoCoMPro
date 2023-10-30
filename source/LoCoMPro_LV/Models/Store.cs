using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con las tiendas de la aplicación web. Representa un establecimiento o tienda. Este modelo se relaciona con la tabla Cantons de la base de datos
    /// </summary>
    public class Store
    {
        [Key]
        [Required(ErrorMessage = "El nombre del establecimiento es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre del establecimiento debe tener entre 2 y 100 caracteres.")]
        [RegularExpression(@"^(?=.*[a-zA-ZáéíóúÁÉÍÓÚ])[\w\s,./\-()%:#áéíóúÁÉÍÓÚ]+$",
            ErrorMessage = "El nombre del establecimiento debe contener al menos una letra (mayúscula o minúscula), además de números, espacios y caracteres especiales básicos, así como letras acentuadas.")]
        public string NameStore { get; set; }

        [Required(ErrorMessage = "El grado de latitud es necesario")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "El grado de longitud es necesario")]
        public double Longitude { get; set; }
        
        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "El nombre de la provincia es obligatorio.")]
        public string NameProvince { get; set; }

        [Display(Name = "Cantón")]
        [Required(ErrorMessage = "El nombre del cantón es obligatorio.")]
        public string NameCanton { get; set; }

        public Canton Canton { get; set; }

        public ICollection<Record> Record { get; set; }
    }
}
