using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Rocastone.Models
{
    [Index(nameof(Nombre), IsUnique = true)]
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nombre Aplicación")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        public string Nombre { get; set; }
    }
}
