using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Data;
using UrnaElectronica.Models;

namespace UrnaElectronica.Controllers
{
    public class EntidadPoliticaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntidadPoliticaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var partidos = await _context.Partidos.Where(p => !p.Eliminado).ToListAsync();
            ViewBag.TotalEntidades = partidos.Count;
            ViewBag.TotalCandidatos = 0;
            return View(partidos);
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partido partido)
        {
            if (ModelState.IsValid)
            {
                partido.Eliminado = false;
                _context.Add(partido);
                await _context.SaveChangesAsync();
                
                // Instrucción para que HTMX recargue la página tras guardar
                Response.Headers.Add("HX-Refresh", "true");
                return Ok();
            }
            return PartialView(partido);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var partido = await _context.Partidos.FindAsync(id);
            if (partido == null) return NotFound();
            return PartialView(partido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Partido partido)
        {
            if (id != partido.Id_Partido) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(partido);
                await _context.SaveChangesAsync();
                
                Response.Headers.Add("HX-Refresh", "true");
                return Ok();
            }   
            return PartialView(partido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var partido = await _context.Partidos.FindAsync(id);
            if (partido == null) return NotFound();

            partido.Eliminado = true; 
            _context.Update(partido);
            await _context.SaveChangesAsync();

            Response.Headers.Add("HX-Refresh", "true");
            return Ok();
        }
    }
}