using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class AirConditionerSizeCalculator
    {

        [Required, Display(Name = "Length")]
        //[Range(3, 12, ErrorMessage = "The WallThickness must be between 3 and 12.")]
        public int? txtLengthA { get; set; }

        [Required, Display(Name = "Length")]
        [Range(0, 11, ErrorMessage = "The Length must be between 0 and 11.")]
        public int? txtLengthB { get; set; }

        [Required, Display(Name = "Breadth")]
        //[Range(3, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? txtBreadthA { get; set; }

        [Required, Display(Name = "Breadth")]
        [Range(0, 11, ErrorMessage = "The Depth must be between 0 and 11.")]
        public int? txtBreadthB { get; set; }

        [Required, Display(Name = "Height")]
        //[Range(3, 12, ErrorMessage = "The Depth must be between 3 and 12.")]
        public int? txtHeightA { get; set; }

        [Required, Display(Name = "Height")]
        [Range(0, 11, ErrorMessage = "The Height must be between 0 and 11.")]
        public int? txtHeightB { get; set; }

        [Required, Display(Name = "NoofPerson")]
        //[Range(3, 999, ErrorMessage = "The Height must be between 3 and 999.")]
        public int? txtNoofPerson { get; set; }

        [Required, Display(Name = "temperature")]
        //[Range(3, 12, ErrorMessage = "The Height must be between 3 and 12.")]
        public int? txttemperature { get; set; }
    }
}
