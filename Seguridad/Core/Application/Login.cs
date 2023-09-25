
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
         
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly SignInManager<Usuario> _signInManager;
            
            public LoginUsuarioHandler(UserManager<Usuario> userManager, IMapper mapper, IJwtGenerator jwtGenerator, SignInManager<Usuario> signInManager)
            {
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

                var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(usuario);
                if( lockoutEndDate != null && lockoutEndDate > DateTimeOffset.Now)
                {
                    throw new Exception(" El usuario está dado de baja");
                } 

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password!, false);
                if (resultado.Succeeded)
                {
                    var roles =  await _userManager.GetRolesAsync(usuario);
                    var claims = new List<Claim>();

                    foreach (var rol in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rol));
                    }
                        
                    var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
                    var tokenCreado = _jwtGenerator.CrearTokenAsync(usuario);
                    usuarioDTO.Token = tokenCreado;
                    usuarioDTO.Rol = roles[0];
                    return usuarioDTO;
                }

                throw new Exception("Credenciales incorrectas");

            }
        }
    }
}
