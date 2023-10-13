using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Seguridad.Core.Application;
using Seguridad.Core.Entities;

namespace Seguridad.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RolController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SeguridadController> _logger;

        public RolController(IMediator mediator, ILogger<SeguridadController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("listar-roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Core.Dto.RolDto>>> GetRoless()
        {
            return await _mediator.Send(new ListaRoles.ListaRolesCommand());
        }

        [HttpPut("cambiar-rol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaModel>> CambiarRol(CambiaRol.CambiaRolCommand parametros, IMediator _mediator)
        {
            return await _mediator.Send(parametros);
        }
    }
}

