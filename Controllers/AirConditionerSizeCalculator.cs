using CivilCalc.Models;
using CivilEngineeringCalculators;
using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Controllers
{
    public class AirConditionerSizeCalculatorController : Controller
    {

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(AirConditionerSizeCalculator aircalculator)
        {
            return PartialView("_CalculationDetails");
        }
        #endregion


    }
}
