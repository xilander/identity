using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
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

        public SeguridadController(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Usuario>> GetUsuarios()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("registar")]
        public async Task<ActionResult<UsuarioDto>> Registrar(Registro.RegistroUsuarioCommand parametros)
        {
            return await _mediator.Send(parametros);
        }
    }
}
