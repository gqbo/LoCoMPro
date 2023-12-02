using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Utils
{
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