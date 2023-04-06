using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class PrecastBoundaryWallCalculator
    {

        public Int32 UnitID { get; set; }

        [Required, Display(Name = "LengthA")]
        public int? LengthA { get; set; }

        [Required, Display(Name = "LengthB")]
        [Range(0, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? LengthB { get; set; }

        [Required, Display(Name = "HeightA")]
        public int? HeightA { get; set; }

        [Required, Display(Name = "HeightB")]
        public int? HeightB { get; set; }

        [Required, Display(Name = "LengthofBarA")]
        public int? LengthofBarA { get; set; }

        [Required, Display(Name = "LengthofBarB")]
        public int? LengthofBarB { get; set; }

        [Required, Display(Name = "HeightofBarA")]
        public int? HeightofBarA { get; set; }

        [Required, Display(Name = "HeightofBarB")]
        public int? HeightofBarB { get; set; }
    }
}
