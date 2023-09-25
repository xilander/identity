using Seguridad.Core.Entities;

namespace Seguridad.Core.JwtLogic
{
    public interface IJwtGenerator
    {
        string CrearTokenAsync(Usuario usuario);
    }
}
