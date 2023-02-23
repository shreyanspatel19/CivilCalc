using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.CAL_Category.Models
{
    public class CAL_CategoryModel
    {
        // ModelName: CAL_CatagoryModel

        /*******************************************************************
         *	DROP-DOWN
         *******************************************************************/
        public class CAL_CategoryComboBoxModel
        {
            public int CategoryID { get; set; }

            public string? CategoryName { get; set; }
        }


        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "CatagoryID")]
        public int CategoryID { get; set; }
        public int CalculatorID { get; set; }

        [Required, Display(Name = "CatagoryName")]
        public string? CategoryName { get; set; }

        public string? Description { get; set; }

        public decimal Sequence { get; set; }

        [Required, Display(Name = "User")]
        public int UserID { get; set; }

        [Required, Display(Name = "UserName")]
        public string? UserName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }
}
