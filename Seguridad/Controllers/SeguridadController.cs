using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seguridad.Core.Application;
using Seguridad.Core.Data;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;

namespace Seguridad.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class SeguridadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<SeguridadController> _logger;

        public SeguridadController(ApplicationDbContext context, IMediator mediator, ILogger<SeguridadController> logger)
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<Usuario>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("registrar-usuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDto>> Registrar(Registro.RegistroUsuarioCommand parametros)
        {
            _logger.LogInformation("Registro: {correo} el {DT}", parametros.Email, DateTime.UtcNow.ToLocalTime());
            return await _mediator.Send(parametros);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(Login.LoginUsuarioCommand parametros)
        {
            _logger.LogInformation("Ingreso: {correo} el {DT}", parametros.Email, DateTime.UtcNow.ToLocalTime());
            return await _mediator.Send(parametros);
        }

        [HttpGet("sesion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDto>> GetUsuario()
        {
            _logger.LogInformation("Obteniendo el usuario logueado en base a su token");
            return await _mediator.Send(new UsuarioActual.UsuarioActualCommand());
        }

        [HttpPost]
        [Route("enviar-correo-recuperacion")]
        public async Task<ActionResult<UsuarioDto>> SendEmailcliente(Solicita.SolicitaCambioPasswordCommand parametros, IMediator _mediator)
        {
            _logger.LogInformation("Solicitud de recuperación para: {correo} el {DT}", parametros.Email, DateTime.UtcNow.ToLocalTime());
            return await _mediator.Send(parametros);
        }

        [HttpPut("cambiar-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDto>> Recuperar(Recupera.RecuperaPasswordCommand parametros, IMediator _mediator)
        {
            _logger.LogInformation("Cambio de contraseña para: {usuarioID} el {DT}", parametros.Id, DateTime.UtcNow.ToLocalTime());
            return await _mediator.Send(parametros);
        }

        [HttpPut("desactiva-usuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaModel>> Desactivar(DesctivaUsuario.DesctivaUsuarioCommand parametros, IMediator _mediator)
        {
            _logger.LogInformation("Usuario desactivado: {usuarioID} el {DT}", parametros.Email, DateTime.UtcNow.ToLocalTime());
            return await _mediator.Send(parametros);
        }

        [HttpGet("validar-token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ValidarToken()
        {
            return Ok(true);
        }
    }
}
