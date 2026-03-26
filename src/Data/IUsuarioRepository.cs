using UrnaElectronica.Models;

namespace UrnaElectronica.Data
{
    /// <summary>
    /// Interfaz para acceso a datos de usuarios.
    /// Define los métodos de repositorio para la entidad Usuario.
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Obtiene todos los usuarios.
        /// </summary>
        Task<List<Usuario>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un usuario por ID.
        /// </summary>
        Task<Usuario?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Obtiene un usuario por email.
        /// </summary>
        Task<Usuario> ObtenerPorEmailAsync(string email);

        /// <summary>
        /// Agrega un nuevo usuario.
        /// </summary>
        Task<Usuario> CrearAsync(Usuario usuario);

        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        Task<Usuario> ActualizarAsync(Usuario usuario);

        /// <summary>
        /// Elimina un usuario.
        /// </summary>
        Task<bool> EliminarAsync(int id);

        /// <summary>
        /// Verifica si un usuario existe por email.
        /// </summary>
        Task<bool> ExisteAsync(string email);
    }
}
