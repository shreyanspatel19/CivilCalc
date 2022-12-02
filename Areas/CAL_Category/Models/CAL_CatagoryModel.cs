namespace CivilCalc.Areas.CAL_Catagory.Models
{
    public class CAL_CatagoryModel
    {
        public int CatagoryId { get; set; }
        public string? CatagoryName { get; set; }

        public string? Description { get; set; }

        public float? Sequence { get; set; }

        public int UserID { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        
    }
}
