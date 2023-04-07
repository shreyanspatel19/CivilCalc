using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class StairCaseCalculator
    {

        [Required, Display(Name = "RiserA")]
        //[Range(3, 12, ErrorMessage = "The WallThickness must be between 3 and 12.")]
        public int? RiserA { get; set; }

        [Required, Display(Name = "RiserB")]
        [Range(0, 11, ErrorMessage = "The Length must be between 0 and 11.")]
        public int? RiserB { get; set; }

        [Required, Display(Name = "TreadA")]
        //[Range(3, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? TreadA { get; set; }

        [Required, Display(Name = "TreadB")]
        [Range(0, 11, ErrorMessage = "The Depth must be between 0 and 11.")]
        public int? TreadB { get; set; }

        [Required, Display(Name = "WidthOfStair")]
        //[Range(3, 12, ErrorMessage = "The Depth must be between 3 and 12.")]
        public int? WidthOfStair { get; set; }

        [Required, Display(Name = "HeightOfStair")]
        [Range(0, 11, ErrorMessage = "The Height must be between 0 and 11.")]
        public int? HeightOfStair { get; set; }

        [Required, Display(Name = "WaistSlab")]
        //[Range(3, 999, ErrorMessage = "The Height must be between 3 and 999.")]
        public int? WaistSlab { get; set; }

        public Int32 GradeofConcrete { get; set; }
    }
}
