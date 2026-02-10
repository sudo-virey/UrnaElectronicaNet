using Microsoft.AspNetCore.Mvc;
using UrnaElectronica.Models;
using UrnaElectronica.Services;

namespace UrnaElectronica.Controllers
{
    /// <summary>
    /// Controlador de autenticación.
    /// Maneja el login, logout y validación de sesiones.
    /// </summary>
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// GET: Mostrar página de login
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            // Si el usuario ya está logueado, redirigir a Home
            if (HttpContext.Session.GetString("UsuarioEmail") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        /// <summary>
        /// POST: Procesar login
        /// Valida credenciales y establece la sesión si son válidas
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Contraseña,RecuérdameBinding")] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validar credenciales usando el servicio de autenticación
                    var usuario = await _authService.ValidarCredencialesAsync(model.Email, model.Contraseña);

                    if (usuario != null)
                    {
                        // Establecer la sesión
                        HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
                        HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                        HttpContext.Session.SetString("UsuarioRol", usuario.Rol);
                        HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

                        // Opcional: Configurar cookie de "Recuérdame" si está marcado
                        if (model.RecuérdameBinding)
                        {
                            // TODO: Implementar cookie de "Recuérdame" persistent
                            _logger.LogInformation($"Cookie de recuérdame requerida para: {usuario.Email}");
                        }

                        _logger.LogInformation($"Login exitoso para usuario: {usuario.Email}");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email o contraseña incorrectos");
                        _logger.LogWarning($"Intento de login fallido para: {model.Email}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error durante login: {ex.Message}");
                    ModelState.AddModelError("", "Error al procesar el login. Por favor intente de nuevo.");
                }
            }
            
            return View(model);
        }

        /// <summary>
        /// GET: Cerrar sesión
        /// </summary>
        [HttpGet]
        public IActionResult Logout()
        {
            var email = HttpContext.Session.GetString("UsuarioEmail");
            HttpContext.Session.Clear();
            
            if (!string.IsNullOrEmpty(email))
            {
                _logger.LogInformation($"Logout realizado para: {email}");
            }

            return RedirectToAction("Login");
        }

        /// <summary>
        /// GET: Validar si la sesión está activa
        /// Útil para AJAX/API calls
        /// </summary>
        [HttpGet]
        public IActionResult ValidarSesion()
        {
            var usuarioEmail = HttpContext.Session.GetString("UsuarioEmail");
            
            if (!string.IsNullOrEmpty(usuarioEmail))
            {
                return Ok(new
                {
                    activa = true,
                    nombre = HttpContext.Session.GetString("UsuarioNombre"),
                    rol = HttpContext.Session.GetString("UsuarioRol"),
                    email = usuarioEmail,
                    id = HttpContext.Session.GetInt32("UsuarioId")
                });
            }

            return Unauthorized(new { activa = false });
        }
    }
}
