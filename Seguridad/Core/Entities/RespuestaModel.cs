namespace Seguridad.Core.Entities
{
	public class RespuestaModel
	{
        public bool Ok { get; set; }
        public int Status { get; set; }
        public string? StatusText { get; set; }
        public string? Mensaje { get; set; }
	}
}

