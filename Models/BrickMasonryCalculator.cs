namespace CivilCalc.Models
{
    public class BrickMasonryCalculator
    {
        public string? ddlUnit { get; set; }
        public string? ddlWallThickness { get; set; }
        public string? ddlRatio { get; set; }
        public int LengthInch { get; set; }
        public int? txtOtherWallThickness { get; set; } 

        public int? txtWallLengthA { get; set; } 
        public int? txtWallLengthB { get; set; }
        public int? txtWallDepthA { get; set; }
        public int? txtWallDepthB { get; set; }
        public int? HeightInch { get; set; }
        public int? txtLengthBrick { get; set; }
        public int? txtWidthBrick { get; set; }
        public int? txtHeightBrick { get; set; }
    }
}
