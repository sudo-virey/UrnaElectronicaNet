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

            // Pasamos el IdEleccion a la vista para el botón "Agregar"
            ViewBag.IdEleccion = id;

            return PartialView(ListaPartialPath, candidatos);
        }


        public IActionResult Create(int eleccionId)
        {
            // Cargamos los partidos para el select del modal
            ViewBag.Partidos = _context.Partidos.Where(p => p.Estado).ToList();
            
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
            
            ViewBag.Partidos = _context.Partidos.ToList();
            return PartialView(CreatePartialPath, modelo);
        }
    }
}