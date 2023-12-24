using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Seguridad.Core.Application;
using Seguridad.Core.Dto;
using Seguridad.Core.Entities;

namespace Seguridad.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class SeguridadController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<SeguridadController> _logger;

        public SeguridadController(IMediator mediator, ILogger<SeguridadController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(Login.LoginUsuarioCommand parametros)
        {
            _logger.LogInformation("Ingreso: {correo} el {DT}", parametros.Email, DateTime.UtcNow.ToLocalTime());
            try
            {
                return await _mediator.Send(parametros);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            
        }

        [HttpPost("registrar-usuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDto>> Registrar(Registro.RegistroUsuarioCommand parametros)
        {
            _logger.LogInformation("Registro: {correo} el {DT}", parametros.Email, DateTime.UtcNow.ToLocalTime());
            try
            {
                return await _mediator.Send(parametros);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            
        }

        [HttpGet("listar-usuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<UsuarioDto>>> GetUsuarios()
        {
            return await _mediator.Send(new ListaUsuarios.ListaUsuariosCommand());
        }

        [HttpPost("obtener-usuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioById(ObtenerUsuarioPorId.ObtenerUsuarioPorIdCommand parametros)
        {
            return await _mediator.Send(parametros);
        }

        [HttpPut("actualiza-usuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDto>>Actualizar(ActualizaUsuario.ActualizaUsuarioCommand parametros, IMediator _mediator)
        {
            _logger.LogInformation("Usuario actualizado: {usuarioID} el {DT}", parametros.Email, DateTime.UtcNow.ToLocalTime());
            return await _mediator.Send(parametros);
        }

        [HttpPut("desactiva-usuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaModel>> Desactivar(DesctivaUsuario.DesctivaUsuarioCommand parametros, IMediator _mediator)
        {
            _logger.LogInformation("Usuario desactivado: {usuarioID} el {DT}", parametros.Email, DateTime.UtcNow.ToLocalTime());
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
            try
            {
                await _mediator.Send(parametros);
                return Ok(new
                {
                    code = 200,
                    msj = "Se envió correo para restablecer contraseña",
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPut("cambiar-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioDto>> Recuperar(Recupera.RecuperaPasswordCommand parametros, IMediator _mediator)
        {
            _logger.LogInformation("Cambio de contraseña para: {usuarioID} el {DT}", parametros.Id, DateTime.UtcNow.ToLocalTime());
            try
            {
                return await _mediator.Send(parametros);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("validar-token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ValidarToken()
        {
            return Ok(true);
        }
    }
}
