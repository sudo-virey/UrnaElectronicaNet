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
        /// Listar todos los procesos electorales activos
        /// </summary>
        public IActionResult Index()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            // Datos de ejemplo usando el nuevo modelo simplificado
            var procesos = new List<ProcesoElectoral>
            {
                new ProcesoElectoral 
                { 
                    Id = 1, 
                    Nombre = "Elección Estudiantil 2023",
                    FechaCreacion = DateTime.Now.AddDays(-30),
                    IdEstatus = 1
                },
                new ProcesoElectoral 
                { 
                    Id = 2, 
                    Nombre = "Consejo Universitario 2024",
                    FechaCreacion = DateTime.Now,
                    IdEstatus = 1
                },
                new ProcesoElectoral 
                { 
                    Id = 3, 
                    Nombre = "Junta Directiva 2022",
                    FechaCreacion = DateTime.Now.AddDays(-365),
                    IdEstatus = 3 // Terminado
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
                FechaCreacion = DateTime.Now.AddDays(-30),
                IdEstatus = 1
            };

            return View(proceso);
        }

        /// <summary>
        /// Crear nuevo proceso electoral (GET - para formularios tradicionales)
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        /// <summary>
        /// Guardar nuevo proceso electoral (POST JSON)
        /// </summary>
        [HttpPost]
        public IActionResult CreateProceso([FromBody] ProcesoElectoral modelo)
        {
            if (!VerificarAutenticacion())
                return Unauthorized();

            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                return Json(new { 
                    success = false, 
                    message = "El nombre del proceso es requerido" 
                });
            }

            // Crear nuevo proceso con id_estatus = 1 (Activo)
            var nuevoProcesoId = new Random().Next(100, 999); // Simulando ID auto-generado
            var nuevosProceso = new ProcesoElectoral
            {
                Id = nuevoProcesoId,
                Nombre = modelo.Nombre.Trim(),
                FechaCreacion = DateTime.Now,
                IdEstatus = (int)EstatusProcesoElectoral.Activo
            };

            // TODO: Guardar en base de datos

            return Json(new { 
                success = true, 
                message = "Proceso creado exitosamente",
                data = nuevosProceso
            });
        }

        /// <summary>
        /// Obtener estado en texto
        /// </summary>
        public string ObtenerEstatusTexto(int idEstatus)
        {
            return idEstatus switch
            {
                1 => "Activo",
                2 => "Eliminado",
                3 => "Terminado",
                _ => "Desconocido"
            };
        }
    }
}
