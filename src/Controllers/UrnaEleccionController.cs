using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Data;
using UrnaElectronica.Models;

namespace UrnaElectronica.Controllers
{
    public class UrnaEleccionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string ListaPartialPath = "~/Views/UrnaEleccion/_Listar.cshtml";
        private const string CreatePartialPath = "~/Views/UrnaEleccion/_Create.cshtml";
        private const string EditPartialPath = "~/Views/UrnaEleccion/_Edit.cshtml";

        public UrnaEleccionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Listar(int id, bool esProcesoFinalizado = false, bool eleccionActiva = true)
        {
            var urnasEleccion = await _context.UrnasElecciones
                .Where(ue => ue.IdEleccion == id && !ue.Eliminado)
                .OrderBy(ue => ue.IdUrna)
                .ToListAsync();

            var urnas = await _context.Urnas
                .Where(u => !u.Eliminado)
                .OrderBy(u => u.Nombre)
                .ToListAsync();

            ViewBag.IdEleccion = id;
            ViewBag.UrnasMap = urnas.ToDictionary(u => u.Id_Urna, u => u.Nombre);
            ViewBag.EstatusMap = GetEstatusMap();
            ViewBag.EsProcesoFinalizado = esProcesoFinalizado;
            ViewBag.EleccionActiva = eleccionActiva;

            return PartialView(ListaPartialPath, urnasEleccion);
        }

