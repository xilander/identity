using MediatR;
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
        [Authorize]
        public async Task<List<Usuario>> GetUsuarios()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("registar")]
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
        public async Task<ActionResult<UsuarioDto>> Get()
        {
            _logger.LogError("no se que paso");
            return await _mediator.Send(new UsuarioActual.UsuarioActualCommand());
        }
    }
}
