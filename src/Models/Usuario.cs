using System.ComponentModel.DataAnnotations;

namespace UrnaElectronica.Models
{
    /// <summary>
    /// Modelo de usuario para autenticación
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Contraseña { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Rol { get; set; } = "Usuario"; // Admin, Operador, Usuario
        
        public bool Activo { get; set; } = true;
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
