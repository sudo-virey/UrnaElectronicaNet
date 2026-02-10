using UrnaElectronica.Models;
using System.Security.Cryptography;
using System.Text;

namespace UrnaElectronica.Data
{
    /// <summary>
    /// Implementación simulada del repositorio de usuarios.
    /// NOTA: Esta es una implementación en memoria para desarrollo.
    /// Será reemplazada por una implementación que use Entity Framework cuando se conecte la DB real.
    /// </summary>
    public class UsuarioRepositorySimulado : IUsuarioRepository
    {
        // Base de datos simulada en memoria
        // TODO: Reemplazar con DbContext cuando se conecte la base de datos real
        private static readonly List<Usuario> _usuarios = new()
        {
            new Usuario
            {
                Id = 1,
                Nombre = "Administrador",
                Email = "admin@urna.gob",
                Contraseña = "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=",
                Rol = "Admin",
                Activo = true
            },
            new Usuario
            {
                Id = 2,
                Nombre = "Operador Electoral",
                Email = "operador@urna.gob",
                Contraseña = "o9fTNdI+TL5ghvJwLcCUfuT8OxBNMQE2jNRx0V/3PUg=",
                Rol = "Operador",
                Activo = true
            },
            new Usuario
            {
                Id = 3,
                Nombre = "Supervisor",
                Email = "supervisor@urna.gob",
                Contraseña = "0ZHKqvBMFvABuPjhQKKPzLZrIiPFl1mYQH7iCZkPqQI=",
                Rol = "Supervisor",
                Activo = true
            }
        };

        public Task<List<Usuario>> ObtenerTodosAsync()
        {
            return Task.FromResult(_usuarios.Where(u => u.Activo).ToList());
        }

        public Task<Usuario> ObtenerPorIdAsync(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Id == id && u.Activo);
            return Task.FromResult(usuario);
        }

        public Task<Usuario> ObtenerPorEmailAsync(string email)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Email == email && u.Activo);
            return Task.FromResult(usuario);
        }

        public Task<Usuario> CrearAsync(Usuario usuario)
        {
            usuario.Id = _usuarios.Max(u => u.Id) + 1;
            _usuarios.Add(usuario);
            return Task.FromResult(usuario);
        }

        public Task<Usuario> ActualizarAsync(Usuario usuario)
        {
            var usuarioExistente = _usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (usuarioExistente != null)
            {
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Rol = usuario.Rol;
                usuarioExistente.Activo = usuario.Activo;
            }
            return Task.FromResult(usuarioExistente);
        }

        public Task<bool> EliminarAsync(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
            {
                usuario.Activo = false; // Soft delete
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> ExisteAsync(string email)
        {
            var existe = _usuarios.Any(u => u.Email == email && u.Activo);
            return Task.FromResult(existe);
        }

        /// <summary>
        /// Genera hash SHA256 de una contraseña.
        /// </summary>
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
