using Microsoft.IdentityModel.Tokens;
using Seguridad.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Seguridad.Core.JwtLogic
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly ILogger _logger;

        public JwtGenerator(ILogger<JwtGenerator> logger)
        {
            _logger = logger;
        }

        public string CrearToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PwgGWhA738I4HoJHEMxEZttLUunzBmpY"));
            var credencial = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim("username", usuario.UserName!),
                new Claim("nombre", usuario.Nombre!),
                new Claim("apellido", usuario.Apellido!),
            };

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(361),
                SigningCredentials = credencial
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            var inicia = token.ValidFrom;
            _logger.LogInformation("Hora de inicio del token {inicia}", inicia);
            var expira = token.ValidTo;
            _logger.LogInformation("Hora de expiracion del token {expira}", expira);

            return tokenHandler.WriteToken(token);
        }

    }
}
