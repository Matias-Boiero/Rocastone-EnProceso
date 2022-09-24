using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocastone.Models
{
    [Index(nameof(Nombre), IsUnique = true)]
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El nombre del {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        public string Nombre { get; set; }

        [Display(Name = "Breve descripción del producto")]
        [Required(ErrorMessage = "La descrpción es obligatoria")]
        public string ShortDescripcion { get; set; }

        [Display(Name = "Descripción del producto")]
        [Required(ErrorMessage = "La {0} es obligatoria")]
        public string DescripcionProducto { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El precio es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public double Precio { get; set; }

        [Display(Name = "Imagen")]
        public string? imgUrl { get; set; }


        //Foreign Key
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }

        public int TipoAplicacionId { get; set; }

        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion? TipoAplicacion { get; set; }

    }
}
