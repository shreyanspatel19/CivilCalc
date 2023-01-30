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

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult()
        {
            var vModel = DBConfig.dbCAL.dbo_PR_CAL_Category_SelectAll().ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}