using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class CarpetAreaCalculator
    {
        public string? lType { get; set; }

        [Required, Display(Name = "LengthFeet")]
        public int? LengthFeet { get; set; }

        [Required, Display(Name = "LengthInche")]
        public int? LengthInche { get; set; }

        [Required, Display(Name = "lengthFeet")]
        public int? lengthFeet { get; set; }

        [Required, Display(Name = "BreadthFeet")]
        public int? BreadthFeet { get; set; }

        [Required, Display(Name = "BreadthInches")]
        public int? BreadthInches { get; set; }
    }
}
