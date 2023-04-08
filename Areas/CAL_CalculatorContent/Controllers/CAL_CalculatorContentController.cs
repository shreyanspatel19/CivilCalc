using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.Areas.CAL_CalculatorContent.Models;
using CivilCalc.BAL;
using CivilCalc.DAL;
using CivilCalc.DAL.CAL.CAL_CalculatorContent;
using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.CAL_CalculatorContent.Controllers
{
    [Area("CAL_CalculatorContent")]
    public class CAL_CalculatorContentController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.CalculatorList = DBConfig.dbCALCalculator.SelectComboBoxCalculator().ToList();
            return View();
        }

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(CAL_CalculatorContentModel obj_CAL_CalculatorContent)
        {

            var vModel = DBConfig.dbCALCalculatorContent.SelectAll().ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult AddEdit(int? CalculatorContentID)
        {
            ViewBag.Action = "Add";
            ViewBag.CalculatorList = DBConfig.dbCALCalculator.SelectComboBoxCalculator().ToList();

            if (CalculatorContentID != null)
            {
                ViewBag.Action = "Edit";

                var d = DBConfig.dbCALCalculatorContent.SelectPK(CalculatorContentID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<SelectPK_Result, CAL_CalculatorContentModel>());
                var vModel = AutoMapper.Mapper.Map<SelectPK_Result, CAL_CalculatorContentModel>(d);

                return View(vModel);
            }
            return View();
        }
        #endregion

        #region _Save
        [HttpPost]
        public IActionResult _Save(CAL_CalculatorContentModel obj_CAL_Calculator)
        {
           
            if (obj_CAL_Calculator.CalculatorContentID == 0)
            {
                var vReturn = DBConfig.dbCALCalculatorContent.Insert(obj_CAL_Calculator);
            }
            else
            {
                DBConfig.dbCALCalculatorContent.Update(obj_CAL_Calculator);
            }
            return RedirectToAction("Index");
        }
        #endregion     
    }
}
