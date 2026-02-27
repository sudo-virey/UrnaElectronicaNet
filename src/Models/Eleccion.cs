using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrnaElectronica.Models
{
    [Table("Elecciones", Schema = "UrnaElectronica")]
    public class Eleccion
    {
        [Key]
        [Column("Id_Eleccion")]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty; // Ej: Alcaldía, Diputación

        [Column("Id_Proceso")]
        public int IdProceso { get; set; }

        [ForeignKey("IdProceso")]
        public virtual ProcesoElectoral? Proceso { get; set; }

        public bool Activo { get; set; } = true;

        public bool Eliminado { get; set; } = false;

        // Aquí colgarían los candidatos (Nivel 3)
        // public virtual ICollection<Candidato> Candidatos { get; set; }
    }
}