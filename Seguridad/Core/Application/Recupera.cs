using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Seguridad.Core.Data;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;
using Seguridad.Core.JwtLogic;
using Seguridad.Core.Services;
using static Seguridad.Core.Entities.EmailConfiguration;

namespace Seguridad.Core.Application
{
	public class Recupera
	{
		public class RecuperaPasswordCommand : IRequest<UsuarioDto>
		{
			public string? Id {get; set;}
			public string? Password { get; set; }
			public string? Token { get; set; }
		}

		
        public class RecuperaPasswordHandler : IRequestHandler<RecuperaPasswordCommand, UsuarioDto>
		{
			private readonly ApplicationDbContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;

			public RecuperaPasswordHandler(ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper)
            {
				_context = context;
                _userManager = userManager;
                _mapper = mapper;
            }

			public async Task<UsuarioDto> Handle(RecuperaPasswordCommand request, CancellationToken cancellationToken)
			{
				var existe = await _context.Users.Where(x => x.Id == request.Id).AnyAsync();
                
                if (!existe)
				{
                    throw new Exception("Usuario no válido");
                }

				try
				{
                    var usuario = await _userManager.FindByIdAsync(request.Id!);
                    //var token = request.Token;
                    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario!);
                    var result = await _userManager.ResetPasswordAsync(usuario!, token!, request.Password!);
					if (result.Succeeded)
					{
						var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario!);
						//usuarioDTO.Token = token;
						return usuarioDTO;
					}
					else
					{
						throw new Exception("Expiro el tiempo de restablecimiento");
					}

				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
            }
		}
	}
}

