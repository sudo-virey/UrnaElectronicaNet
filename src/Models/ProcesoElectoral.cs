using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrnaElectronica.Models
{
    [Table("Procesos", Schema = "UrnaElectronica")]
    public class ProcesoElectoral
    {
        [Key]
        [Column("Id_Proceso")]
        public int Id { get; set; }  
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)] 
        public string Nombre { get; set; } = string.Empty;
        
        [Column("Id_Estatus_Proceso")]
        public int IdEstatus { get; set; } = 1;

        [Column("Fecha_Creado")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("Activo")]
        public bool Activo { get; set; } = true;

        [Column("Eliminado")]
        public bool Eliminado { get; set; } = false;

        // Mapeo a la columna de SQL para el auto-cierre
        [Column("Fecha_Cerrado")]
        public DateTime? FechaEvento { get; set; }

        public virtual ICollection<Eleccion> Elecciones { get; set; } = new List<Eleccion>();

    }
}