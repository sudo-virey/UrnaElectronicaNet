using Microsoft.AspNetCore.Mvc;
using UrnaElectronica.Models;

namespace UrnaElectronica.Controllers
{
    /// <summary>
    /// Controlador para gestionar procesos electorales
    /// </summary>
    public class ProcesoElectoralController : BaseController
    {
        /// <summary>
        /// Listar todos los procesos electorales
        /// </summary>
        public IActionResult Index()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            // Datos de ejemplo
            var procesos = new List<ProcesoElectoral>
            {
                new ProcesoElectoral 
                { 
                    Id = 1, 
                    Nombre = "Elección Estudiantil 2023",
                    Descripcion = "Elección para directiva estudiantil",
                    FechaInicio = DateTime.Now.AddDays(-30),
                    FechaFin = DateTime.Now.AddDays(30)
                },
                new ProcesoElectoral 
                { 
                    Id = 2, 
                    Nombre = "Consejo Universitario 2024",
                    Descripcion = "Elección del consejo universitario",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddDays(60)
                }
            };

            return View(procesos);
        }

        /// <summary>
        /// Ver detalles de un proceso electoral
        /// </summary>
        public IActionResult Details(int id)
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            var proceso = new ProcesoElectoral 
            { 
                Id = id,
                Nombre = "Elección Estudiantil 2023",
                Descripcion = "Elección para directiva estudiantil",
                FechaInicio = DateTime.Now.AddDays(-30),
                FechaFin = DateTime.Now.AddDays(30)
            };

            return View(proceso);
        }

        /// <summary>
        /// Crear nuevo proceso electoral
        /// </summary>
        public IActionResult Create()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        /// <summary>
        /// Guardar nuevo proceso electoral
        /// </summary>
        [HttpPost]
        public IActionResult Create(ProcesoElectoral modelo)
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                // TODO: Guardar en la base de datos
                return RedirectToAction(nameof(Index));
            }

            return View(modelo);
        }
    }
}
