using System.ComponentModel.DataAnnotations;

namespace UrnaElectronica.Models
{
    /// <summary>
    /// Modelo de partido para elecciones
    /// </summary>
public class Partido
{
   [Key] // <--- Esto le dice a EF: "Esta es mi Primary Key"
    public int Id_Partido { get; set; }
    public string Nombre { get; set; }
    public string Siglas { get; set; }
    public string Tipo { get; set; } // Partido, Coalición, Independiente
    public bool Estado { get; set; }
    public bool Eliminado { get; set; }
}
}