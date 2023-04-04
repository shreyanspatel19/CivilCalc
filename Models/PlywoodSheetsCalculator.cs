using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class PlywoodSheetsCalculator
    {
        public Int32 UnitID { get; set; }

        [Required, Display(Name = "RoomLengthA")]
        public int? RoomLengthA { get; set; }

        [Required, Display(Name = "RoomLengthB")]
        public int? RoomLengthB { get; set; }

        [Required, Display(Name = "RoomWidthA")]
        public int? RoomWidthA { get; set; }

        [Required, Display(Name = "RoomWidthB")]
        public int? RoomWidthB { get; set; }


        [Required, Display(Name = "PlywoodLengthA")]
        public int? PlywoodLengthA { get; set; }

        [Required, Display(Name = "PlywoodLengthB")]
        public int? PlywoodLengthB { get; set; }

        [Required, Display(Name = "PlywoodWidthA")]
        public int? PlywoodWidthA { get; set; }

        [Required, Display(Name = "PlywoodWidthB")]
        public int? PlywoodWidthB { get; set; }
    }
}
