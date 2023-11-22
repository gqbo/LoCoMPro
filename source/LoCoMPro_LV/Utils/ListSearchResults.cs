using LoCoMPro_LV.Models;
using System.ComponentModel.DataAnnotations;

namespace LoCoMPro_LV.Utils
{
    public class ListSearchResults
    {
        public Store Store { get; set; }
        public int productCount { get; set; }
        public double percentageInList { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public double? totalPrice { get; set; } = 0;
        public IList<Record> Records { get; set; }
        public double Distance { get; set; }
    }
}
