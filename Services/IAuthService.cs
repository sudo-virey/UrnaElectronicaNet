using UrnaElectronica.Models;

namespace UrnaElectronica.Services
{
    /// <summary>
    /// Interfaz para servicio de autenticación.
    /// Define los métodos disponibles para autenticación de usuarios.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Valida las credenciales del usuario contra la base de datos.
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <param name="contraseña">Contraseña en texto plano</param>
        /// <returns>Usuario si las credenciales son válidas, null en caso contrario</returns>
        Task<Usuario> ValidarCredencialesAsync(string email, string contraseña);

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        Task<Usuario> ObtenerUsuarioPorIdAsync(int id);

        /// <summary>
        /// Obtiene un usuario por su email.
        /// </summary>
        Task<Usuario> ObtenerUsuarioPorEmailAsync(string email);

        /// <summary>
        /// Verifica si el usuario está activo.
        /// </summary>
        bool EsUsuarioActivo(Usuario usuario);
    }
}
