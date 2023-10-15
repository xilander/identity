using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Application
{
    public class ObtenerUsuarioPorId
	{
		public class ObtenerUsuarioPorIdCommand : IRequest<UsuarioDto>
        {
			public string? Id { get; set; }
		}

        public class ObtenerUsuarioPorIdHandler : IRequestHandler<ObtenerUsuarioPorIdCommand, UsuarioDto>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;
            private bool activo;

            public ObtenerUsuarioPorIdHandler(UserManager<Usuario> userManager,  IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<UsuarioDto> Handle(ObtenerUsuarioPorIdCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByIdAsync(request.Id!) ?? throw new Exception("El usuario no existe");

                var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(usuario);
                if (lockoutEndDate != null && lockoutEndDate > DateTimeOffset.Now)
                {
                    activo = false;
                }
                else
                {
                    activo = true;
                }

                var roles = await _userManager.GetRolesAsync(usuario);
                var claims = new List<Claim>();

                foreach (var rol in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }

                var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
                usuarioDTO.Correo = usuario.Email;
                usuarioDTO.Activo = activo;
                usuarioDTO.Rol = roles[0];
                return usuarioDTO;
            }
        }

    }
        
}

