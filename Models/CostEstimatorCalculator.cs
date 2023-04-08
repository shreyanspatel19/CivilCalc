using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class CostEstimatorCalculator
    {
        public int? MeBuiltupmberType { get; set; }

        [Required, Display(Name = "ApproxCost")]
        public int? ApproxCost { get; set; }
    }
}
