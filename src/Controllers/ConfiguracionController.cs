using Microsoft.AspNetCore.Mvc;
using UrnaElectronica.Models;

namespace UrnaElectronica.Controllers
{
    public class ConfiguracionController : BaseController
    {
        private readonly ILogger<ConfiguracionController> _logger;
        
        // Simulación de base de datos en memoria
        private static List<ConfiguracionUrna> _configuraciones = new()
        {
            new ConfiguracionUrna
            {
                Id = 1,
                Nombre = "Urna Principal",
                Ubicacion = "Centro Cívico",
                NumeroMesa = 1,
                Departamento = "La Paz",
                Municipio = "La Paz",
                Activa = true
            }
        };

        public ConfiguracionController(ILogger<ConfiguracionController> logger)
        {
            _logger = logger;
        }

        // GET: Configuracion
        public IActionResult Index()
        {
            // Verificar autenticación
            if (HttpContext.Session.GetString("UsuarioEmail") == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            return View(_configuraciones);
        }

        // GET: Configuracion/Details/5
        public IActionResult Details(int id)
        {
            var configuracion = _configuraciones.FirstOrDefault(c => c.Id == id);
            if (configuracion == null)
            {
                return NotFound();
            }
            return View(configuracion);
        }

        // GET: Configuracion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Configuracion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Nombre,Ubicacion,NumeroMesa,Departamento,Municipio,Activa")] ConfiguracionUrna configuracion)
        {
            if (ModelState.IsValid)
            {
                configuracion.Id = _configuraciones.Max(c => c.Id) + 1;
                _configuraciones.Add(configuracion);
                return RedirectToAction(nameof(Index));
            }
            return View(configuracion);
        }

        // GET: Configuracion/Edit/5
        public IActionResult Edit(int id)
        {
            var configuracion = _configuraciones.FirstOrDefault(c => c.Id == id);
            if (configuracion == null)
            {
                return NotFound();
            }
            return View(configuracion);
        }

        // POST: Configuracion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nombre,Ubicacion,NumeroMesa,Departamento,Municipio,Activa")] ConfiguracionUrna configuracion)
        {
            if (id != configuracion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existente = _configuraciones.FirstOrDefault(c => c.Id == id);
                if (existente != null)
                {
                    existente.Nombre = configuracion.Nombre;
                    existente.Ubicacion = configuracion.Ubicacion;
                    existente.NumeroMesa = configuracion.NumeroMesa;
                    existente.Departamento = configuracion.Departamento;
                    existente.Municipio = configuracion.Municipio;
                    existente.Activa = configuracion.Activa;
                    existente.FechaActualizacion = DateTime.Now;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(configuracion);
        }

        // GET: Configuracion/Delete/5
        public IActionResult Delete(int id)
        {
            var configuracion = _configuraciones.FirstOrDefault(c => c.Id == id);
            if (configuracion == null)
            {
                return NotFound();
            }
            return View(configuracion);
        }

        // POST: Configuracion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var configuracion = _configuraciones.FirstOrDefault(c => c.Id == id);
            if (configuracion != null)
            {
                _configuraciones.Remove(configuracion);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
