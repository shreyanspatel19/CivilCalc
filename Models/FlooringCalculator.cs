﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CivilCalc.Models
{
    public class FlooringCalculator
    {

        public Int32 UnitID { get; set; }
        public Int32 Grade { get; set; }

        [Required, Display(Name = "LengthA")]
        public int? LengthA { get; set; }

        [Required, Display(Name = "LengthB")]
        [Range(0, 12, ErrorMessage = "The Length must be between 3 and 12.")]
        public int? LengthB { get; set; }

        [Required, Display(Name = "WidthA")]
        public int? WidthA { get; set; }

        [Required, Display(Name = "WidthB")]
        public int? WidthB { get; set; }

        [Required, Display(Name = "TileLength")]
        public int? TileLength { get; set; }

        [Required, Display(Name = "TileWidth")]
        public int? TileWidth { get; set; }
    }
}