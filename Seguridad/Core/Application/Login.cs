using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Seguridad.Core.Data;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;
using Seguridad.Core.JwtLogic;

namespace Seguridad.Core.Application
{
    public class Login
    {
        public class  LoginUsuarioCommand : IRequest<UsuarioDto>
        {
            public string? Email { get; set; }
            public string? Password { get; set; }  
        }

        public class LoginUsuarioValidator : AbstractValidator<LoginUsuarioCommand>
        {
            public LoginUsuarioValidator() {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class LoginUsuarioHandler : IRequestHandler<LoginUsuarioCommand, UsuarioDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly SignInManager<Usuario> _signInManager;

            public LoginUsuarioHandler( ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper, IJwtGenerator jwtGenerator, SignInManager<Usuario> signInManager)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
            }

            public async Task<UsuarioDto> Handle(LoginUsuarioCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email!);
                if (usuario == null)
                {
                    throw new Exception("El usuario no existe");
                }
                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password!, false);
                if (resultado.Succeeded)
                {
                    var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
                    usuarioDTO.Token = _jwtGenerator.CrearToken(usuario);
                    return usuarioDTO;
                }
                throw new Exception("Credenciales Incorrectas");
            }
        }
    }
}
