namespace Seguridad.Core.Dto
{
    public class UsuarioDto
    {
        public string? ID { get; set; }
        public string? UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public string? UnidadNegocio { get; set; }
        public string? Cargo { get; set; }
        public string? Banco { get; set; }
        public string? CuentaBanco { get; set; }
        public string? Clabe { get; set; }
        public string? Rol { get; set; }
        public bool Activo { get; set; }
        public string? Token { get; set; }
    }
}
