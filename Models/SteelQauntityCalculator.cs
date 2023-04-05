using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class SteelQauntityCalculator
    {
        public Int32 MemberType { get; set; }

        [Required, Display(Name = "LengthA")]
        public int? ConcreteQauntity { get; set; }
    }
}
