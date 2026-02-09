using System.ComponentModel.DataAnnotations;

namespace UrnaElectronica.Models
{
    /// <summary>
    /// Configuración general de la urna electrónica
    /// </summary>
    public class ConfiguracionUrna
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string Ubicacion { get; set; } = string.Empty;
        
        [Required]
        public int NumeroMesa { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Departamento { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Municipio { get; set; } = string.Empty;
        
        public bool Activa { get; set; } = true;
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}
