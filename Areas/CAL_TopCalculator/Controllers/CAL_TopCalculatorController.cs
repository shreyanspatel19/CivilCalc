using AutoMapper;
using CivilCalc.Areas.CAL_TopCalculator.Models;
using CivilCalc.BAL;
using CivilCalc.DAL;
using CivilCalc.DAL.CAL.CAL_TopCalculator;
using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.CAL_TopCalculator.Controllers
{
    [CheckAccess]
    [Area("CAL_TopCalculator")]
	public class CAL_TopCalculatorController : Controller
	{
		public IActionResult Index()
        {
      
            ViewBag.CalculatorList = DBConfig.dbCALCalculator.SelectComboBoxUser().ToList();
            return View();
		}
        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(CAL_TopCalculatorModel obj_CAL_TopCalculator)
        {
            var vModel = DBConfig.dbCALTopCalculator.SelectForSearch(obj_CAL_TopCalculator.CalculatorID).ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult _AddEdit(int? TopCalculatorID)
        {
            ViewBag.Action = "Add";
            ViewBag.CategoryList = DBConfig.dbCALCategory.SelectComboBoxCategory().ToList();
            ViewBag.CalculatorList = DBConfig.dbCALCalculator.SelectComboBoxUser().ToList();

            if (TopCalculatorID != null)
            {
                ViewBag.Action = "Edit";

                var d = DBConfig.dbCALTopCalculator.SelectPK(TopCalculatorID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<SelectPK_Result, CAL_TopCalculatorModel>());
                var vModel = AutoMapper.Mapper.Map<SelectPK_Result, CAL_TopCalculatorModel>(d);

                return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(CAL_TopCalculatorModel obj_CAL_TopCalculator)
        {
            if (obj_CAL_TopCalculator.TopCalculatorID == 0)
            {
                var vReturn = DBConfig.dbCALTopCalculator.Insert(obj_CAL_TopCalculator);
            }
            else
            {
                DBConfig.dbCALTopCalculator.Update(obj_CAL_TopCalculator);
            }
            return Content(null);
        }
        #endregion        

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int TopCalculatorID)
        {
            DBConfig.dbCALTopCalculator.Delete(TopCalculatorID);
            return Content(null);
        }
        #endregion
    }
}
