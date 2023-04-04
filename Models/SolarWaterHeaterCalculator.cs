using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class SolarWaterHeaterCalculator
    {

        [Required, Display(Name = "Nos")]
        public int? Nos { get; set; }
    }
}
