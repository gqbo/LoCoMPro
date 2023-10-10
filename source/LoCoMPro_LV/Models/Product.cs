using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Models
{
    /// <summary>
    /// Modelo relacionado con los productos de la aplicación web. Este modelo se relaciona con la tabla Product de la base de datos.
    /// </summary>
    public class Product
    {
        [Key]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s,./\-()%:#]+$", ErrorMessage = "El nombre del producto debe contener solo letras (mayúsculas o minúsculas), espacios y caracteres especiales básicos.")]
        public string NameProduct { get; set; }

        public ICollection<Record> Record { get; set; }

        public ICollection<Associated> Associated { get; set;}
    }
}
