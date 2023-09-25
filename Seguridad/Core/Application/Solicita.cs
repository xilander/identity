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
    public class Solicita
	{
		public class SolicitaCambioPasswordCommand : IRequest<UsuarioDto>
		{
            public string? Email { get; set; }
        }

        public class SolicitaCambioPasswordHandler : IRequestHandler<SolicitaCambioPasswordCommand, UsuarioDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IMapper _mapper;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly EnviarCorreos _enviarCorreos;

            public SolicitaCambioPasswordHandler(ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper, IJwtGenerator jwtGenerator, IConfiguration configuration)
            {
                _context = context;
                _userManager = userManager;
                _mapper = mapper;
                _jwtGenerator = jwtGenerator;
                _enviarCorreos = new EnviarCorreos(configuration);
            }

            public async Task<UsuarioDto> Handle(SolicitaCambioPasswordCommand request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email!);
                if (usuario == null)
                {
                    throw new Exception("No se reconoce el correo ingresado");
                }

                var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(usuario);
                if (lockoutEndDate != null && lockoutEndDate > DateTimeOffset.Now)
                {
                    throw new Exception(" El usuario está dado de baja");
                }

                var token = _jwtGenerator.CrearTokenAsync(usuario!);
                //var token = await _userManager.GeneratePasswordResetTokenAsync(usuario!);

                MailData data = new MailData();
                data.idcliente = usuario.Id!;
                data.EmailToId = usuario.Email!;
                data.EmailToName = usuario.Nombre!;
                data.EmailSubject = "Recuperar Contraseña";
                data.path = Directory.GetCurrentDirectory() + "/Resources/Plantillas/RecoveryPass.html";

                string filePath = data.path;
                string emailTemplateText = File.ReadAllText(filePath);
                string url = "http://localhost:4200/recupera/" + data.idcliente + "/" + token;

                emailTemplateText = emailTemplateText.Replace("idcliente", data.EmailToName);
                emailTemplateText = emailTemplateText.Replace("idcorreo", data.EmailToId);
                emailTemplateText = emailTemplateText.Replace("idenlace", url);
                data.HtmlBody = emailTemplateText;

                try
                {
                    await _enviarCorreos.SendMailHtml(data);

                    var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
                    return usuarioDTO;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }


    }
}

