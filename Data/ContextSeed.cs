using Microsoft.AspNetCore.Identity;
using Rocastone.Models;

namespace Rocastone.Data
{
    public class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Basico.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin@yopmail.com",
                Email = "superadmin@yopmail.com",
                Nombre = "Matias",
                Apellido = "Boiero",
                Provincia = "Buenos Aires",
                Ciudad = "San MIguel",
                Direccion = "Muñoz 1063",
                Telefono = "1146736316",
                ProfilePicture = null,
                CodigoPostal = 1663,
                EmailConfirmed = true,
                //PhoneNumberConfirmed = true

            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                //Le asigno todos los roles al superadministrador
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin123*");
                    await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Basico.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enum.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enum.Roles.SuperAdmin.ToString());
                }
            }
        }
    }
}
