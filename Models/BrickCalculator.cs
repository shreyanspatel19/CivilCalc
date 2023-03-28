using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class BrickCalculator
    {
        public string? ddlUnit { get; set; }

        //public Int32 UnitID { get; set; }
        public string? ddlWallThickness { get; set; }
        public string? ddlRatio { get; set; }

        [Required, Display(Name = "WallThickness")]
        //[Range(3, 12, ErrorMessage = "The WallThickness must be between 3 and 12.")]
        public int? txtOtherWallThickness { get; set; }

        [Required, Display(Name = "LengthA")]
        //[Range(3, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? txtWallLengthA { get; set; }

        [Required, Display(Name = "LengthB")]
        [Range(0, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? txtWallLengthB { get; set; }

        [Required, Display(Name = "DepthA")]
        //[Range(3, 12, ErrorMessage = "The Depth must be between 3 and 12.")]
        public int? txtWallDepthA { get; set; }

        [Required, Display(Name = "DepthB")]
        [Range(0, 12, ErrorMessage = "The Depth must be between 3 and 12.")]
        public int? txtWallDepthB { get; set; }

        [Required, Display(Name = "LengthBrick")]
        //[Range(3, 99, ErrorMessage = "LengthBrick")]
        public int? txtLengthBrick { get; set; }

        [Required, Display(Name = "WidthBrick")]
        //[Range(3, 999, ErrorMessage = "The Height must be between 3 and 999.")]
        public int? txtWidthBrick { get; set; }

        [Required, Display(Name = "HeightBrick")]
        //[Range(3, 12, ErrorMessage = "The Height must be between 3 and 12.")]
        public int? txtHeightBrick { get; set; }
    }
}
