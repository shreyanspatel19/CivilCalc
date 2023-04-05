using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class SolarRoofTopCalculator
    {
        public Int32 ConsuptionType { get; set; }

        [Required, Display(Name = "Units")]
        public int? Units { get; set; }
    }
}
