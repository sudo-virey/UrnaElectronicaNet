using Microsoft.AspNetCore.Mvc;
using UrnaElectronica.Models;
using System.Security.Cryptography;
using System.Text;

namespace UrnaElectronica.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        
        // Base de datos simulada en memoria
        private static List<Usuario> _usuarios = new()
        {
            new Usuario
            {
                Id = 1,
                Nombre = "Administrador",
                Email = "admin@urna.gov",
                Contraseña = HashPassword("admin123"),
                Rol = "Admin",
                Activo = true
            },
            new Usuario
            {
                Id = 2,
                Nombre = "Operador",
                Email = "operador@urna.gov",
                Contraseña = HashPassword("operador123"),
                Rol = "Operador",
                Activo = true
            }
        };

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            // Si el usuario ya está logueado, redirigir a Home
            if (HttpContext.Session.GetString("UsuarioEmail") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Contraseña,RecuérdameBinding")] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = _usuarios.FirstOrDefault(u =>
                    u.Email == model.Email &&
                    u.Activo &&
                    VerifyPassword(model.Contraseña, u.Contraseña));

                if (usuario != null)
                {
                    // Guardar en sesión
                    HttpContext.Session.SetString("UsuarioEmail", usuario.Email);
                    HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                    HttpContext.Session.SetString("UsuarioRol", usuario.Rol);
                    HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                    
                    _logger.LogInformation($"Usuario {usuario.Email} autenticado correctamente");
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email o contraseña incorrectos");
                    _logger.LogWarning($"Intento de login fallido para: {model.Email}");
                }
            }
            return View(model);
        }

        // GET: Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Método auxiliar para hash de contraseña
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Método auxiliar para verificar contraseña
        private static bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }
}
