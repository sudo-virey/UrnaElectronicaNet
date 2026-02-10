using Microsoft.AspNetCore.Mvc;

namespace UrnaElectronica.Controllers
{
    /// <summary>
    /// Controlador base que proporciona funcionalidad común para todos los controladores.
    /// Verifica que el usuario tenga una sesión activa.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Se ejecuta antes de cada acción.
        /// Verifica si el usuario tiene una sesión activa.
        /// </summary>
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            // Información de sesión disponible en toda la aplicación
            var usuarioEmail = HttpContext.Session.GetString("UsuarioEmail");
            var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            var usuarioRol = HttpContext.Session.GetString("UsuarioRol");
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            // Pasar información de sesión a la vista
            ViewBag.UsuarioEmail = usuarioEmail;
            ViewBag.UsuarioNombre = usuarioNombre;
            ViewBag.UsuarioRol = usuarioRol;
            ViewBag.UsuarioId = usuarioId;
            ViewBag.EstaAutenticado = !string.IsNullOrEmpty(usuarioEmail);

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Verifica si el usuario está autenticado.
        /// Puede ser usado en acciones para redireccionar a login si no hay sesión.
        /// </summary>
        protected bool VerificarAutenticacion()
        {
            var usuarioEmail = HttpContext.Session.GetString("UsuarioEmail");
            return !string.IsNullOrEmpty(usuarioEmail);
        }

        /// <summary>
        /// Obtiene el ID del usuario actual de la sesión.
        /// </summary>
        protected int? ObtenerIdUsuarioActual()
        {
            return HttpContext.Session.GetInt32("UsuarioId");
        }

        /// <summary>
        /// Obtiene el email del usuario actual de la sesión.
        /// </summary>
        protected string ObtenerEmailUsuarioActual()
        {
            return HttpContext.Session.GetString("UsuarioEmail");
        }

        /// <summary>
        /// Obtiene el rol del usuario actual de la sesión.
        /// </summary>
        protected string ObtenerRolUsuarioActual()
        {
            return HttpContext.Session.GetString("UsuarioRol");
        }
    }
}
