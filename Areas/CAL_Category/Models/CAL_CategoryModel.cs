using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.CAL_Category.Models
{
    public class CAL_CategoryModel
    {
        // ModelName: CAL_CatagoryModel

        /*******************************************************************
         *	FILTERS
         *******************************************************************/
        public string? C_CategoryId { get; set; }

        public string? C_CategoryName { get; set; }



        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "CatagoryID")]
        public int CategoryID { get; set; }

        [Required, Display(Name = "CatagoryName")]
        public string? CategoryName { get; set; }

        [Required, Display(Name = "Description")]
        public string? Description { get; set; }

        [Required, Display(Name = "Sequence")]
        public int Sequence { get; set; }

        [Required, Display(Name = "User")]
        public int UserID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }    }
}
