namespace Seguridad.Core.Dto
{
    public class UsuarioDto
    {
        public string? ID { get; set; }
        public string? UserName { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Token { get; set; }
        public DateTime? Expira { get; set; }

    }
}
