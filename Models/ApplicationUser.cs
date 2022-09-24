using Microsoft.AspNetCore.Identity;

namespace Rocastone.Models
{
    public class ApplicationUser : IdentityUser
    {


        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public byte[]? ProfilePicture { get; set; }

        public string? Provincia { get; set; }
        public string? Ciudad { get; set; }

        public string? Direccion { get; set; }
        public int? CodigoPostal { get; set; }

        public string? Telefono { get; set; }
    }
}
