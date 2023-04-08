using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.CAL_CalculatorContent.Models
{
    public class CAL_CalculatorContentModel
    {
        public int CalculatorContentID { get; set; }
        public int CalculatorID { get; set; }
        public string? PageContent  { get; set; }

        [Display(Name = "Sequence")]
        public decimal Sequence { get; set; }
        public string? Description { get; set; }
        public int UserID { get; set; }
    }
}
