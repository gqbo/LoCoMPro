namespace LoCoMPro_LV.Utils
{
    public class InfoTopReportsUser
    {
        /// <summary>
        /// Almacena la cantidad de registros de un usuario.
        /// </summary>
        public int RecordsCount { get; set; }

        /// <summary>
        /// Almacena la cantidad de reportes aceptados.
        /// </summary>
        public int AcceptReportsCount { get; set; }

        /// <summary>
        /// Almacena el porcentaje relacionado a la cantidad de reportes aceptados.
        /// </summary>
        public double PorcentageReport { get; set; }

        /// <summary>
        /// Almacena el nombre de usuario.
        /// </summary>
        public string NameGenerator { get; set; }

        /// <summary>
        /// Almacena la cantidad de reportes de un usuario.
        /// </summary>
        public int TotalReports { get; set; }

        public InfoTopReportsUser(int RecordCount, int ReportsCount, double Porcetange, string NameGeneratorUser, int TotalReportsUser)
        {
            RecordsCount = RecordCount;
            AcceptReportsCount = ReportsCount;
            PorcentageReport = Porcetange;
            NameGenerator = NameGeneratorUser;
            TotalReports = TotalReportsUser;
        }
    }
}
