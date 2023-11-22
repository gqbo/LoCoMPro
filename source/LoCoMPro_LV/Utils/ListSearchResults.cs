using LoCoMPro_LV.Models;

namespace LoCoMPro_LV.Utils
{
    public class ListSearchResults
    {
        public Store Store { get; set; }
        public int productCount { get; set; }
        public double? totalPrice { get; set; } = 0;
        public IList<Record> Records { get; set; }
    }
}
