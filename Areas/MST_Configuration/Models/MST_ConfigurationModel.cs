using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.MST_Configuration.Models
{
    public class MST_ConfigurationModel
    {
        // ModelName: CAL_TopCalculatorModel

        /*******************************************************************
         *	FILTERS
         *******************************************************************/

       



        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "ConfigurationID")]
        public int ConfigurationID { get; set; }

        [Required, Display(Name = "WebsiteLogoPath")]
        public string? WebsiteLogoPath { get; set; }

        [Required, Display(Name = "MetaTitle")]
        public string? MetaTitle { get; set; }

        public string? MetaKeyword { get; set; }

        public string? MetaDescription { get; set; }

        public string? MetaAuthor { get; set; }

        public string? MetaOgTitle { get; set; }

        public string? MetaOgImage { get; set; }

        public string? MetaOgDescription { get; set; }

        public string? MetaOgUrl { get; set; }

        public string? MetaOgType { get; set; }

        public string? AboutWebsite { get; set; }

        public string? MobileAppDetail { get; set; }

        public string? PlayStoreURL { get; set; }

        public string? AppStoreURL { get; set; }

        public string? FooterHTML { get; set; }

        public string? Description { get; set; }

        [Required, Display(Name = "UserID")]
        public int UserID { get; set; }

        public string? UserName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
