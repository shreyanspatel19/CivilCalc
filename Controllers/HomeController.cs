using CivilCalc.Areas.CAL_Category.Models;
using CivilCalc.DAL;
using CivilCalc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CivilCalc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("Quantity-Estimator/{CalculatorName}")]
        public IActionResult QE(string CalculatorName)
        {
            ViewBag.Title = ""+ CalculatorName + "";
            DBConfig.dbCALCalculator.SelectPK(2).ToList();
            return Content(null);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}