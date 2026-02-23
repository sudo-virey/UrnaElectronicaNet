using System.ComponentModel.DataAnnotations;

namespace UrnaElectronica.Models
{
    public class Impresora
    {
        [Key]
        public int Id_Impresora { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección MAC es obligatoria")]
        public string Mac { get; set; } = string.Empty;

        public bool Activo { get; set; }
        
        // Campo para borrado lógico
        public bool Eliminado { get; set; } = false;
    }
}