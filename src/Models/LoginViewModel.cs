using System.ComponentModel.DataAnnotations;

namespace UrnaElectronica.Models
{
    /// <summary>
    /// Modelo para el formulario de login
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 255 caracteres")]
        public string Contraseña { get; set; } = string.Empty;
        
        [Display(Name = "Recuérdame")]
        public bool RecuérdameBinding { get; set; }
    }
}
