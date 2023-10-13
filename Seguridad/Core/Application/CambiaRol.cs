using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Application
{
    public class CambiaRol
	{
		public class CambiaRolCommand : IRequest<RespuestaModel>
		{
			public string? id { get; set; }
            public string? nuevoRol { get; set; }
		}

        public class CambiaRolHandler : IRequestHandler<CambiaRolCommand, RespuestaModel>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private string mensaje = "";

            public CambiaRolHandler(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<RespuestaModel> Handle(CambiaRolCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByIdAsync(request.id!);
                if(usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }

                try
                {
                    var rol = await _userManager.GetRolesAsync(usuario);
                    var rolAnterior = await _roleManager.FindByNameAsync(rol[0]);

                    if (request.nuevoRol == rolAnterior!.Name)
                    {
                        mensaje = "Sin cambios";
                    }
                    else
                    {
                        _ = _userManager.RemoveFromRoleAsync(usuario, rolAnterior.Name!);
                        await _userManager.AddToRoleAsync(usuario, request.nuevoRol!);

                        mensaje = "Se modificó el rol de " + usuario.Nombre + "a " + request.nuevoRol;
                    }

                    var respuesta = new RespuestaModel
                    {
                        Ok = true,
                        Status = 202,
                        StatusText = "Accepted",
                        Mensaje = mensaje
                    };
                    return respuesta;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }
    }
}

