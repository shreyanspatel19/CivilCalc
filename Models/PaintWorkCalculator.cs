using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class PaintWorkCalculator
    {

        public Int32 UnitID { get; set; }

        [Required, Display(Name = "Carpet_Area")]
        public int? Carpet_Area { get; set; }

        [Required, Display(Name = "DoorWidth")]
        public int? DoorWidth { get; set; }

        [Required, Display(Name = "DoorHeight")]
        public int? DoorHeight { get; set; }

        [Required, Display(Name = "NoofDoors")]
        public int? NoofDoors { get; set; }

        [Required, Display(Name = "WindowWidth")]
        public int? WindowWidth { get; set; }

        [Required, Display(Name = "WindowHeight")]
        public int? WindowHeight { get; set; }

        [Required, Display(Name = "NoofWindows")]
        public int? NoofWindows { get; set; }
    }
}
