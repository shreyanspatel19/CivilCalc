using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class SteelWeightCalculator
    {
        public Int32 UnitID { get; set; }

        [Required, Display(Name = "Length")]
        public int? LengthA { get; set; }

        [Required, Display(Name = "Length")]
        [Range(0, 11, ErrorMessage = "The Length must be between 0 and 11.")]
        public int? LengthB { get; set; }

        [Required, Display(Name = "Diameter")]
        public int? Diameter { get; set; }

        [Required, Display(Name = "Quantity")]
        [Range(0, 11, ErrorMessage = "The Quantity must be between 0 and 11.")]
        public int? Quantity { get; set; }
    }
}
