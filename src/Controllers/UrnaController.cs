using System.Security.Cryptography;
using System.Text;
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

        // Listar accesos de una urna
        public async Task<IActionResult> Accesos(int id)
        {
            var urna = await _context.Urnas.FindAsync(id);
            var lista = await _context.Accesos
                .Where(a => a.Id_Urna == id && !a.Eliminado)
                .ToListAsync();

            ViewBag.UrnaNombre = urna?.Nombre;
            ViewBag.IdUrna = id;
            return PartialView("_ListaAccesos", lista);
        }

        // Eliminar acceso (Borrado lógico)
        [HttpPost]
        public async Task<IActionResult> EliminarAcceso(int id)
        {
            var acceso = await _context.Accesos.FindAsync(id);
            if (acceso == null) return NotFound();

            int idUrna = acceso.Id_Urna;
            acceso.Eliminado = true;
            _context.Update(acceso);
            await _context.SaveChangesAsync();

            // Recargamos la lista parcial en el modal
            return RedirectToAction("Accesos", new { id = idUrna });
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerarAccesoUnico(int idUrna)
        {
            // 1. Generar un Hash único
            string rawData = Guid.NewGuid().ToString() + DateTime.Now.Ticks;
            string hashUnico;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                // Convertimos a string hex y tomamos 12 caracteres para que el QR no sea gigante
                // pero mantenga una entropía alta.
                hashUnico = BitConverter.ToString(bytes).Replace("-", "").Substring(0, 12);
            }

            // 2. Verificar que no exista ya en la DB (aunque es estadísticamente improbable)
            while (await _context.Accesos.AnyAsync(a => a.Codigo == hashUnico))
            {
                hashUnico = Guid.NewGuid().ToString().Substring(0, 12).ToUpper();
            }

            var nuevoAcceso = new Acceso
            {
                Id_Urna = idUrna,
                Codigo = hashUnico,
                Activo = true,
                Eliminado = false
            };

            _context.Accesos.Add(nuevoAcceso);
            await _context.SaveChangesAsync();

            // 3. Recargar la lista parcial en el modal usando HTMX
            return RedirectToAction("Accesos", new { id = idUrna });
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> GenerarLote(int idUrna)
{
    var urna = await _context.Urnas.FindAsync(idUrna);
    if (urna == null) return NotFound();

    for (int i = 0; i < 20; i++)
    {
        // Generamos un Hash SHA-256 único por cada iteración
        string rawData = Guid.NewGuid().ToString() + DateTime.Now.Ticks + i;
        string hashUnico;

        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            // Tomamos 12 caracteres para que el QR sea legible y seguro
            hashUnico = BitConverter.ToString(bytes).Replace("-", "").Substring(0, 12);
        }

        var nuevoAcceso = new Acceso
        {
            Id_Urna = idUrna,
            Codigo = hashUnico,
            Activo = true,
            Eliminado = false
        };
        _context.Accesos.Add(nuevoAcceso);
    }

        await _context.SaveChangesAsync();

        // IMPORTANTE: Redirigimos al método que lista los accesos 
        // para que HTMX actualice el contenido del modal con los 20 nuevos
        return RedirectToAction("Accesos", new { id = idUrna });
    }
    }
}