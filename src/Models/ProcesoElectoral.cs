using System.ComponentModel.DataAnnotations;

namespace UrnaElectronica.Models
{
    /// <summary>
    /// Modelo de proceso electoral - almacena procesos electorales
    /// Campos: id_proceso, Nombre, Fecha de creación, id_estatus
    /// </summary>
    public class ProcesoElectoral
    {
        [Key]
        public int Id { get; set; }  // id_proceso
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(255, ErrorMessage = "El nombre no puede exceder 255 caracteres")]
        public string Nombre { get; set; } = string.Empty;
        
        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        
        [Display(Name = "Estado")]
        public int IdEstatus { get; set; } = 1; // 1=Activo, 2=Eliminado, 3=Terminado
        
        public List<Candidato> Candidatos { get; set; } = new List<Candidato>();
    }
    
    /// <summary>
    /// Estados posibles de un proceso electoral
    /// </summary>
    public enum EstatusProcesoElectoral
    {
        Activo = 1,
        Eliminado = 2,
        Terminado = 3
    }
}
