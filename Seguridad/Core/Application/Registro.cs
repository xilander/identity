﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Seguridad.Core.Data;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Application
{
    public class Registro
    {
        public class RegistroUsuarioCommand : IRequest<UsuarioDto>
        {
            public string? Nombre { get; set; }
            public string? Apellido { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
        }
        public class RegistroUsuarioValidator : AbstractValidator<RegistroUsuarioCommand>
        {
            public RegistroUsuarioValidator()
            {
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido");
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty().WithMessage("No olvide la contraseña");
            }
        }
        
        public class RegistroUsuarioHandler : IRequestHandler<RegistroUsuarioCommand, UsuarioDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;

            public RegistroUsuarioHandler(ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper)
            {
                _context = context;
                _userManager = userManager;
                _mapper =  mapper;
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
                };
                
                var resultado = await _userManager.CreateAsync(usuario, request.Password);
                   
                if (resultado.Succeeded) {
                    var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
                    return usuarioDTO;
                }
                throw new Exception("No se pudo crear el usuario");
            }
        }
     }
}
