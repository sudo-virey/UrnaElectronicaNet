using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrnaElectronica.Models
{
    [Table("Cat_Estatus_Urna", Schema = "UrnaElectronica")]
    public class CatEstatusUrna
    {
        [Key]
        [Column("Id_Estatus_Urna")]
        public int IdEstatusUrna { get; set; }

        [Column("Estatus")]
        public string Estatus { get; set; } = string.Empty;
    }
}
