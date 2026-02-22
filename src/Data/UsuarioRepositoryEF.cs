using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Models;

namespace UrnaElectronica.Data
{
    public class UsuarioRepositoryEF : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepositoryEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            // Ahora busca en las tablas reales de SQL Server
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Activo);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios.Where(u => u.Activo).ToListAsync();
        }

        public async Task<Usuario> CrearAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> ActualizarAsync(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            usuario.Activo = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email && u.Activo);
        }
    }
}