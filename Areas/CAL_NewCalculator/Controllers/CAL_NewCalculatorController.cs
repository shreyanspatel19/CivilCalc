using AutoMapper;
using CivilCalc.Areas.CAL_NewCalculator.Models;
using CivilCalc.DAL;
using CivilCalc.DAL.CAL.CAL_NewCalculator;
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

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(CAL_NewCalculatorModel objNewCalculatorModel)
        {
            var vModel = DBConfig.dbCALNewCalculator.SelectAll().ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult _AddEdit(int? NewCalculatorID)
        {
            ViewBag.Action = "Add";

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
        public IActionResult _Save(CAL_NewCalculatorModel objNewCalculatorModel)
        {
            if (objNewCalculatorModel.NewCalculatorID == 0)
            {
                var vReturn = DBConfig.dbCALNewCalculator.Insert(objNewCalculatorModel);
            }
            else
            {
                DBConfig.dbCALNewCalculator.Update(objNewCalculatorModel);
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
