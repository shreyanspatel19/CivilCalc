using System.Net.NetworkInformation;

namespace CivilCalc.BAL
{
    public static class CV
    {
        private static IHttpContextAccessor _httpContextAccessor;

        static CV()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        public static string? UserName()
        {
            string? UserName = null;
            if(_httpContextAccessor.HttpContext.Session.GetString("UserName") != null)
            {
                UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName").ToString();
            }
            return UserName;
        }

        public static int? UserID()
        {
            int? UserID = null;

            if(_httpContextAccessor.HttpContext.Session.GetString("UserID") != null)
            {
                UserID = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserID"));
                    
            }
            return UserID;
        }
        // table icone 
        public static string imgtiles = "/ClinetPanel/img/icons/tiles_calculator.png";
        public static string imgCement = "/ClinetPanel/img/icons/cement.png";
        public static string imgSand = "/ClinetPanel/img/icons/sand.png";
        public static string imgAggregate = "/ClinetPanel/img/icons/Aggregate.png";
        public static string imgPaintArea = "/ClinetPanel/img/icons/Icon_PaintArea.png";
        public static string imgprimer = "/ClinetPanel/img/icons/ICON_primer.png";
        public static string imgputty = "/ClinetPanel/img/icons/ICON_putty-knife.png";

    }
}
