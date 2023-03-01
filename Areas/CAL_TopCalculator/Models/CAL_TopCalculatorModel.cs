using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.CAL_TopCalculator.Models
{
    public class CAL_TopCalculatorModel
    {

        // ModelName: CAL_TopCalculatorModel

        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "TopCalculatorID")]
        public int TopCalculatorID { get; set; }

        [Required, Display(Name = "CalculatorID")]
        public int CalculatorID { get; set; }

        [Required, Display(Name = "CalculatorName")]
        public string? CalculatorName { get; set; }

        public string? Description { get; set; }
        [ Display(Name = "HeaderName")]
        public decimal Sequence { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }
}
