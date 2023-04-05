using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.Areas.LOG_Calculation.Models;
using CivilCalc.DAL;
using CivilCalc.DAL.LOG.LOG_Calculation;
using CivilCalc.Models;
using CivilEngineeringCalculators;
using Microsoft.AspNetCore.Mvc;
using SelectForSearch_Result = CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result;

namespace CivilCalc.Controllers
{
    public class TopSoilCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Top-Soil-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Top-Soil-Calculator");


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

            return View("TopSoilCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(TopSoilCalculator TopSoil)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Top-Soil-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            CalculateTopSoilValue(TopSoil);
            CalculatorLogInsert(TopSoil);

            return PartialView("_TopSoilCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Top Soil Value For Meter And CM

        protected void CalculateTopSoilValue(TopSoilCalculator TopSoil)
        {
            decimal Length = Convert.ToDecimal(TopSoil.LengthA + "." + TopSoil.LengthB);
            decimal width = Convert.ToDecimal(TopSoil.WidthA + "." + TopSoil.WidthB);
            decimal depth = Convert.ToDecimal(TopSoil.Depth);
            string unit = (TopSoil.UnitID == 1) ? "m" : "ft";
            string answer;
            try
            {
                if (TopSoil.LengthA != null && TopSoil.WidthA != null && TopSoil.Depth != null)
                {
                    #region Calculation

                    if (TopSoil.UnitID == 1 && TopSoil.MeasurmentID == 1)
                    {
                        depth /= 100m; // cm to meter
                    }
                    else if (TopSoil.UnitID == 1 && TopSoil.MeasurmentID == 2)
                    {
                        depth *= 2.54m; // inch to cm
                        depth /= 100; // cm to metre
                    }
                    else if (TopSoil.UnitID == 2 && TopSoil.MeasurmentID == 1)
                    {
                        depth /= 10.7639m; // ft2 to meter2
                        depth /= 100; // cm to metre
                    }
                    else if (TopSoil.UnitID == 2 && TopSoil.MeasurmentID == 2)
                    {
                        depth /= 12m;  // inch to feet
                        depth /= 35.3147m; // ft3 to m3
                    }

                    Decimal TopSoilCubicMeterAndCMValue = CommonFunctions.Volume(Length, width, depth);
                    ViewBag.lblAnswerTopSoilCubicMeterAndCMValue = TopSoilCubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";

                    Decimal TopSoilCubicFeetAndInchValue = CommonFunctions.ConvertFeetAndInchForVolume(TopSoilCubicMeterAndCMValue);
                    ViewBag.lblAnswerTopSoilCubicFeetAndInchValue = TopSoilCubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";

                    answer = (TopSoil.UnitID == 1) ? TopSoilCubicMeterAndCMValue.ToString("0.00") : TopSoilCubicFeetAndInchValue.ToString("0.00");
                    #endregion Calculation

                    #region Formula For Meter/CM

                    ViewBag.lblTopSoilFormula = @"Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                                             + @"<br /><br />Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Length + " " + unit + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + width + " " + unit + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + depth.ToString("0.0000") + " " + unit + "</mi></msub></mrow>"
                                                             + @"<br /><br />Total Volume = " + answer + " " + unit + "<sup>3</sup>";
                    #endregion Formula For Meter/CM
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function CalculateTopSoilValueForMeterAndCM

        #region Insert Log Function

        public void CalculatorLogInsert(TopSoilCalculator TopSoil)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Top Soil Calculator";

                if (TopSoil.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(TopSoil.UnitID);

                if (TopSoil.LengthA != null)
                {
                    if (TopSoil.LengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(TopSoil.LengthA + "." + TopSoil.LengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(TopSoil.LengthA);
                }
                if (TopSoil.WidthA != null)
                {
                    if (TopSoil.WidthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(TopSoil.WidthA + "." + TopSoil.WidthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(TopSoil.WidthA);
                }

                if (TopSoil.Depth != null)
                    entLOG_Calculation.ParamD = Convert.ToString(TopSoil.Depth);

                if (TopSoil.UnitID == 1)
                {
                    if (ViewBag.lblAnswerTopSoilCubicMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerTopSoilCubicMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerTopSoilCubicFeetAndInchValue;
                    }
                }
                else if (TopSoil.UnitID == 2)
                {
                    if (ViewBag.lblAnswerTopSoilCubicFeetAndInchValue != null)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerTopSoilCubicFeetAndInchValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerTopSoilCubicMeterAndCMValue;
                    }
                }
                else
                {
                    if (ViewBag.lblAnswerTopSoilCubicMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerTopSoilCubicMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerTopSoilCubicFeetAndInchValue;
                    }
                }

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
