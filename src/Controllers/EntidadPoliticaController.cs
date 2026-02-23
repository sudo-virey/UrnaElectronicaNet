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
            return PartialView();
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
            return PartialView(partido);
        }

        // GET: EntidadPolitica/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Buscamos el partido por su Llave Primaria (Id_Partido)
            var partido = await _context.Partidos.FindAsync(id);

            if (partido == null) return NotFound();

            return PartialView(partido);
        }

        // POST: EntidadPolitica/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Partido partido)
        {
            if (id != partido.Id_Partido) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partido); // Marcamos el objeto como modificado
                    await _context.SaveChangesAsync(); // SQL: UPDATE Partidos SET ...
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Partidos.Any(e => e.Id_Partido == partido.Id_Partido)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return PartialView(partido);
        }
    }
}