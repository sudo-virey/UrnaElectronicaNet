using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Data;

namespace UrnaElectronica.Controllers
{
    public class CandidatoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string ListaPartialPath = "~/Views/Candidatos/_Listar.cshtml";
        private const string CreatePartialPath = "~/Views/Candidatos/_Create.cshtml";
        private const string EditPartialPath = "~/Views/Candidatos/_Edit.cshtml";

        public CandidatoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Aquí irían las acciones para crear, editar y eliminar candidatos

        // GET: Candidato/Create?eleccionId=5

        // GET: Candidato/Listar/5
        public async Task<IActionResult> Listar(int id)
        {
            // Obtenemos los candidatos de la elección con su partido
            var candidatos = await _context.Candidatos
                .Where(c => c.IdEleccion == id && !c.Eliminado)
                .ToListAsync();

            var partidosMap = await _context.Partidos
                .Where(p => !p.Eliminado)
                .ToDictionaryAsync(p => p.Id_Partido, p => p.Nombre);

            // Pasamos el IdEleccion a la vista para el botón "Agregar"
            ViewBag.IdEleccion = id;
            ViewBag.PartidosMap = partidosMap;

            return PartialView(ListaPartialPath, candidatos);
        }


        public IActionResult Create(int eleccionId)
        {
            // Cargamos los partidos para el select del modal
            ViewBag.Partidos = _context.Partidos.Where(p => p.Estado && !p.Eliminado).ToList();
            
            var modelo = new Candidato { IdEleccion = eleccionId };
            return PartialView(CreatePartialPath, modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Candidato modelo, IFormFile? FotoFile)
        {
            if (ModelState.IsValid)
            {
                // 1. Manejo de la subida del Logo/Foto
                if (FotoFile != null && FotoFile.Length > 0)
                {
                    // Definimos la ruta física en el servidor
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/candidatos");
                    
                    // Creamos el directorio si no existe (importante para evitar errores)
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    
                    // Generamos un nombre único para evitar duplicados
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(FotoFile.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FotoFile.CopyToAsync(stream);
                    }
                    
                    // Guardamos la ruta relativa que usaremos en las etiquetas <img>
                    modelo.FotoRuta = "/uploads/candidatos/" + fileName;
                }

                // 2. Valores por defecto según tu modelo
                modelo.Activo = true;
                modelo.Eliminado = false;
                modelo.Registrado = true; 

                _context.Candidatos.Add(modelo);
                await _context.SaveChangesAsync();

                Response.Headers["HX-Trigger"] = $"{{\"candidatoCreado\":{{\"idEleccion\":{modelo.IdEleccion}}}}}";
                return Ok();
            }
            
            ViewBag.Partidos = _context.Partidos.Where(p => p.Estado && !p.Eliminado).ToList();
            return PartialView(CreatePartialPath, modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int eleccionId)
        {
            var candidato = await _context.Candidatos
                .FirstOrDefaultAsync(c => c.Id == id && c.IdEleccion == eleccionId && !c.Eliminado);

            if (candidato == null)
            {
                return NotFound();
            }

            candidato.Eliminado = true;
            candidato.Activo = false;

            _context.Candidatos.Update(candidato);
            await _context.SaveChangesAsync();

            var candidatos = await _context.Candidatos
                .Where(c => c.IdEleccion == eleccionId && !c.Eliminado)
                .ToListAsync();

            var partidosMap = await _context.Partidos
                .Where(p => !p.Eliminado)
                .ToDictionaryAsync(p => p.Id_Partido, p => p.Nombre);

            ViewBag.IdEleccion = eleccionId;
            ViewBag.PartidosMap = partidosMap;
            return PartialView(ListaPartialPath, candidatos);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var candidato = await _context.Candidatos
                .FirstOrDefaultAsync(c => c.Id == id && !c.Eliminado);

            if (candidato == null)
            {
                return NotFound();
            }

            ViewBag.Partidos = _context.Partidos.Where(p => !p.Eliminado).ToList();

            return PartialView(EditPartialPath, candidato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Candidato modelo, IFormFile? FotoFile)
        {
            if (id != modelo.Id)
            {
                return NotFound();
            }

            var candidatoExistente = await _context.Candidatos
                .FirstOrDefaultAsync(c => c.Id == id && !c.Eliminado);

            if (candidatoExistente == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                candidatoExistente.Nombre = modelo.Nombre;
                candidatoExistente.ColorFondo = modelo.ColorFondo;
                candidatoExistente.ColorTexto = modelo.ColorTexto;
                candidatoExistente.IdPartido = modelo.IdPartido;

                if (FotoFile != null && FotoFile.Length > 0)
                {
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/candidatos");

                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(FotoFile.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FotoFile.CopyToAsync(stream);
                    }

                    candidatoExistente.FotoRuta = "/uploads/candidatos/" + fileName;
                }

                _context.Candidatos.Update(candidatoExistente);
                await _context.SaveChangesAsync();

                Response.Headers["HX-Trigger"] = $"{{\"candidatoActualizado\":{{\"idEleccion\":{candidatoExistente.IdEleccion}}}}}";
                return Ok();
            }

            modelo.IdEleccion = candidatoExistente.IdEleccion;
            modelo.FotoRuta = candidatoExistente.FotoRuta;
            ViewBag.Partidos = _context.Partidos.Where(p => !p.Eliminado).ToList();
            return PartialView(EditPartialPath, modelo);
        }
    }
}