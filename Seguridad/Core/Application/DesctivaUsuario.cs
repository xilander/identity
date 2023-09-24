using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Seguridad.Core.Data;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Application
{
	public class DesctivaUsuario
	{
		public class DesctivaUsuarioCommand : IRequest<RespuestaModel>
		{
            public string? Email { get; set; }
            public bool Activo { get; set; }
        }

		public class DesactivaUsuarioHandler : IRequestHandler<DesctivaUsuarioCommand, RespuestaModel>
		{
            private readonly ApplicationDbContext _context;
            private readonly UserManager<Usuario> _userManager;

            public DesactivaUsuarioHandler(ApplicationDbContext context, UserManager<Usuario> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            public async Task<RespuestaModel> Handle(DesctivaUsuarioCommand request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (!existe)
                {
                    throw new Exception("El usuario no existe");
                }

                try
                {
                    var usuario = await _userManager.FindByEmailAsync(request.Email!);
                    var respuesta = new RespuestaModel();
                    respuesta.Ok = true;
                    respuesta.Status = 202;
                    respuesta.StatusText = "Accepted";

                    if (request.Activo)
                    {
                        var activaUsuario = await _userManager.SetLockoutEndDateAsync(usuario!, null);
                        respuesta.Mensaje = "El usuario ha sido reactivado";
                    }
                    else
                    {
                        var desactivaUsuario = await _userManager.SetLockoutEndDateAsync(usuario!, DateTimeOffset.MaxValue);
                        respuesta.Mensaje = "El usuario ha sido desactivado";
                    }
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

