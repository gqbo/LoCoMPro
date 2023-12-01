using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Utils
{
    /// <summary>
    /// Clase que representa un modelo que combina información de un registro (Record) y una tienda (Store),
    /// e implementa una lista de las anomalias presentes en esa combinación
    /// </summary>
    public class RecordStoreAnomaliesModel
    {
        public Record Record { get; set; }
        public Store Store { get; set; }
        public IList<Anomalie> Anomalies { get; set; }
        public int recordValoration { get; set; }
        public int generatorValoration { get; set; }
        public IList<int> reporterValorations { get; set; }
    }
}