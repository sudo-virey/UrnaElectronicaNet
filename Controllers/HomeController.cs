using Microsoft.AspNetCore.Mvc;

namespace UrnaElectronica.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Verificar si el usuario está autenticado
            if (HttpContext.Session.GetString("UsuarioEmail") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            if (HttpContext.Session.GetString("UsuarioEmail") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
