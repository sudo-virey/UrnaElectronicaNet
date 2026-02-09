using System.ComponentModel.DataAnnotations;

namespace UrnaElectronica.Models
{
    /// <summary>
    /// Modelo de candidato para elecciones
    /// </summary>
    public class Candidato
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Partido { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Posicion { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        
        public bool Activo { get; set; } = true;
        
        public int Orden { get; set; }
    }
}
