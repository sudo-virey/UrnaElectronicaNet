using Microsoft.AspNetCore.Mvc;

namespace UrnaElectronica.Controllers
{
    /// <summary>
    /// Controlador para gestionar urnas electrónicas
    /// </summary>
    public class UrnaController : BaseController
    {
        /// <summary>
        /// Listar todas las urnas
        /// </summary>
        public IActionResult Index()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            // Datos de ejemplo
            var urnas = new List<dynamic>
            {
                new { Id = 1, Codigo = "URN-001", Estado = "Operativa", UbicacionFisica = "Aula A-101", VersionSoftware = "2.1.0" },
                new { Id = 2, Codigo = "URN-002", Estado = "Operativa", UbicacionFisica = "Aula A-102", VersionSoftware = "2.1.0" },
                new { Id = 3, Codigo = "URN-003", Estado = "Mantenimiento", UbicacionFisica = "Aula B-201", VersionSoftware = "2.0.5" },
                new { Id = 4, Codigo = "URN-004", Estado = "Operativa", UbicacionFisica = "Aula B-202", VersionSoftware = "2.1.0" }
            };

            ViewBag.TotalUrnas = urnas.Count;
            ViewBag.Operativas = urnas.Count(u => u.Estado == "Operativa");
            ViewBag.EnMantenimiento = urnas.Count(u => u.Estado == "Mantenimiento");

            return View(urnas);
        }

        /// <summary>
        /// Ver detalles técnicos de una urna
        /// </summary>
        public IActionResult Details(int id)
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            var urna = new 
            { 
                Id = id,
                Codigo = $"URN-{id:D3}",
                Estado = "Operativa",
                UbicacionFisica = "Aula A-101",
                VersionSoftware = "2.1.0",
                FechaUltimaVerificacion = DateTime.Now.AddDays(-5),
                CertificadoDigital = "Válido hasta 2025-12-31"
            };

            return View(urna);
        }

        /// <summary>
        /// Formulario para registrar nueva urna
        /// </summary>
        public IActionResult Create()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        /// <summary>
        /// Guardar nueva urna
        /// </summary>
        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            // TODO: Validar y guardar en BD
            return RedirectToAction(nameof(Index));
        }
    }
}
