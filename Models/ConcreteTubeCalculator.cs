using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class ConcreteTubeCalculator
    {

        public Int32 UnitID { get; set; }
        public Decimal GradeID { get; set; }

        [Required, Display(Name = "InnerDiameterA")]
        public int? InnerDiameterA { get; set; }

        [Required, Display(Name = "InnerDiameterB")]
        public int? InnerDiameterB { get; set; }

        [Required, Display(Name = "OuterDiameterA")]
        public int? OuterDiameterA { get; set; }

        [Required, Display(Name = "OuterDiameterB")]
        public int? OuterDiameterB { get; set; }

        [Required, Display(Name = "HeightA")]
        public int? HeightA { get; set; }

        [Required, Display(Name = "HeightB")]
        public int? HeightB { get; set; }

        [Required, Display(Name = "ColumnNo")]
        public int? ColumnNo { get; set; }

    }
}
