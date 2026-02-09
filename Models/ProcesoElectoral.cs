using System.ComponentModel.DataAnnotations;

namespace UrnaElectronica.Models
{
    /// <summary>
    /// Modelo de proceso electoral
    /// </summary>
    public class ProcesoElectoral
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        
        [Required]
        public DateTime FechaInicio { get; set; }
        
        [Required]
        public DateTime FechaFin { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, En Proceso, Completado
        
        public List<Candidato> Candidatos { get; set; } = new List<Candidato>();
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
