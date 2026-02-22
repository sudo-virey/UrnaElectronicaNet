using Microsoft.AspNetCore.Mvc;

namespace UrnaElectronica.Controllers
{
    /// <summary>
    /// Controlador para gestionar impresoras de auditoría
    /// </summary>
    public class ImpresoraController : BaseController
    {
        /// <summary>
        /// Listar todas las impresoras
        /// </summary>
        public IActionResult Index()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            // Datos de ejemplo
            var impresoras = new List<dynamic>
            {
                new { Id = 1, Codigo = "IMP-001", Modelo = "HP LaserJet Pro", Estado = "Operativa", Ubicacion = "Centro de Control 1", PaperRestante = "Suficiente" },
                new { Id = 2, Codigo = "IMP-002", Modelo = "Canon imagePROGRAF", Estado = "Operativa", Ubicacion = "Centro de Control 1", PaperRestante = "Suficiente" },
                new { Id = 3, Codigo = "IMP-003", Modelo = "Ricoh MP C3004", Estado = "Bajo Papel", Ubicacion = "Centro de Control 2", PaperRestante = "Crítico" },
                new { Id = 4, Codigo = "IMP-004", Modelo = "Brother HL-L8360", Estado = "Inactiva", Ubicacion = "Almacén", PaperRestante = "N/A" }
            };

            ViewBag.TotalImpresoras = impresoras.Count;
            ViewBag.Operativas = impresoras.Count(i => i.Estado == "Operativa");
            ViewBag.ConProblemas = impresoras.Count(i => i.Estado != "Operativa");

            return View(impresoras);
        }

        /// <summary>
        /// Ver detalles de una impresora
        /// </summary>
        public IActionResult Details(int id)
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            var impresora = new 
            { 
                Id = id,
                Codigo = $"IMP-{id:D3}",
                Modelo = "HP LaserJet Pro M454dw",
                Serie = "SN123456789",
                Estado = "Operativa",
                Ubicacion = "Centro de Control 1",
                ToneRestante = "75%",
                PaperRestante = "Suficiente",
                FechaUltimoMantenimiento = DateTime.Now.AddDays(-15),
                ProximoMantenimiento = DateTime.Now.AddDays(15)
            };

            return View(impresora);
        }

        /// <summary>
        /// Registrar nueva impresora
        /// </summary>
        public IActionResult Create()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        /// <summary>
        /// Guardar nueva impresora
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
