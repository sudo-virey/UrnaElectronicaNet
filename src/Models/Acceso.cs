using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrnaElectronica.Models
{
    public class Acceso
    {
        [Key]
        public int Id_Acceso { get; set; }

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        public int Id_Urna { get; set; }

        [ForeignKey("Id_Urna")]
        public Urna? Urna { get; set; }

        public bool Activo { get; set; }
        
        public bool Eliminado { get; set; }
    }
}