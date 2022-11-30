using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.CAL_Calculator.Controllers
{
    [Area("CAL_Calculator")]
    public class CAL_CalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEditCalculator()
        {
            return View();
        }
    }
}
