using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Seguridad.Core.Data;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;
using Seguridad.Core.JwtLogic;

namespace Seguridad.Core.Application
{
    public class Registro
    {
        public class RegistroUsuarioCommand : IRequest<UsuarioDto>
        {
            public string? Nombre { get; set; }
            public string? Apellido { get; set; }
            public string? Email { get; set; }
            public string? UserName { get; set; }
            public string? Rol { get; set; }
            public string? Password { get; set; }
        }

       
        public class RegistroUsuarioHandler : IRequestHandler<RegistroUsuarioCommand, UsuarioDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;
            private readonly IJwtGenerator _jwtGenerator;

            public RegistroUsuarioHandler(ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper, IJwtGenerator jwtGenerator)
            {
                _context = context;
                _userManager = userManager;
                _mapper =  mapper;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<UsuarioDto> Handle(RegistroUsuarioCommand request, CancellationToken cancellationToken)
            {

                var existe = await _context.Users.Where( x => x.Email == request.Email).AnyAsync();
                if (existe) {
                    throw new Exception("El correo del usuario ya existe");
                }


                var usuario = new Usuario
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Email = request.Email,
                    UserName = request.UserName,
                };
                

                IdentityResult crearUsuario = await _userManager.CreateAsync(usuario, request.Password!);
                if (crearUsuario.Succeeded) {
                    
                    IdentityResult asignarRol = await _userManager.AddToRoleAsync(usuario, request.Rol!);
                    if (asignarRol.Succeeded)
                    {
                        var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
                        usuarioDTO.Token = _jwtGenerator.CrearTokenAsync(usuario); 
                        return usuarioDTO;
                    }
                    else
                    {
                        await _userManager.DeleteAsync(usuario);
                        throw new Exception("No se generó el rol");
                    }
                }
                throw new Exception("No se pudo crear el usuario");
            }
        }
     }
}
