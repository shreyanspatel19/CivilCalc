using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.CAL_Catagory.Controllers
{
    [Area("CAL_Catagory")]
    public class CAL_CatagoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
