using Seguridad.Core.Entities;

namespace Seguridad.Core.JwtLogic
{
    public interface IJwtGenerator
    {
        string CrearToken(Usuario usuario);
    }
}
