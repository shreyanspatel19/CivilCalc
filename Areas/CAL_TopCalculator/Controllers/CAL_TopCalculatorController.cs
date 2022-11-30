using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.CAL_TopCalculator.Controllers
{
	[Area("CAL_TopCalculator")]
	public class CAL_TopCalculatorController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult AddEditTopCalculator()
		{
			return View();
		}
	}
}
