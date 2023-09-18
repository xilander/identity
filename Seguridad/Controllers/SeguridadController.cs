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
        public async Task<ActionResult<UsuarioDto>> Registrar(Registro.RegistroUsuarioCommand parametros)
        {
            return await _mediator.Send(parametros);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(Login.LoginUsuarioCommand parametros)
        {
            return await _mediator.Send(parametros);
        }

        [HttpGet("sesion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDto>> GetUsuario()
        {
            _logger.LogError("Obteniendo el usuario logueado en base a su token");
            return await _mediator.Send(new UsuarioActual.UsuarioActualCommand());
        }

        [HttpPost]
        [Route("enviar-correo-recuperacion")]
        public async Task<ActionResult<UsuarioDto>> SendEmailcliente(Solicita.SolicitaCambioPasswordCommand parametros, IMediator _mediator)
        {
            return await _mediator.Send(parametros);
        }

        [HttpPut("cambiar-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDto>> Recuperar(Recupera.RecuperaPasswordCommand parametros, IMediator _mediator)
        {
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
