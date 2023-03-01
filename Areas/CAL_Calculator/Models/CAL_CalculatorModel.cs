using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Areas.CAL_Calculator.Models
{
    public class CAL_CalculatorModel
    {
        // ModelName: CAL_CalculatorModel
        /*******************************************************************
          *	DROP-DOWN FORM
        *******************************************************************/
        public class CAL_CalculatorComboBoxModel
        {
            public int CalculatorID { get; set; }
            public string? CalculatorName { get; set; }
        }
        /*******************************************************************
         *	ADDEDIT FORM
         *******************************************************************/
        [Required, Display(Name = "CalculatorID")]
        public int CalculatorID { get; set; }

        [Required, Display(Name = "CategoryID")]
        public int CategoryID { get; set; }

        [Required, Display(Name = "CalculatorName")]
        public string? CalculatorName { get; set; }

        [Required, Display(Name = "CalculatorIcon")]
        public string? CalculatorIcon { get; set; }

        public IFormFile? File { get; set; }

        [Required, Display(Name = "URLName")]
        public string? URLName { get; set; }

        [Required, Display(Name = "HeaderName")]
        public string? HeaderName { get; set; }

        public string? SubHeaderName { get; set; }

        public string? CalculatorDescription { get; set; }

        public string? PageSection1 { get; set; }

        public string? PageSection2 { get; set; }

        public string? PageSection3 { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeyword { get; set; }

        public string? MetaDescription { get; set; }

        public string? MetaAuthor { get; set; }

        public string? MetaOgTitle { get; set; }

        [Required, Display(Name = "MetaOgImage")]
        public string? MetaOgImage { get; set; }
        public IFormFile? MetaOgFile { get; set; }

        public string? MetaOgDescription { get; set; }

        public string? MetaOgUrl { get; set; }

        public string? MetaOgType { get; set; }

        [Display(Name = "Sequence")]
        public decimal Sequence { get; set; }

        public string? Description { get; set; }

        [Required, Display(Name = "UserID")]
        public int UserID { get; set; }

        [Required, Display(Name = "UserName")]
        public string? UserName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }
}

