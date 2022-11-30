using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.MST_Configuration.Controllers
{
    [Area("MST_Configuration")]
    public class MST_ConfigurationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEditConfiguration()
        {
            return View();
        }
    }
}
