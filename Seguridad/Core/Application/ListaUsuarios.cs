using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Seguridad.Core.Data;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Application
{
    public class ListaUsuarios
    {
        public class ListaUsuariosCommand : IRequest<List<UsuarioDto>> { }

        public class ListaUsuariosHandler : IRequestHandler<ListaUsuariosCommand, List<UsuarioDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<Usuario> _userManager;
            private bool activo;
            private DateTimeOffset? lockEndDate;

            public ListaUsuariosHandler(ApplicationDbContext context, UserManager<Usuario> userManager)
            {
                _context = context;
                _userManager = userManager;
            }

            async Task<List<UsuarioDto>> IRequestHandler<ListaUsuariosCommand , List<UsuarioDto>>.Handle(ListaUsuariosCommand request, CancellationToken cancellationToken)
            {
                var usuarios = await _context.Users.ToListAsync();
                List<UsuarioDto> usuariosDTO = new List<UsuarioDto>();

                foreach (var usuario in usuarios)
                {
                    var roles = await _userManager.GetRolesAsync(usuario);
                    lockEndDate = await _userManager.GetLockoutEndDateAsync(usuario);

                    if( lockEndDate == null)
                    {
                        activo = true;
                    }
                    else
                    {
                        activo = false;
                    };
                    
                    usuariosDTO.Add(new UsuarioDto
                    {
                        ID = usuario.Id,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        UserName = usuario.UserName,
                        Rol = roles[0],
                        Activo = activo
                    });
                }

                return usuariosDTO;
                
            }
        }
    }
}

