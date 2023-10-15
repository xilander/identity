using Microsoft.AspNetCore.Identity;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Data
{
    public static class SeguridadData
	{

        public static async Task InsertarUsuario(IServiceProvider services, ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!userManager.Users.Any())
            {
                var usuario = new Usuario
                {
                    Nombre = "Gang",
                    Apellido = "of Four",
                    UserName = "Admin",
                    Email = "xilander@gmail.com"
                };

                await userManager.CreateAsync(usuario, "Abcd1234+");
            }
        }

    }
}

