using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Data;
using UrnaElectronica.Models;

namespace UrnaElectronica.Controllers
{
    public class ImpresoraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImpresoraController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Solo traemos las que NO estén eliminadas
            var impresoras = await _context.Impresoras
                .Where(i => !i.Eliminado)
                .ToListAsync();
            return View(impresoras);
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Impresora impresora)
        {
            if (ModelState.IsValid)
            {
                impresora.Eliminado = false; // Aseguramos que no nazca borrada
                _context.Add(impresora);
                await _context.SaveChangesAsync();
                
                // HTMX refresca la tabla automáticamente al cerrar el modal
                Response.Headers.Add("HX-Refresh", "true");
                return Ok();
            }
            return PartialView(impresora);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var impresora = await _context.Impresoras.FindAsync(id);
            if (impresora == null) return NotFound();

            impresora.Eliminado = true; // Borrado lógico
            _context.Update(impresora);
            await _context.SaveChangesAsync();

            Response.Headers.Add("HX-Refresh", "true");
            return Ok();
        }
        
        

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var impresora = await _context.Impresoras.FindAsync(id);
        if (impresora == null || impresora.Eliminado) return NotFound();

        return PartialView(impresora);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Impresora impresora)
        {
            if (id != impresora.Id_Impresora) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(impresora);
                    await _context.SaveChangesAsync();
                    Response.Headers.Add("HX-Refresh", "true");
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Impresoras.Any(e => e.Id_Impresora == id)) return NotFound();
                    else throw;
                }
            }
            return PartialView(impresora);
        }
        
    }
}