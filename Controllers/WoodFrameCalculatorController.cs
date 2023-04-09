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
    public class WoodFrameCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Wood-Framing-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Wood-Framing-Calculator");


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

            return View("WoodFrameCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(AsphaltCalculator asphalt)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Wood-Framing-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (asphalt.UnitID == 1)
                CalculateWoodFrameValueForMeterAndCM(asphalt);
            else
                CalculateWoodFrameValueForFeetAndInch(asphalt);

            CalculatorLogInsert(asphalt);

            return PartialView("_WoodFrameCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Wood Frame Value For Meter And CM

        protected void CalculateWoodFrameValueForMeterAndCM(AsphaltCalculator asphalt)
        {
            try
            {
                if (asphalt.LengthA != null && asphalt.WidthA != null && asphalt.DepthA != null)
                {
                    #region Calculation

                    Decimal WoodFrameCubicMeterAndCMValue = CommonFunctions.Volume(Convert.ToDecimal(asphalt.LengthA + "." + asphalt.LengthB), Convert.ToDecimal(asphalt.WidthA + "." + asphalt.WidthB), Convert.ToDecimal(asphalt.DepthA + "." + asphalt.DepthB));
                    ViewBag.lblAnswerWoodFrameCubicMeterAndCMValue = WoodFrameCubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";

                    Decimal WoodFrameCubicFeetAndInchValue = CommonFunctions.ConvertFeetAndInchForVolume(WoodFrameCubicMeterAndCMValue);
                    ViewBag.lblAnswerWoodFrameCubicFeetAndInchValue = WoodFrameCubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";

                    #endregion Calculation

                    #region Formula For Meter/CM

                    if (asphalt.LengthB != null || asphalt.WidthB != null || asphalt.DepthB != null)
                    {
                        ViewBag.lblWoodFrameFormula = @"Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub><mo>&#xD7;</mo><msub><mi>Thickness</mi></msub></mrow>"
                                                                 + @"<br /><br />Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + asphalt.LengthA + "." + asphalt.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + asphalt.DepthA + "." + asphalt.DepthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + asphalt.WidthA + "." + asphalt.WidthB + "</mi></msub></mrow>"
                                                                 + @"<br /><br />Total Volume = " + WoodFrameCubicMeterAndCMValue.ToString("0.00") + " " + "m<sup>3</sup>";
                    }
                    else
                    {
                        ViewBag.lblWoodFrameFormula = @"Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub><mo>&#xD7;</mo><msub><mi>Thickness</mi></msub></mrow>"
                                                                 + @"<br /><br />Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + asphalt.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + asphalt.DepthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + asphalt.WidthA + "</mi></msub></mrow>"
                                                                 + @"<br /><br />Total Volume = " + WoodFrameCubicMeterAndCMValue.ToString("0.00") + " " + "m<sup>3</sup>";
                    }
                    #endregion Formula For Meter/CM
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Wood Frame Value For Meter And CM

        #region Function Calculate Wood Frame Value For Feet And Inch

        protected void CalculateWoodFrameValueForFeetAndInch(AsphaltCalculator asphalt)
        {
            try
            {
                if (asphalt.LengthA != null && asphalt.WidthA != null && asphalt.DepthA != null)
                {
                    #region Calculation

                    Decimal WoodFrameCubicFeetAndInchValue = CommonFunctions.Volume(Convert.ToDecimal(asphalt.LengthA + "." + asphalt.LengthB), Convert.ToDecimal(asphalt.WidthA + "." + asphalt.WidthB), Convert.ToDecimal(asphalt.DepthA + "." + asphalt.DepthB));
                    ViewBag.lblAnswerWoodFrameCubicFeetAndInchValue = WoodFrameCubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";

                    Decimal WoodFrameCubicMeterAndCMValue = CommonFunctions.ConvertMeterAndCMForVolume(WoodFrameCubicFeetAndInchValue);
                    ViewBag.lblAnswerWoodFrameCubicMeterAndCMValue = WoodFrameCubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";

                    #endregion Calculation

                    #region Formula For Feet/Inch

                    if (asphalt.LengthB != null || asphalt.WidthB != null || asphalt.DepthB != null)
                    {
                        ViewBag.lblWoodFrameFormula = @"Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub><mo>&#xD7;</mo><msub><mi>Thickness</mi></msub></mrow>"
                                                                 + @"<br /><br />Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + asphalt.LengthA + "." + asphalt.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + asphalt.DepthA + "." + asphalt.DepthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + asphalt.WidthA + "." + asphalt.WidthB + "</mi></msub></mrow>"
                                                                 + @"<br /><br />Total Volume = " + WoodFrameCubicFeetAndInchValue.ToString("0.00") + " " + "ft<sup>3</sup>";
                    }
                    else
                    {
                        ViewBag.lblWoodFrameFormula = @"Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub><mo>&#xD7;</mo><msub><mi>Thickness</mi></msub></mrow>"
                                                                 + @"<br /><br />Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + asphalt.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + asphalt.DepthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + asphalt.WidthA + "</mi></msub></mrow>"
                                                                 + @"<br /><br />Total Volume = " + WoodFrameCubicFeetAndInchValue.ToString("0.00") + " " + "ft<sup>3</sup>";
                    }


                    #endregion Formula For Feet/Inch
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Wood Frame Value For Feet And Inch

        #region Insert Log Function

        public void CalculatorLogInsert(AsphaltCalculator asphalt)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Wood-Frame-Calculator";

                if (asphalt.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(asphalt.UnitID);

                if (asphalt.LengthA != null)
                {
                    if (asphalt.LengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(asphalt.LengthA + "." + asphalt.LengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(asphalt.LengthA);
                }

                if (asphalt.DepthA != null)
                {
                    if (asphalt.DepthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(asphalt.DepthA + "." + asphalt.DepthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(asphalt.DepthA);
                }

                if (asphalt.WidthA != null)
                {
                    if (asphalt.WidthB != null)
                        entLOG_Calculation.ParamD = Convert.ToString(asphalt.WidthA + "." + asphalt.WidthB);
                    else
                        entLOG_Calculation.ParamD = Convert.ToString(asphalt.WidthA);
                }

                if (asphalt.UnitID == 1)
                {
                    if (ViewBag.lblAnswerWoodFrameCubicMeterAndCMValue != String.Empty)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerWoodFrameCubicMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerWoodFrameCubicFeetAndInchValue;
                    }
                }
                else
                {
                    if (ViewBag.lblAnswerWoodFrameCubicFeetAndInchValue != String.Empty)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerWoodFrameCubicFeetAndInchValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerWoodFrameCubicMeterAndCMValue;
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
