using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Data; // Para encontrar tu ApplicationDbContext
using UrnaElectronica.Models; // Para encontrar tu clase Partido

namespace UrnaElectronica.Controllers
{
    public class EntidadPoliticaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntidadPoliticaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. VISTA PRINCIPAL: Lista todos los partidos
        public async Task<IActionResult> Index()
        {
            var partidos = await _context.Partidos
                .Where(p => !p.Eliminado)
                .ToListAsync();

            ViewBag.TotalEntidades = partidos.Count;
            ViewBag.TotalCandidatos = 0; // Por ahora 0 ya que la base está vacía

            return View(partidos);
        }

        // 2. FORMULARIO: Muestra la página para escribir los datos (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 3. GUARDAR: Recibe los datos del formulario y los mete a la DB (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partido partido)
        {
            if (ModelState.IsValid)
            {
                partido.Eliminado = false; // Nos aseguramos que el registro esté activo
                _context.Add(partido);
                await _context.SaveChangesAsync(); // <--- Aquí ocurre la magia en SQL
                
                return RedirectToAction(nameof(Index)); // Al terminar, regresa a la tabla
            }
            return View(partido);
        }
    }
}