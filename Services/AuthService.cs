using UrnaElectronica.Models;
using UrnaElectronica.Data;
using System.Security.Cryptography;
using System.Text;

namespace UrnaElectronica.Services
{
    /// <summary>
    /// Implementación del servicio de autenticación.
    /// Maneja la validación de credenciales y operaciones de autenticación.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUsuarioRepository usuarioRepository, ILogger<AuthService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        /// <summary>
        /// Valida las credenciales del usuario.
        /// </summary>
        public async Task<Usuario> ValidarCredencialesAsync(string email, string contraseña)
        {
            try
            {
                var usuario = await _usuarioRepository.ObtenerPorEmailAsync(email);

                if (usuario == null)
                {
                    _logger.LogWarning($"Intento de login fallido: usuario no encontrado - {email}");
                    return null;
                }

                if (!EsUsuarioActivo(usuario))
                {
                    _logger.LogWarning($"Intento de login con usuario inactivo - {email}");
                    return null;
                }

                if (!VerifyPassword(contraseña, usuario.Contraseña))
                {
                    _logger.LogWarning($"Intento de login con contraseña incorrecta - {email}");
                    return null;
                }

                _logger.LogInformation($"Login exitoso - {email} ({usuario.Rol})");
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en ValidarCredencialesAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<Usuario> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _usuarioRepository.ObtenerPorIdAsync(id);
        }

        public async Task<Usuario> ObtenerUsuarioPorEmailAsync(string email)
        {
            return await _usuarioRepository.ObtenerPorEmailAsync(email);
        }

        public bool EsUsuarioActivo(Usuario usuario)
        {
            return usuario != null && usuario.Activo;
        }

        /// <summary>
        /// Genera hash SHA256 de una contraseña.
        /// </summary>
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Verifica si una contraseña en texto plano coincide con su hash.
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            _logger.LogInformation($"[DEBUG] Hash ingresado calculado: {hashOfInput}");
            _logger.LogInformation($"[DEBUG] Hash almacenado: {hash}");
            _logger.LogInformation($"[DEBUG] ¿Coinciden?: {hashOfInput == hash}");
            return hashOfInput == hash;
        }
    }
}
