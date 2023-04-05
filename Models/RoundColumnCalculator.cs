using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class RoundColumnCalculator
    {

        public Int32 UnitID { get; set; }
        public Decimal GradeID { get; set; }

        [Required, Display(Name = "DiameterA")]
        public int? DiameterA { get; set; }

        [Required, Display(Name = "DiameterB")]
        public int? DiameterB { get; set; }

        [Required, Display(Name = "HeightA")]
        public int? HeightA { get; set; }

        [Required, Display(Name = "HeightB")]
        public int? HeightB { get; set; }

        [Required, Display(Name = "ColumnNo")]
        public int? ColumnNo { get; set; }

    }
}
