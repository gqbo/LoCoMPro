namespace LoCoMPro_LV.Utils;
using System.ComponentModel.DataAnnotations;
public class InfoTopReportsUser
{
    /// <summary>
    /// Almacena la cantidad de registros de un usuario.
    /// </summary>
    [Display(Name = "Registros realizados")]
    public int RecordsCount { get; set; }

    /// <summary>
    /// Almacena la cantidad de reportes recibidos de un usuario.
    /// </summary>
    [Display(Name = "Reportes recibidos")]
    public int ReportsReceived { get; set; }

    /// <summary>
    /// Almacena la cantidad de reportes realizados de un usuario.
    /// </summary>
    [Display(Name = "Reportes realizados")]
    public int ReportsMade { get; set; }

    /// <summary>
    /// Almacena la cantidad de reportes aceptados.
    /// </summary>
    [Display(Name = "Reportes aceptados")]
    /// 
    public int AcceptedReportsCount { get; set; }

    /// <summary>
    /// Almacena el porcentaje relacionado a la cantidad de reportes aceptados.
    /// </summary>
    public double AcceptedReportsPercentage { get; set; }

    /// <summary>
    /// Almacena el nombre de usuario.
    /// </summary>
    [Display(Name = "Usuario")]
    public string NameGenerator { get; set; }

    /// <summary>
    /// Almacena la valoración de un usuario basado en estrellas.
    /// </summary>
    [Display(Name = "Valoración")]
    public int UserRating { get; set; }

    public InfoTopReportsUser(int RecordsCount, int ReportsReceived, int ReportsMade, int AcceptedReportsCount, double AcceptedReportsPercentage, string NameGenerator, int UserRating)
    {
        this.RecordsCount = RecordsCount;
        this.ReportsReceived = ReportsReceived;
        this.ReportsMade = ReportsMade;
        this.AcceptedReportsCount = AcceptedReportsCount;
        this.AcceptedReportsPercentage = AcceptedReportsPercentage;
        this.NameGenerator = NameGenerator;
        this.UserRating = UserRating;
    }
}
