using Microsoft.AspNetCore.Identity;

namespace Seguridad.Core.Entities
{
    public class Usuario : IdentityUser
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? UnidadNegocio { get; set; }
        public string? Cargo { get; set; }
        public string? Banco { get; set; }
        public string? CuentaBanco { get; set; }
        public string? Clabe { get; set; }
    }

    public class Rol : IdentityRole
    {
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
    }
}
