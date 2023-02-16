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

        public string? F_UserName { get; set; }



        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "ConfigurationID")]
        public string? ConfigurationID { get; set; }

        [Required, Display(Name = "WebsiteLogoPath")]
        public string? WebsiteLogoPath { get; set; }

        [Required, Display(Name = "MetaTitle")]
        public string? MetaTitle { get; set; }

        [Required, Display(Name = "MetaKeyword")]
        public string? MetaKeyword { get; set; }

        [Required, Display(Name = "MetaDescription")]
        public string? MetaDescription { get; set; }

        [Required, Display(Name = "MetaAuthor")]
        public string? MetaAuthor { get; set; }

        [Required, Display(Name = "MetaOgTitle")]
        public string? MetaOgTitle { get; set; }

        [Required, Display(Name = "MetaOgImage")]
        public string? MetaOgImage { get; set; }

        [Required, Display(Name = "MetaOgDescription")]
        public string? MetaOgDescription { get; set; }

        [Required, Display(Name = "MetaOgUrl")]
        public string? MetaOgUrl { get; set; }

        [Required, Display(Name = "MetaOgType")]
        public string? MetaOgType { get; set; }

        [Required, Display(Name = "AboutWebsite")]
        public string? AboutWebsite { get; set; }

        [Required, Display(Name = "MobileAppDetail")]
        public string? MobileAppDetail { get; set; }

        [Required, Display(Name = "PlayStoreURL")]
        public string? PlayStoreURL { get; set; }

        [Required, Display(Name = "AppStoreURL")]
        public string? AppStoreURL { get; set; }

        [Required, Display(Name = "FooterHTML")]
        public string? FooterHTML { get; set; }

        [Required, Display(Name = "Description")]
        public string? Description { get; set; }

        [Required, Display(Name = "UserID")]
        public int? UserID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
