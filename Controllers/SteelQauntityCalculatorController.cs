using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.Areas.LOG_Calculation.Models;
using CivilCalc.DAL;
using CivilCalc.DAL.LOG.LOG_Calculation;
using CivilCalc.Models;
using Microsoft.AspNetCore.Mvc;
using SelectForSearch_Result = CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result;

namespace CivilCalc.Controllers
{
    public class SteelQauntityCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Steel-Quantity-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Steel-Quantity-Calculator");


            if (lstCalculator.Count > 0)
            {
                foreach (var itemCalculator in lstCalculator)
                {
                    ViewData["HeaderName"] = itemCalculator.HeaderName;
                    ViewData["SubHeaderName"] = itemCalculator.SubHeaderName;
                    ViewData["CalculatorName"] = itemCalculator.CalculatorName;
                    ViewData["CategoryName"] = itemCalculator.CategoryName;
                    ViewBag.CalculatorIcon = itemCalculator.CalculatorIcon;

                    // Meta tag
                    ViewData["MetaTitle"] = itemCalculator.MetaTitle;
                    ViewData["MetaKeyword"] = itemCalculator.MetaKeyword;
                    ViewData["MetaDescription"] = itemCalculator.MetaDescription;
                    ViewData["MetaAuthor"] = itemCalculator.MetaAuthor;

                    // Meta Og tag
                    ViewData["MetaOgTitle"] = itemCalculator.MetaOgTitle;
                    ViewData["MetaOgType"] = itemCalculator.MetaOgType;
                    ViewData["MetaOgDescription"] = itemCalculator.MetaOgDescription;
                    ViewData["MetaOgUrl"] = itemCalculator.MetaOgUrl;
                    ViewData["MetaOgImage"] = itemCalculator.MetaOgImage;

                    break;
                }
            }

            return View("SteelQauntityCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(SteelQauntityCalculator steelqauntity)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Steel-Quantity-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            CalculateValue(steelqauntity);
            CalculatorLogInsert(steelqauntity);

            return PartialView("_SteelQauntityCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Value For Steel

        protected void CalculateValue(SteelQauntityCalculator steelqauntity)
        {
            try
            {
                #region Variables

                decimal ConcreteQauntity = Convert.ToDecimal(steelqauntity.ConcreteQauntity);
                decimal SteelQuantity = 0;

                #endregion Variables

                #region Calculate Quantity

                if (steelqauntity.ConcreteQauntity != null)
                    SteelQuantity = ConcreteQauntity * Convert.ToDecimal(steelqauntity.MemberType);

                ViewBag.lblKgAnswer = SteelQuantity.ToString("0.00") + " kg.";
                ViewBag.lblTonAnswer = (SteelQuantity / 1000m).ToString("0.00") + " ton";

                #endregion Calculate Quantity

                #region Formula

                ViewBag.lblSteelWeightFormula = @"<br /><math xmlns=""http://www.w3.org/1998/math/mathml""><mo><b>Steel quantity = </b></mo><mrow><msub><mi>Member type</mi></msub><mo>&#xd7;</mo><msub><mi>Concrete qauntity</mi></msub></mrow>"
                                           + @"<br /><br /><math xmlns=""http://www.w3.org/1998/math/mathml""><mo><b>Steel quantity = </b></mo><mrow><msub><mi>" + Convert.ToDecimal(steelqauntity.MemberType) + "</mi></msub><mo>&#xd7;</mo><msub><mi>" + ConcreteQauntity + "</mi></msub></mrow>"
                                           + @"<br /><br /><b>Total Quantity = </b>" + SteelQuantity.ToString("0.00") + " kg or " + (SteelQuantity / 1000m).ToString("0.00") + " ton";
                #endregion Formula
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Value For Steel

        #region Insert Log Function

        public void CalculatorLogInsert(SteelQauntityCalculator steelqauntity)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Steel-Qauntity-Calculator";

                if (steelqauntity.MemberType != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(steelqauntity.MemberType);

                if (steelqauntity.ConcreteQauntity != null)
                    entLOG_Calculation.ParamB = Convert.ToString(steelqauntity.ConcreteQauntity);

                entLOG_Calculation.ParamC = ViewBag.lblKgAnswer.Trim();
                entLOG_Calculation.ParamD = ViewBag.lblTonAnswer.Trim();
                entLOG_Calculation.Created = DateTime.Now;
                entLOG_Calculation.Modified = DateTime.Now;

                #endregion Gather Data


                #region Insert
                DBConfig.dbLOGCalculation.Insert(entLOG_Calculation);
                #endregion Insert
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Insert Log Function


    }
}
