using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class ConcreteBlockCalculator
    {

        public Int32 UnitID { get; set; }
        public Decimal WallThicknessID { get; set; }
        public Int32 RatioID { get; set; }

        [Required, Display(Name = "WallThickness")]
        //[Range(3, 12, ErrorMessage = "The WallThickness must be between 3 and 12.")]
        public int? OtherWallThickness { get; set; }

        [Required, Display(Name = "LengthA")]
        //[Range(3, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? WallLengthA { get; set; }

        [Required, Display(Name = "LengthB")]
        [Range(0, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? WallLengthB { get; set; }

        [Required, Display(Name = "DepthA")]
        //[Range(3, 12, ErrorMessage = "The Depth must be between 3 and 12.")]
        public int? WallDepthA { get; set; }

        [Required, Display(Name = "DepthB")]
        [Range(0, 12, ErrorMessage = "The Depth must be between 3 and 12.")]
        public int? WallDepthB { get; set; }

        [Required, Display(Name = "LengthBrick")]
        //[Range(3, 99, ErrorMessage = "LengthBrick")]
        public int? LengthBrick { get; set; }

        [Required, Display(Name = "WidthBrick")]
        //[Range(3, 999, ErrorMessage = "The Height must be between 3 and 999.")]
        public int? WidthBrick { get; set; }

        [Required, Display(Name = "HeightBrick")]
        //[Range(3, 12, ErrorMessage = "The Height must be between 3 and 12.")]
        public int? HeightBrick { get; set; }
    }
}
