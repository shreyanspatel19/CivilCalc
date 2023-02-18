using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.DAL;
using CivilCalc.DAL.CAL.CAL_Calculator;
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

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(CAL_CalculatorModel objCalculatorModel)
        {
            var vModel = DBConfig.dbCALCalculator.SelectByCategoryNameCalculatorNameUserName(objCalculatorModel.F_CatagoryName, objCalculatorModel.F_CalculatorName, objCalculatorModel.F_UserName).ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult _AddEdit(int? CalculatorID)
        {
            ViewBag.Action = "Add";

            if (CalculatorID != null)
            {
                ViewBag.Action = "Edit";

                var d = DBConfig.dbCALCalculator.SelectPK(CalculatorID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<SelectPK_Result, CAL_CalculatorModel>());
                var vModel = AutoMapper.Mapper.Map<SelectPK_Result, CAL_CalculatorModel>(d);

                 return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(CAL_CalculatorModel objCalculatorModel)
        {
            if (objCalculatorModel.CalculatorID == 0)
            {
                var vReturn = DBConfig.dbCALCalculator.Insert(objCalculatorModel);
            }
            else
            {
                DBConfig.dbCALCalculator.Update(objCalculatorModel);
            }
            return Content(null);
        }
        #endregion        

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int CalculatorID)
        {
            DBConfig.dbCALCalculator.Delete(CalculatorID);
            return Content(null);
        }
        #endregion
    }
}
