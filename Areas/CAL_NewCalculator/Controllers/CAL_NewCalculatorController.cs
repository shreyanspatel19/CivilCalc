using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.CAL_NewCalculator.Controllers
{
	[Area("CAL_NewCalculator")]
	public class CAL_NewCalculatorController : Controller
	{		
		public IActionResult Index()
		{
			return View();
		}

        public IActionResult AddEditNewCalculator()
        {
            return View();
        }
    }
}
