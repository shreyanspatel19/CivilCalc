using CivilCalc.DAL.LOG.LOG_Calculation;
using CivilCalc.Models;
using CivilEngineeringCalculators;
using Microsoft.AspNetCore.Mvc;
using CivilCalc.DAL;
using CivilCalc.Areas.LOG_Calculation.Models;
using System.Collections.Generic;
using CivilCalc.DAL.CAL.CAL_Calculator;
using System.Data;

namespace CivilCalc.Controllers
{
    public class CementConcreteCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Cement-Concrete-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectURLName("/Quantity-Estimator/Brick-Calculator");


            if (lstCalculator.Count > 0)
            {
                foreach (var itemCalculator in lstCalculator)
                {
                    ViewBag.MetaTitle = itemCalculator.MetaTitle;
                    ViewBag.HeaderName = itemCalculator.HeaderName;
                    ViewBag.SubHeaderName = itemCalculator.SubHeaderName;
                    ViewBag.CalculatorName = itemCalculator.CalculatorName;
                    ViewBag.CategoryName = itemCalculator.CategoryName;
                    ViewBag.MetaKeyword = itemCalculator.MetaKeyword;
                    ViewBag.MetaDescription = itemCalculator.MetaDescription;
                    ViewBag.CalculatorIcon = itemCalculator.CalculatorIcon;
                    break;
                }
            }

            return View("CementConcreteCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(CementConcreteCalculator cementcalculator)
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> Calculator = DBConfig.dbCALCalculator.SelectURLName("/Quantity-Estimator/Brick-Calculator");
            
            return PartialView("_CementConcreteCalculatorResult", Calculator);
        }
        #endregion

    }
}
