using AutoMapper;
using CivilCalc.Areas.CAL_NewCalculator.Models;
using CivilCalc.BAL;
using CivilCalc.DAL;
using CivilCalc.DAL.CAL.CAL_NewCalculator;
using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.CAL_NewCalculator.Controllers
{
    [CheckAccess]
    [Area("CAL_NewCalculator")]
	public class CAL_NewCalculatorController : Controller
	{		
		public IActionResult Index()
		{
            ViewBag.CalculatorList = DBConfig.dbCALCalculator.SelectComboBoxCalculator().ToList();
            return View();
		}

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(CAL_NewCalculatorModel obj_CAL_NewCalculator)
        {
            var vModel = DBConfig.dbCALNewCalculator.SelectForSearch(obj_CAL_NewCalculator.CalculatorID).ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult _AddEdit(int? NewCalculatorID)
        {
            ViewBag.Action = "Add";
            ViewBag.CategoryList = DBConfig.dbCALCategory.SelectComboBoxCategory().ToList();
            ViewBag.CalculatorList = DBConfig.dbCALCalculator.SelectComboBoxCalculator().ToList();

            if (NewCalculatorID != null)
            {
                ViewBag.Action = "Edit";

                var varNewCalculator = DBConfig.dbCALNewCalculator.SelectPK(NewCalculatorID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<SelectPK_Result, CAL_NewCalculatorModel>());
                var vModel = AutoMapper.Mapper.Map<SelectPK_Result, CAL_NewCalculatorModel>(varNewCalculator);

                return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(CAL_NewCalculatorModel obj_CAL_NewCalculator)
        {
            if (obj_CAL_NewCalculator.NewCalculatorID == 0)
            {
                var vReturn = DBConfig.dbCALNewCalculator.Insert(obj_CAL_NewCalculator);
            }
            else
            {
                DBConfig.dbCALNewCalculator.Update(obj_CAL_NewCalculator);
            }
            return Content(null);
        }
        #endregion        

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int NewCalculatorID)
        {
            DBConfig.dbCALNewCalculator.Delete(NewCalculatorID);
            return Content(null);
        }
        #endregion
    }
}
