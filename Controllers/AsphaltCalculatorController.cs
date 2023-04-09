using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.Areas.LOG_Calculation.Models;
using CivilCalc.DAL;
using CivilCalc.Models;
using CivilEngineeringCalculators;
using Microsoft.AspNetCore.Mvc;
using SelectForSearch_Result = CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result;

namespace CivilCalc.Controllers
{
    public class AsphaltCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Asphalt-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Asphalt-Calculator");


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

            return View("AsphaltCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(AsphaltCalculator asphalt)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Asphalt-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            CalculateValue(asphalt);
            CalculatorLogInsert(asphalt);

            return PartialView("_AsphaltCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Value For Asphalt

        protected void CalculateValue(AsphaltCalculator asphalt)
        {
            try
            {
                #region Variables

                Decimal AsphaltMeterCMValue = 0;
                Decimal AsphaltFeetInchValue = 0;
                Decimal AsphaltInKg = 0;
                Decimal AsphaltInTonne = 0;

                #endregion Variables

                #region Calculate Quantity

                if (asphalt.LengthA != null && asphalt.WidthA != null && asphalt.DepthA != null)
                {
                    if (asphalt.UnitID == 1)
                    {
                        AsphaltMeterCMValue = CommonFunctions.Volume(Convert.ToDecimal(asphalt.LengthA + "." + asphalt.LengthB), Convert.ToDecimal(asphalt.WidthA + "." + asphalt.WidthB), Convert.ToDecimal(asphalt.DepthA + "." + asphalt.DepthB));
                        ViewBag.lblAsphaltMeterAndCMValue = AsphaltMeterCMValue.ToString("0.00") + " m<sup>3</sup>";
                        AsphaltFeetInchValue = CommonFunctions.ConvertFeetAndInchForVolume(AsphaltMeterCMValue);
                        ViewBag.lblAsphaltFeetAndInchValue = AsphaltFeetInchValue.ToString("0.00") + " ft<sup>3</sup>";
                    }
                    else
                    {
                        AsphaltFeetInchValue = CommonFunctions.Volume(Convert.ToDecimal(asphalt.LengthA + "." + asphalt.LengthB), Convert.ToDecimal(asphalt.WidthA + "." + asphalt.WidthB), Convert.ToDecimal(asphalt.DepthA + "." + asphalt.DepthB));
                        ViewBag.lblAsphaltMeterAndCMValue = AsphaltFeetInchValue.ToString("0.00") + " ft<sup>3</sup>";
                        AsphaltMeterCMValue = CommonFunctions.ConvertMeterAndCMForVolume(AsphaltFeetInchValue);
                        ViewBag.lblAsphaltFeetAndInchValue = AsphaltMeterCMValue.ToString("0.00") + " m<sup>3</sup>";
                    }

                    AsphaltInKg = AsphaltMeterCMValue * 2322m;
                    AsphaltInTonne = AsphaltInKg / 1000m;
                    ViewBag.lblQuantityAnswerAsphaltValue = Convert.ToDecimal(AsphaltInTonne).ToString("0.00") + " Ton";

                }

                #endregion Calculate Quantity

                #region Formula

                ViewBag.lblAsphaltFormula1 = @"<b>Total Volume = </b><math xmlns=""http://www.w3.org/1998/math/mathml""><mrow><msub><mi>Length</mi></msub><mo>&#xd7;</mo><msub><mi>Width</mi></msub><mo>&#xd7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                       + @"<br /><br /><b>Total Volume = </b><math xmlns=""http://www.w3.org/1998/math/mathml""><mrow><msub><mi>" + asphalt.LengthA + "." + asphalt.LengthB + "</mi></msub><mo>&#xd7;</mo><msub><mi>" + asphalt.WidthA + "." + asphalt.WidthB + "</mi></msub><mo>&#xd7;</mo><msub><mi>" + asphalt.DepthA + "." + asphalt.DepthB + "</mi></msub></mrow>"
                                       + @"<br /><br /><b>Total Volume = </b>" + AsphaltMeterCMValue.ToString("0.00") + " " + "m<sup>3</sup>";

                ViewBag.lblAsphaltFormula2 = @"<b>Total Quantity = </b><math xmlns=""http://www.w3.org/1998/math/mathml""><mrow><msub><mi>Total Volume</mi></msub><mo>&#xd7;</mo><msub><mi>Density of Asphalt</mi></msub></mrow>"
                                       + @"<br /><br /><b>Total Quantity = </b><math xmlns=""http://www.w3.org/1998/math/mathml""><mrow><msub><mi>" + AsphaltMeterCMValue.ToString("0.00") + "</mi></msub><mo>&#xd7;</mo><msub><mi>2322</mi></msub></mrow>"
                                       + @"<br /><br /><b>Total Quantity = </b>" + AsphaltInKg.ToString("0.00") + " kgs or " + AsphaltInTonne.ToString("0.00") + " ton";
                #endregion Formula
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Value For Asphalt

        #region Insert Log Function
        public void CalculatorLogInsert(AsphaltCalculator asphalt)
        {
            try
            {


                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Asphalt-Calculator";

                if (asphalt.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(asphalt.UnitID);

                if (asphalt.LengthA != null)
                {
                    if (asphalt.LengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(asphalt.LengthA + "." + asphalt.LengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(asphalt.LengthA);
                }

                if (asphalt.WidthA != null)
                {
                    if (asphalt.WidthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(asphalt.WidthA + "." + asphalt.WidthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(asphalt.WidthA);
                }


                if (asphalt.DepthA != null)
                {
                    if (asphalt.DepthB != null)
                        entLOG_Calculation.ParamD = Convert.ToString(asphalt.DepthA + "." + asphalt.DepthB);
                    else
                        entLOG_Calculation.ParamD = Convert.ToString(asphalt.DepthA);
                }

                entLOG_Calculation.ParamE = ViewBag.lblAsphaltMeterAndCMValue;
                entLOG_Calculation.ParamF = ViewBag.lblAsphaltFeetAndInchValue;
                entLOG_Calculation.ParamG = ViewBag.lblQuantityAnswerAsphaltValue;

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
