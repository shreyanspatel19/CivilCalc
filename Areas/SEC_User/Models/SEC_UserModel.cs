﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.SEC_User.Models
{
    public class SEC_UserModel
    {
        // ModelName: CAL_CatagoryModel

        /*******************************************************************
         *	FILTERS
         *******************************************************************/

        public class SEC_UserComboBoxModel
        {
            public int UserID { get; set; }

            public string? UserName { get; set; }
        }

        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "UserID")]
        public int UserID { get; set; }

        [Required, Display(Name = "UserName")]
        public string? UserName { get; set; }

        [Required, Display(Name = "Password")]
        public string? Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? MobileNo { get; set; }

        public string? DisplayName { get; set; }

        [Required, Display(Name = "CreatedByUserID")]
        public int CreatedByUserID { get; set; }

        public string? Description { get; set; }

        public Boolean IsActive { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public string? F_UserName { get; set; }


    }
}
