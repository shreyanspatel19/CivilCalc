using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.SEC_User.Models
{
    public class SEC_UserModel
    {
        // ModelName: CAL_CatagoryModel

        /*******************************************************************
         *	FILTERS
         *******************************************************************/
       
        public string? F_UserName { get; set; }

        



        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "CatagoryID")]
        public int UserID { get; set; }

        [Required, Display(Name = "CatagoryName")]
        public string? UserName { get; set; }

        [Required, Display(Name = "Password")]
        public string? Password { get; set; }

        [Required, Display(Name = "Email")]
        public string? Email { get; set; }

        [Required, Display(Name = "MobileNo")]
        public string? MobileNo { get; set; }

        [Required, Display(Name = "DisplayName")]
        public string? DisplayName { get; set; }

        [Required, Display(Name = "CreatedByUserID")]
        public int CreatedByUserID { get; set; }

        [Required, Display(Name = "Description")]
        public string? Description { get; set; }

        [Required, Display(Name = "IsActive")]
        public Boolean IsActive { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
