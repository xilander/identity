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
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync(cancellationToken: cancellationToken);
                if (!existe)
                {
                    throw new Exception("El usuario no existe");
                }

                try
                {
                    var usuario = await _userManager.FindByEmailAsync(request.Email!);
                    var respuesta = new RespuestaModel
                    {
                        Ok = true,
                        Status = 202,
                        StatusText = "Accepted"
                    };

                    if (request.Activo)
                    {
                        var activaUsuario = await _userManager.SetLockoutEndDateAsync(usuario!, null);
                        respuesta.Mensaje = "El usuario ha sido reactivado";
                    }
                    else
                    {
                        //var desactivaUsuario = await _userManager.SetLockoutEndDateAsync(usuario!, DateTimeOffset.MaxValue);
                        var desactivaUsuario = await _context.Database.ExecuteSqlAsync($@"
                            UPDATE ""AspNetUsers"" SET ""LockoutEnd"" = '3020-01-31 00:00:00' WHERE ""Email"" =  {request.Email}
                        ");
                        
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

