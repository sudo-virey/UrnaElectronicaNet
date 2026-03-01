using Microsoft.AspNetCore.Mvc;
using UrnaElectronica.Data;
using UrnaElectronica.Models;

namespace UrnaElectronica.Controllers
{
    public class EleccionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EleccionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Modal de creación
        public IActionResult Create(int procesoId)
        {
            // Pasamos el procesoId para que el form sepa a qué proceso pertenece
            var nuevaEleccion = new Eleccion { IdProceso = procesoId };
            return PartialView("_Create", nuevaEleccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Eleccion modelo)
        {
            
            if (ModelState.IsValid)
            {
                modelo.Activo = true;
                modelo.Eliminado = false;
                _context.Elecciones.Add(modelo);
                await _context.SaveChangesAsync();

                // Al ser una vista de detalles, refrescamos la página para ver la nueva elección en la lista
                Response.Headers.Add("HX-Refresh", "true");
                return Ok();
            }
            return PartialView("_Create", modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var eleccion = await _context.Elecciones.FindAsync(id);
            if (eleccion == null) return NotFound();

            eleccion.Eliminado = true;
            _context.Update(eleccion);
            await _context.SaveChangesAsync();

            Response.Headers.Add("HX-Refresh", "true");
            return Ok();
        }
    }
}