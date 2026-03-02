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
    
        // GET: Eleccion/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var eleccion = await _context.Elecciones.FindAsync(id);
            if (eleccion == null) return NotFound();
            
            return PartialView("_Edit", eleccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Eleccion modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // En HTMX, a veces es mejor recuperar la entidad original para no perder datos 
                    // que no están en el formulario (como IdProceso o Activo)
                    var original = await _context.Elecciones.FindAsync(modelo.Id);
                    if (original == null) return NotFound();

                    original.Nombre = modelo.Nombre;
                    original.Activo = modelo.Activo;
                    // Agrega aquí otros campos que necesites editar de la tabla Elecciones
                    
                    _context.Update(original);
                    await _context.SaveChangesAsync();

                    // Refrescamos la página para ver los cambios reflejados
                    Response.Headers.Add("HX-Refresh", "true");
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error o manejarlo de alguna forma
                ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar la elección.");
            }
            return PartialView("_Edit", modelo);
        }
    
    }
}