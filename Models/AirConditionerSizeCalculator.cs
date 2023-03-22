using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class AirConditionerSizeCalculator
    {

        //[Required, Display(Name = "WallThickness")]
        //[Range(3, 12, ErrorMessage = "The WallThickness must be between 3 and 12.")]
        public int? txtLengthA { get; set; }

        //[Required, Display(Name = "LengthA")]
        //[Range(3, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? txtLengthB { get; set; }

        //[Required, Display(Name = "LengthB")]
        //[Range(3, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? txtBreadthA { get; set; }

        //[Required, Display(Name = "DepthA")]
        //[Range(3, 12, ErrorMessage = "The Depth must be between 3 and 12.")]
        public int? txtBreadthB { get; set; }

        //[Required, Display(Name = "DepthB")]
        //[Range(3, 12, ErrorMessage = "The Depth must be between 3 and 12.")]
        public int? txtHeightA { get; set; }

        //[Required, Display(Name = "LengthBrick")]
        //[Range(3, 99, ErrorMessage = "yes")]
        public int? txtHeightB { get; set; }

        //[Required, Display(Name = "WidthBrick")]
        //[Range(3, 999, ErrorMessage = "The Height must be between 3 and 999.")]
        public int? txtNoofPerson { get; set; }

        //[Required, Display(Name = "HeightBrick")]
        //[Range(3, 12, ErrorMessage = "The Height must be between 3 and 12.")]
        public int? txttemperature { get; set; }
    }
}
