using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Rocastone.Models
{
    [Index(nameof(NombreCategoria), IsUnique = true)]
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")] //data Notation.. Required es como un Not Null
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]

        public string? NombreCategoria { get; set; }


        [Display(Name = "Nro. de orden")]
        [Required(ErrorMessage = "En número de orden es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El orden debe ser mayor a cero")]
        public int MostrarOrden { get; set; }
    }
}
