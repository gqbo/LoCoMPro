using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Pages.Reports
{
    public class RecordStoreReportModel
    {
        public Record Record { get; set; }
        public Store Store { get; set; }
        public IList<Report> Reports { get; set;}
        public int recordValoration { get; set; }
        public int generatorValoration { get; set; }
        public int reporterValoration { get; set; }
    }
}