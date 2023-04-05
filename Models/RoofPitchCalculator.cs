using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class RoofPitchCalculator
    {
        public Int32 UnitID { get; set; }

        [Required, Display(Name = "RiseA")]
        public int? RiseA { get; set; }

        [Required, Display(Name = "RiseB")]
        public int? RiseB { get; set; }

        [Required, Display(Name = "RunA")]
        public int? RunA { get; set; }

        [Required, Display(Name = "RunB")]
        public int? RunB { get; set; }
    }
}
