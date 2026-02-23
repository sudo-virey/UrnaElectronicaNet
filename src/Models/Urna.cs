using System.ComponentModel.DataAnnotations;

namespace UrnaElectronica.Models
{
    public class Urna
    {
        [Key]
        public int Id_Urna { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Modelo { get; set; } = string.Empty;

        public bool Activo { get; set; }
        
        public bool Eliminado { get; set; } = false;
    }
}