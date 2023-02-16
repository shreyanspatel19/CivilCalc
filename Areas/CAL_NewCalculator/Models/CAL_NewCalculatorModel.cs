using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.CAL_NewCalculator.Models
{
	public class CAL_NewCalculatorModel
	{

        // ModelName: CAL_NewCalculatorModel

        /*******************************************************************
         *	FILTERS
         *******************************************************************/

        public string? F_UserName { get; set; }

        public string? F_CalculatorName { get; set; }



        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "NewCalculatorID")]
        public int NewCalculatorID { get; set; }

        [Required, Display(Name = "CalculatorID")]
        public int CalculatorID { get; set; }

        [Required, Display(Name = "CalculatorName")]
        public string? CalculatorName { get; set; }

        [Required, Display(Name = "Description")]
        public string? Description { get; set; }

        [Required, Display(Name = "Sequence")]
        public decimal Sequence { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
