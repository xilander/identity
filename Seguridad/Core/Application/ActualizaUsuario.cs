using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Seguridad.Core.Data;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Application
{
    public class ActualizaUsuario
	{
		public class ActualizaUsuarioCommand : IRequest<UsuarioDto>
		{
			public string? Nombre { get; set; }
			public string? Apellido { get; set; }
			public string? Email { get; set; }
			public string? UserName { get; set; }
			public string? Password { get; set; }
			public string? Rol { get; set; }
			public Boolean Activo { get; set; }
		}

        public class ActualizaUsuarioHandler : IRequestHandler<ActualizaUsuarioCommand, UsuarioDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;

            public ActualizaUsuarioHandler(ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
            }
            
            public async Task<UsuarioDto> Handle(ActualizaUsuarioCommand request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (!existe)
                {
                    throw new Exception("El usuario no existe");
                }

                var usuario = await _userManager.FindByEmailAsync(request.Email!);

                var usuarioModificado = new Usuario
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                };

                IdentityResult actualizaUsuario = await _userManager.UpdateAsync(usuarioModificado);
                if (actualizaUsuario.Succeeded)
                {
                    var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario!);
                    return usuarioDTO;
                }

                throw new Exception("No se pudo actualizar el usuario");
            }
            
        }
    }
}

