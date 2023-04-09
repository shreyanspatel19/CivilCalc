using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.DAL;
using CivilCalc.Models;
using Microsoft.AspNetCore.Mvc;
using CivilEngineeringCalculators;
using SelectForSearch_Result = CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result;
using CivilCalc.Areas.LOG_Calculation.Models;

namespace CivilCalc.Controllers
{
    public class ExcavationCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Excavation-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Excavation-Calculator");


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

            return View("ExcavationCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(ExcavationCalculator excavation)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Excavation-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (excavation.UnitID == 1)
                CalculateExcavationValueForMeterAndCM(excavation);
            else
                CalculateExcavationValueForFeetAndInch(excavation);

            CalculatorLogInsert(excavation);

            return PartialView("_ExcavationCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Excavation Value For Meter And CM

        protected void CalculateExcavationValueForMeterAndCM(ExcavationCalculator excavation)
        {
            try
            {
                if (excavation.LengthA != null && excavation.WidthA != null && excavation.DepthA != null)
                {
                    #region Calculation

                    Decimal ExcavationCubicMeterAndCMValue = CommonFunctions.Volume(Convert.ToDecimal(excavation.LengthA + "." + excavation.LengthB), Convert.ToDecimal(excavation.WidthA + "." + excavation.WidthB), Convert.ToDecimal(excavation.DepthA + "." + excavation.DepthB));
                    ViewBag.lblAnswerExcavationCubicMeterAndCMValue = ExcavationCubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";

                    Decimal ExcavationCubicFeetAndInchValue = CommonFunctions.ConvertFeetAndInchForVolume(ExcavationCubicMeterAndCMValue);
                    ViewBag.lblAnswerExcavationCubicFeetAndInchValue = ExcavationCubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";

                    #endregion Calculation

                    #region Formula For Meter/CM

                    if (excavation.LengthB != null || excavation.WidthB != null || excavation.DepthB != null)
                    {
                        ViewBag.lblExcavationFormula = @"Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                                  + @"<br /><br />Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + excavation.LengthA + "." + excavation.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + excavation.WidthA + "." + excavation.WidthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + excavation.DepthA + "." + excavation.DepthB + "</mi></msub></mrow>"
                                                  + @"<br /><br />Total Volume = " + ExcavationCubicMeterAndCMValue.ToString("0.00") + " " + "m<sup>3</sup>";
                    }
                    else
                    {
                        ViewBag.lblExcavationFormula = @"Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                                  + @"<br /><br />Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + excavation.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + excavation.WidthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + excavation.DepthA + "</mi></msub></mrow>"
                                                  + @"<br /><br />Total Volume = " + ExcavationCubicMeterAndCMValue.ToString("0.00") + " " + "m<sup>3</sup>";
                    }

                    #endregion Formula For Meter/CM
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Excavation Value For Meter And CM

        #region Function Calculate Excavation Value For Feet And Inch

        protected void CalculateExcavationValueForFeetAndInch(ExcavationCalculator excavation)
        {
            try
            {
                if (excavation.LengthA != null && excavation.WidthA != null && excavation.DepthA != null)
                {
                    #region Calculation

                    Decimal ExcavationCubicFeetAndInchValue = CommonFunctions.Volume(Convert.ToDecimal(excavation.LengthA + "." + excavation.LengthB), Convert.ToDecimal(excavation.WidthA + "." + excavation.WidthB), Convert.ToDecimal(excavation.DepthA + "." + excavation.DepthB));
                    ViewBag.lblAnswerExcavationCubicFeetAndInchValue = ExcavationCubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";

                    Decimal ExcavationCubicMeterAndCMValue = CommonFunctions.ConvertMeterAndCMForVolume(ExcavationCubicFeetAndInchValue);
                    ViewBag.lblAnswerExcavationCubicMeterAndCMValue = ExcavationCubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";

                    #endregion Calculation

                    #region Formula For Feet/Inch

                    if (excavation.LengthB != null || excavation.WidthB != null || excavation.DepthB != null)
                    {
                        ViewBag.lblExcavationFormula = @"Total volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                                                     + @"<br /><br />Total volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + excavation.LengthA + "." + excavation.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + excavation.WidthA + "." + excavation.WidthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + excavation.DepthA + "." + excavation.DepthB + "</mi></msub></mrow>"
                                                                     + @"<br /><br />Total volume = " + ExcavationCubicFeetAndInchValue.ToString("0.00") + " " + "ft<sup>3</sup>";
                    }
                    else
                    {
                        ViewBag.lblExcavationFormula = @"Total volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                                                     + @"<br /><br />Total volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + excavation.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + excavation.WidthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + excavation.DepthA + "</mi></msub></mrow>"
                                                                     + @"<br /><br />Total volume = " + ExcavationCubicFeetAndInchValue.ToString("0.00") + " " + "ft<sup>3</sup>";
                    }
                    #endregion Formula For Feet/Inch
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Excavation Value For Feet And Inch

        #region Insert Log Function
        public void CalculatorLogInsert(ExcavationCalculator excavation)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Excavation-Calculator";

                if (excavation.UnitID > 0)
                    entLOG_Calculation.ParamA = Convert.ToString(excavation.UnitID);

                if (excavation.LengthA != null)
                {
                    if (excavation.LengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(excavation.LengthA + "." + excavation.LengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(excavation.LengthA);
                }

                if (excavation.WidthA != null)
                {
                    if (excavation.WidthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(excavation.WidthA + "." + excavation.WidthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(excavation.WidthA);
                }

                if (excavation.DepthA != null)
                {
                    if (excavation.DepthB != null)
                        entLOG_Calculation.ParamD = Convert.ToString(excavation.DepthA + "." + excavation.DepthB);
                    else
                        entLOG_Calculation.ParamD = Convert.ToString(excavation.DepthA);
                }

                if (excavation.UnitID == 1)
                {
                    if (ViewBag.lblAnswerExcavationCubicMeterAndCMValue != String.Empty)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerExcavationCubicMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerExcavationCubicFeetAndInchValue;
                    }
                }
                else
                {
                    if (ViewBag.lblAnswerExcavationCubicFeetAndInchValue != String.Empty)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerExcavationCubicFeetAndInchValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerExcavationCubicMeterAndCMValue;
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
