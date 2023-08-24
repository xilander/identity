using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;
using Seguridad.Core.JwtLogic;

namespace Seguridad.Core.Application
{
    public class UsuarioActual
    {

        public class UsuarioActualCommand: IRequest<UsuarioDto> { }

        public class UsuarioActualHandler : IRequestHandler<UsuarioActualCommand, UsuarioDto>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly ISesionUsuario _sesionUsuario;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IMapper _mapper;

            public UsuarioActualHandler(UserManager<Usuario> userManager, ISesionUsuario sesionUsuario, IJwtGenerator jwtGenerator, IMapper mapper )
            {
                _userManager = userManager;
                _sesionUsuario = sesionUsuario;
                _jwtGenerator = jwtGenerator;
                _mapper = mapper;
            }

            public async Task<UsuarioDto> Handle(UsuarioActualCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_sesionUsuario.GetSesionUsuario());

                if (usuario != null) {
                    var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
                    usuarioDTO.Token = _jwtGenerator.CrearToken(usuario);
                    return usuarioDTO;
                }

                throw new Exception("Usuario no encontrado");
            }
        }

    }
}
