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
            public string? Id { get; set; }
            public string? Nombre { get; set; }
			public string? Apellido { get; set; }
			public string? Email { get; set; }
			public string? UserName { get; set; }
			public string? Password { get; set; }
			public string? Rol { get; set; }
			public Boolean Activo { get; set; }
            public string? UnidadNegocio { get; set; }
            public string? Cargo { get; set; }
            public string? Banco { get; set; }
            public string? CuentaBanco { get; set; }
            public string? Clabe { get; set; }
        }

        public class ActualizaUsuarioHandler : IRequestHandler<ActualizaUsuarioCommand, UsuarioDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IMapper _mapper;

            public ActualizaUsuarioHandler(ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
            {
                _context = context;
                _userManager = userManager;
                _roleManager = roleManager;
                _mapper = mapper;
            }
            
            public async Task<UsuarioDto> Handle(ActualizaUsuarioCommand request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Id == request.Id).AnyAsync();
                if (!existe)
                {
                    throw new Exception("El usuario no existe");
                }
                var usuario = await _userManager.FindByIdAsync(request.Id!);

                bool emailsincambio = usuario?.Email == request.Email;
                if (!emailsincambio)
                {
                    var existeCorreo = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                    if (existeCorreo)
                    {
                        throw new Exception("El nuevo correo ya existe");
                    }
                }

                bool userSincambio = usuario?.UserName == request.UserName;
                if (!userSincambio)
                {
                    var existeuser = await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync();
                    if (existeuser)
                    {
                        throw new Exception("El Nombre de Usuario ya existe");
                    }
                }
                usuario!.Nombre = request.Nombre;
                usuario!.Apellido = request.Apellido;
                usuario!.Email = request.Email;
                usuario!.UserName = request.UserName;
                usuario!.UnidadNegocio = request.UnidadNegocio;
                usuario!.Cargo = request.Cargo;
                usuario!.Banco = request.Banco;
                usuario!.CuentaBanco = request.CuentaBanco;
                usuario!.Clabe = request.Clabe;

                IdentityResult actualizaUsuario = await _userManager.UpdateAsync(usuario);
                if (actualizaUsuario.Succeeded)
                {
                    var rol = await _userManager.GetRolesAsync(usuario);
                    var rolAnterior = await _roleManager.FindByNameAsync(rol[0]);
                    if (request.Rol != rolAnterior!.Name)
                    {
                        await _userManager.RemoveFromRoleAsync(usuario, rolAnterior.Name!);
                        await _userManager.AddToRoleAsync(usuario, request.Rol!);
                    }
                    
                    var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
                    return usuarioDTO;
                }

                throw new Exception("No se pudo actualizar el usuario");
            }            
        }
    }
}