        public async Task<IActionResult> Create(int eleccionId)
        {
            await CargarCatalogosCreateEdit(eleccionId);

            var modelo = new UrnaEleccion
            {
                IdEleccion = eleccionId,
                IdEstatusUrna = 1,
                Activo = true,
                Eliminado = false
            };

            return PartialView(CreatePartialPath, modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UrnaEleccion modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.IdEstatusUrna = 1;
                var yaAsignada = await _context.UrnasElecciones.AnyAsync(ue =>
                    ue.IdEleccion == modelo.IdEleccion &&
                    ue.IdUrna == modelo.IdUrna &&
                    !ue.Eliminado);

                if (yaAsignada)
                {
                    ModelState.AddModelError(string.Empty, "La urna ya está asignada a esta elección.");
                }
                else
                {
                    modelo.Eliminado = false;

                    _context.UrnasElecciones.Add(modelo);
                    await _context.SaveChangesAsync();

                    Response.Headers["HX-Trigger"] = $"{{\"urnaEleccionChanged\":{{\"idEleccion\":{modelo.IdEleccion}}}}}";
                    return Ok();
                }
            }

            await CargarCatalogosCreateEdit(modelo.IdEleccion);
            return PartialView(CreatePartialPath, modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var modelo = await _context.UrnasElecciones
                .FirstOrDefaultAsync(ue => ue.IdUrnaEleccion == id && !ue.Eliminado);

            if (modelo == null)
            {
                return NotFound();
            }

            await CargarCatalogosCreateEdit(modelo.IdEleccion, modelo.IdUrnaEleccion);
            return PartialView(EditPartialPath, modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [FromForm(Name = "ListaNominal")] string? listaNominalRaw,
            [FromForm(Name = "IdUrna")] string? idUrnaRaw,
            [FromForm(Name = "Activo")] bool activo)
        {
            var existente = await _context.UrnasElecciones
                .FirstOrDefaultAsync(ue => ue.IdUrnaEleccion == id && !ue.Eliminado);

            if (existente == null)
            {
                return NotFound();
            }

            var listaNominal = existente.ListaNominal;
            if (!string.IsNullOrWhiteSpace(listaNominalRaw))
            {
                if (int.TryParse(listaNominalRaw, out var listaNominalParseada))
                {
                    listaNominal = listaNominalParseada;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El valor de lista nominal no es válido.");
                }
            }

            var urnaSeleccionada = existente.IdUrna;
            if (!string.IsNullOrWhiteSpace(idUrnaRaw))
            {
                if (int.TryParse(idUrnaRaw, out var idUrnaParseada))
                {
                    urnaSeleccionada = idUrnaParseada;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La urna seleccionada no es válida.");
                }
            }

            if (listaNominal < 0)
            {
                ModelState.AddModelError(string.Empty, "La lista nominal no puede ser negativa.");
            }

            var duplicada = await _context.UrnasElecciones.AnyAsync(ue =>
                ue.IdEleccion == existente.IdEleccion &&
                ue.IdUrna == urnaSeleccionada &&
                ue.IdUrnaEleccion != id &&
                !ue.Eliminado);

            if (duplicada)
            {
                ModelState.AddModelError(string.Empty, "La urna seleccionada ya está asignada a esta elección.");
            }

            if (!ModelState.IsValid)
            {
                existente.ListaNominal = listaNominal;
                existente.IdUrna = urnaSeleccionada;
                existente.Activo = activo;

                await CargarCatalogosCreateEdit(existente.IdEleccion, existente.IdUrnaEleccion);
                return PartialView(EditPartialPath, existente);
            }

            existente.ListaNominal = listaNominal;
            existente.IdUrna = urnaSeleccionada;
            existente.Activo = activo;

            _context.UrnasElecciones.Update(existente);
            await _context.SaveChangesAsync();

            Response.Headers["HX-Trigger"] = $"{{\"urnaEleccionChanged\":{{\"idEleccion\":{existente.IdEleccion}}}}}";
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int eleccionId)
        {
            var existente = await _context.UrnasElecciones
                .FirstOrDefaultAsync(ue => ue.IdUrnaEleccion == id && ue.IdEleccion == eleccionId && !ue.Eliminado);

            if (existente == null)
            {
                return NotFound();
            }

            existente.Eliminado = true;
            existente.Activo = false;
            _context.UrnasElecciones.Update(existente);
            await _context.SaveChangesAsync();

            var urnasEleccion = await _context.UrnasElecciones
                .Where(ue => ue.IdEleccion == eleccionId && !ue.Eliminado)
                .OrderBy(ue => ue.IdUrna)
                .ToListAsync();

            var urnas = await _context.Urnas
                .Where(u => !u.Eliminado)
                .OrderBy(u => u.Nombre)
                .ToListAsync();

            ViewBag.IdEleccion = eleccionId;
            ViewBag.UrnasMap = urnas.ToDictionary(u => u.Id_Urna, u => u.Nombre);
            ViewBag.EstatusMap = GetEstatusMap();

            return PartialView(ListaPartialPath, urnasEleccion);
        }

        private async Task CargarCatalogosCreateEdit(int eleccionId, int? idUrnaEleccionActual = null)
        {
            var urnasAsignadas = _context.UrnasElecciones
                .Where(ue => ue.IdEleccion == eleccionId && !ue.Eliminado);

            if (idUrnaEleccionActual.HasValue)
            {
                urnasAsignadas = urnasAsignadas.Where(ue => ue.IdUrnaEleccion != idUrnaEleccionActual.Value);
            }

            var idsUrnasAsignadas = await urnasAsignadas
                .Select(ue => ue.IdUrna)
                .ToListAsync();

            ViewBag.Urnas = await _context.Urnas
                .Where(u => !u.Eliminado && !idsUrnasAsignadas.Contains(u.Id_Urna))
                .OrderBy(u => u.Nombre)
                .ToListAsync();

            if (idUrnaEleccionActual.HasValue)
            {
                var actual = await _context.UrnasElecciones
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ue => ue.IdUrnaEleccion == idUrnaEleccionActual.Value);

                if (actual != null)
                {
                    var urnaActual = await _context.Urnas
                        .FirstOrDefaultAsync(u => u.Id_Urna == actual.IdUrna && !u.Eliminado);

                    if (urnaActual != null)
                    {
                        var urnas = (ViewBag.Urnas as List<Urna>) ?? new List<Urna>();
                        if (!urnas.Any(u => u.Id_Urna == urnaActual.Id_Urna))
                        {
                            urnas.Add(urnaActual);
                            ViewBag.Urnas = urnas.OrderBy(u => u.Nombre).ToList();
                        }
                    }
                }
            }

            ViewBag.Estatus = GetEstatusMap();
        }

        private static Dictionary<int, string> GetEstatusMap()
        {
            return new Dictionary<int, string>
            {
                { 1, "En espera" },
                { 2, "En ejecución" },
                { 3, "Finalizado" }
            };
        }
    }
}
