using Microsoft.AspNetCore.Identity;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Data
{
    public class SiembraUsuario
	{
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

            string[] rolesNames = { "Admin", "Supervisor", "Usuario", "Abogado", "Gestor" };
            IdentityResult roleResult;

            foreach (var roleName in rolesNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Crea el usuario admin si la tabla de usuarios esta vacía
            if ( !userManager.Users.Any())
            {
                var adminUser = new Usuario
                {
                    UserName = "admin",
                    Email = "admin@test.com",
                    Nombre = "Gang",
                    Apellido = "of Four"
                };

                IdentityResult createAdminUser = await userManager.CreateAsync(adminUser, "Abcd1234+");
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("Se creo usuario Admin");
                }
            }
        }
    }
}

