namespace UrnaElectronica.Middleware
{
    /// <summary>
    /// Middleware para verificar autenticación.
    /// Redirige a la página de login si el usuario intenta acceder a rutas protegidas sin sesión activa.
    /// </summary>
    public class AutenticacionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AutenticacionMiddleware> _logger;

        // Rutas públicas que no requieren autenticación
        private static readonly List<string> _rutasPublicas = new()
        {
            "/Auth/Login",
            "/Auth/Logout",
            "/Auth/ValidarSesion",
            "/Home/Privacy",
            "/css/",
            "/js/",
            "/images/",
            "/lib/"
        };

        public AutenticacionMiddleware(RequestDelegate next, ILogger<AutenticacionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            // Verificar si la ruta es pública
            bool esRutaPublica = _rutasPublicas.Any(ruta => 
                path.StartsWith(ruta, StringComparison.OrdinalIgnoreCase));

            if (!esRutaPublica)
            {
                var usuarioEmail = context.Session.GetString("UsuarioEmail");
                
                // Si no hay sesión y no es ruta pública, redirigir a login
                if (string.IsNullOrEmpty(usuarioEmail) && path != "/")
                {
                    _logger.LogWarning($"Acceso denegado a ruta protegida: {path}");

                    var esRequestHtmx = string.Equals(
                        context.Request.Headers["HX-Request"],
                        "true",
                        StringComparison.OrdinalIgnoreCase);

                    if (esRequestHtmx)
                    {
                        context.Response.Headers["HX-Redirect"] = "/Auth/Login";
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        context.Response.ContentType = "text/html; charset=utf-8";
                        await context.Response.WriteAsync("<script>window.location='/Auth/Login';</script>");
                        return;
                    }

                    context.Response.Redirect("/Auth/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}
