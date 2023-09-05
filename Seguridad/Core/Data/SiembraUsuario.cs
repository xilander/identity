using Microsoft.AspNetCore.Identity;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Data
{
    public class SiembraUsuario
	{
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

            //Create the Admin User
            var adminUser = new Usuario
            {
                UserName = "Admin",
                Email = "xilander@gmail.com",
                Nombre = "Gang",
                Apellido = "of Four"
            };

            var userExist = await userManager.FindByEmailAsync(adminUser.Email);
            if (userExist == null)
            {
                IdentityResult createAdminUser = await userManager.CreateAsync(adminUser, "Abcd1234+");
                if (createAdminUser.Succeeded)
                {
                    Console.WriteLine("se creo");
                }
            }
        }
    }
}

