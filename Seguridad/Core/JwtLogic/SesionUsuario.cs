namespace Seguridad.Core.JwtLogic
{
    public class SesionUsuario : ISesionUsuario
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SesionUsuario(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetSesionUsuario()
        {
            var context = _httpContextAccessor.HttpContext;

            var userName = context.User.Claims.FirstOrDefault( x => x.Type == "username" ).Value;

               // _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == "username")?.Value;
            return userName!;
        }

    }
}
