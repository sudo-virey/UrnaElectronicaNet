using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrnaElectronica.Models
{
    [Table("Urnas_Elecciones", Schema = "UrnaElectronica")]
    public class UrnaEleccion
    {
        [Key]
        [Column("Id_Urna_Eleccion")]
        public int IdUrnaEleccion { get; set; }

        [Column("Lista_Nominal")]
        public int ListaNominal { get; set; }

        [Column("Id_Eleccion")]
        public int IdEleccion { get; set; }

        [Column("Id_Urna")]
        public int IdUrna { get; set; }

        [Column("Id_Estatus_Urna")]
        public int IdEstatusUrna { get; set; } = 1;

        [Column("Fecha_Hora_Inicio")]
        public DateTime? FechaHoraInicio { get; set; }

        [Column("Fecha_Hora_Fin")]
        public DateTime? FechaHoraFin { get; set; }

        public bool Activo { get; set; } = true;

        public bool Eliminado { get; set; } = false;
    }
}
