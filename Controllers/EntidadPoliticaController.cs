using Microsoft.AspNetCore.Mvc;

namespace UrnaElectronica.Controllers
{
    /// <summary>
    /// Controlador para gestionar entidades políticas (partidos, listas, candidatos)
    /// </summary>
    public class EntidadPoliticaController : BaseController
    {
        /// <summary>
        /// Listar todas las entidades políticas
        /// </summary>
        public IActionResult Index()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            // Datos de ejemplo
            var entidades = new List<dynamic>
            {
                new { Id = 1, Nombre = "Partido Democrático", Siglas = "PD", Tipo = "Partido", CandidatosRegistrados = 45, Estado = "Activo" },
                new { Id = 2, Nombre = "Movimiento Ciudadano", Siglas = "MC", Tipo = "Movimiento", CandidatosRegistrados = 38, Estado = "Activo" },
                new { Id = 3, Nombre = "Unidad Popular", Siglas = "UP", Tipo = "Coalición", CandidatosRegistrados = 62, Estado = "Activo" },
                new { Id = 4, Nombre = "Independientes", Siglas = "IND", Tipo = "Independientes", CandidatosRegistrados = 12, Estado = "Activo" }
            };

            ViewBag.TotalEntidades = entidades.Count;
            ViewBag.TotalCandidatos = entidades.Sum(e => e.CandidatosRegistrados);

            return View(entidades);
        }

        /// <summary>
        /// Ver detalles de una entidad política
        /// </summary>
        public IActionResult Details(int id)
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            var entidad = new 
            { 
                Id = id,
                Nombre = "Partido Democrático",
                Siglas = "PD",
                Tipo = "Partido",
                Representante = "Dr. Juan García López",
                Email = "contacto@partidodem.ec",
                Teléfono = "+593-2-1234567",
                CandidatosRegistrados = 45,
                FechaRegistro = DateTime.Now.AddDays(-45),
                Estado = "Activo"
            };

            return View(entidad);
        }

        /// <summary>
        /// Registrar nueva entidad política
        /// </summary>
        public IActionResult Create()
        {
            if (!VerificarAutenticacion())
                return RedirectToAction("Login", "Auth");

            return View();
        }

        /// <summary>
        /// Guardar nueva entidad política
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
