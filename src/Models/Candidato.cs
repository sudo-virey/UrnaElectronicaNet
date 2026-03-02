using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Candidato
{
    [Key]
    [Column("Id_Candidato")]
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;
    
    [Column("Color_Texto")]
    public string? ColorTexto { get; set; }
    
    [Column("Color_Fondo")]
    public string? ColorFondo { get; set; }

    [Column("Logo")]
    public string? FotoRuta { get; set; } // Ruta de la imagen
    
    public bool Registrado { get; set; } = false;

    [Column("Id_Partido")]
    public int? IdPartido { get; set; } // Relación con la tabla Part

    [Column("Id_Eleccion")]
    public int IdEleccion { get; set; } // Relación con la tabla Ele

    public bool Activo { get; set; } = true;
    
    public bool Eliminado { get; set; } = false;   
}