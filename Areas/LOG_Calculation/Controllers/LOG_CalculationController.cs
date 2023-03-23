using Microsoft.AspNetCore.Mvc;
using CivilCalc.Areas.LOG_Calculation.Models;
using CivilCalc.DAL.LOG.LOG_Calculation;
using AutoMapper;
using CivilCalc.BAL;
using CivilCalc.DAL;

namespace CivilCalc.Areas.LOG_Calculation.Controllers
{
    [CheckAccess]
    [Area("LOG_Calculation")]
    public class LOG_CalculationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(LOG_CalculationModel obj_LOG_Calculation)
        {
            var vModel = DBConfig.dbLOGCalculation.SelectForSearch(obj_LOG_Calculation.F_ScreenName).ToList();
            return PartialView("_List", vModel);
        }

        #endregion

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int CalculationID)
        {
            DBConfig.dbLOGCalculation.Delete(CalculationID);
            return Content(null);
        }
        #endregion
    }
}
