using System.ComponentModel.DataAnnotations;

namespace CivilCalc.Areas.LOG_Calculation.Models
{
    public class LOG_CalculationModel
    {
        [Required , Display(Name = "CalculationID")]
        public int CalculationID { get; set; }
        public string? ScreenName  { get; set; }
        public string? ParamA { get; set; }
        public string? ParamB { get; set; }
        public string? ParamC { get; set; }
        public string? ParamD { get; set; }
        public string? ParamE { get; set; }
        public string? ParamF { get; set; }
        public string? ParamG { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string? ParamH { get; set; }
        public string? ParamI { get; set; }
        public string? ParamJ { get; set; }
        public string? ParamK { get; set; }
        public string? ParamL { get; set; }
        public string? ParamM { get; set; }
        public string? ParamN { get; set; }
        public string? ParamO { get; set; }
        public string? ParamP { get; set; }
        public string? ParamQ { get; set; }
        public string? ParamR { get; set; }
        public string? ParamS { get; set; }
        public string? ParamT { get; set; }
        public string? ParamU { get; set; }
        public string? ParamV { get; set; }
        public string? ParamW { get; set; }
        public string? ParamX { get; set; }
        public string? ParamY { get; set; }
        public string? ParamZ { get; set; }
        public string? F_ScreenName { get; set; }
    }
}
