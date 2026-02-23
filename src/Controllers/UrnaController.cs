using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Data;
using UrnaElectronica.Models;

namespace UrnaElectronica.Controllers
{
    public class UrnaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UrnaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listado principal
        public async Task<IActionResult> Index()
        {
            var urnas = await _context.Urnas.Where(u => !u.Eliminado).ToListAsync();
            
            ViewBag.TotalUrnas = urnas.Count;
            ViewBag.Operativas = urnas.Count(u => u.Activo);
            ViewBag.EnMantenimiento = urnas.Count(u => !u.Activo);

            return View(urnas);
        }

        // GET: Create (Modal)
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Urna urna)
        {
            if (ModelState.IsValid)
            {
                urna.Eliminado = false;
                _context.Add(urna);
                await _context.SaveChangesAsync();
                Response.Headers.Add("HX-Refresh", "true");
                return Ok();
            }
            return PartialView(urna);
        }

        // GET: Edit (Modal)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var urna = await _context.Urnas.FindAsync(id);
            if (urna == null || urna.Eliminado) return NotFound();
            return PartialView(urna);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Urna urna)
        {
            if (id != urna.Id_Urna) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(urna);
                await _context.SaveChangesAsync();
                Response.Headers.Add("HX-Refresh", "true");
                return Ok();
            }
            return PartialView(urna);
        }

        // POST: Delete (Lógico)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var urna = await _context.Urnas.FindAsync(id);
            if (urna == null) return NotFound();

            urna.Eliminado = true;
            _context.Update(urna);
            await _context.SaveChangesAsync();

            Response.Headers.Add("HX-Refresh", "true");
            return Ok();
        }
    }
}