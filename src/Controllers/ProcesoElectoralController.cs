using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Data;
using UrnaElectronica.Models;

namespace UrnaElectronica.Controllers
{
    public class ProcesoElectoralController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcesoElectoralController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //if (!VerificarAutenticacion()) return RedirectToAction("Login", "Auth");

            var procesos = await _context.Procesos
                .Where(p => !p.Eliminado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();

            return View(procesos);
        }

        // GET: Parcial de Creación
        public IActionResult Create() {
            return PartialView("_Create", new ProcesoElectoral());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProcesoElectoral modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.FechaCreacion = DateTime.Now;
                modelo.Activo = true;
                modelo.Eliminado = false;
                modelo.IdEstatus = 1; 

                _context.Procesos.Add(modelo);
                await _context.SaveChangesAsync();

                // HTMX refresca la tabla automáticamente al cerrar el modal
                Response.Headers.Add("HX-Refresh", "true");
            }
            return PartialView("_Create", modelo);
        }

        // POST: ProcesoElectoral/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var proceso = await _context.Procesos.FindAsync(id);
            if (proceso == null) return NotFound();
            proceso.Eliminado = true; // Borrado lógico
            _context.Update(proceso);
            await _context.SaveChangesAsync();

            Response.Headers.Add("HX-Refresh", "true");
            return Ok();
        }


        // GET: Parcial de Edición
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var proceso = await _context.Procesos.FindAsync(id);
            if (proceso == null) return NotFound();
            return PartialView("_Edit", proceso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProcesoElectoral modelo) // Eliminamos el 'int id' extra para evitar conflictos de mapeo
        {
            // 1. Verificación de seguridad
            if (modelo.Id == 0) return NotFound();

            // 2. Corregimos la lógica: Si el modelo ES válido, guardamos.
            if (ModelState.IsValid) 
            {
                try
                {
                    // Buscamos el registro original para no perder datos que no están en el form (como FechaCreacion)
                    var procesoDB = await _context.Procesos.FindAsync(modelo.Id);
                    if (procesoDB == null) return NotFound();

                    // Actualizamos solo los campos permitidos
                    procesoDB.Nombre = modelo.Nombre;
                    procesoDB.FechaEvento = modelo.FechaEvento;
                    procesoDB.IdEstatus = modelo.IdEstatus;

                    _context.Update(procesoDB);
                    await _context.SaveChangesAsync();

                    // Notificamos a HTMX que refresque la página para ver los cambios
                    Response.Headers.Add("HX-Refresh", "true");
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Procesos.Any(e => e.Id == modelo.Id)) return NotFound();
                    else throw;
                }
            }

            // Si hay errores de validación, regresamos la parcial para mostrar los mensajes
            return PartialView("_Edit", modelo);
        }

        public static string ObtenerEstatusTexto(int idEstatus)
        {
            return idEstatus switch { 1 => "Activo", 3 => "Finalizado", 4 => "Inactivo", _ => "Desconocido" };
        }
    
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var proceso = await _context.Procesos
                .Include(p => p.Elecciones)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (proceso == null) return NotFound();

            var eleccionesIds = proceso.Elecciones
                .Where(e => !e.Eliminado)
                .Select(e => e.Id)
                .ToList();

            var candidatosPorEleccion = await _context.Candidatos
                .Where(c => eleccionesIds.Contains(c.IdEleccion) && !c.Eliminado)
                .GroupBy(c => c.IdEleccion)
                .Select(g => new { IdEleccion = g.Key, Total = g.Count() })
                .ToDictionaryAsync(x => x.IdEleccion, x => x.Total);

            var urnasPorEleccion = await _context.UrnasElecciones
                .Where(ue => eleccionesIds.Contains(ue.IdEleccion) && !ue.Eliminado)
                .GroupBy(ue => ue.IdEleccion)
                .Select(g => new { IdEleccion = g.Key, Total = g.Count() })
                .ToDictionaryAsync(x => x.IdEleccion, x => x.Total);

            ViewBag.CandidatosPorEleccion = candidatosPorEleccion;
            ViewBag.UrnasPorEleccion = urnasPorEleccion;

            return View(proceso);
        }
    
    }
}