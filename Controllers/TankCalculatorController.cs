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
    public class TankCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Tank-Volume-Capacity-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Tank-Volume-Capacity-Calculator");


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

            return View("TankCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(TankCalculator tank)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Tank-Volume-Capacity-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (tank.UnitID == 1)
                CalculateTankValueForMeterAndCM(tank);
            else
                CalculateTankValueForFeetAndInch(tank);

            CalculatorLogInsert(tank);

            return PartialView("_TankCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Tank Value For Meter And CM

        protected void CalculateTankValueForMeterAndCM(TankCalculator tank)
        {
            try
            {
                if (tank.LengthA != null && tank.WidthA != null && tank.DepthA != null)
                {
                    #region Calculation

                    Decimal TankCubicMeterAndCMValue = CommonFunctions.Volume(Convert.ToDecimal(tank.LengthA + "." + tank.LengthB), Convert.ToDecimal(tank.WidthA + "." + tank.WidthB), Convert.ToDecimal(tank.DepthA + "." + tank.DepthB));
                    ViewBag.lblAnswerTankCubicMeterAndCMValue = TankCubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";

                    Decimal TankCubicFeetAndInchValue = CommonFunctions.ConvertFeetAndInchForVolume(TankCubicMeterAndCMValue); ;
                    ViewBag.lblAnswerTankCubicFeetAndInchValue = TankCubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";

                    Decimal TankLiterVolumeValue = 0;
                    TankLiterVolumeValue = TankCubicMeterAndCMValue * Convert.ToDecimal(1000);
                    ViewBag.lblAnswerTankLiterVolumeValue = TankLiterVolumeValue.ToString("0.00") + " lt";

                    #endregion Calculation

                    #region Formula
                    if (tank.LengthB != null || tank.WidthB != null || tank.DepthB != null)
                    {
                        ViewBag.lblTankFormula1 = @"<b>Total Volume</b> = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                                                 + @"<br /><br /><b>Total Volume =</b> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + tank.LengthA + "." + tank.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + tank.WidthA + "." + tank.WidthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + tank.DepthA + "." + tank.DepthB + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><b>Total Volume = </b>" + TankCubicMeterAndCMValue.ToString("0.00") + " " + "m<sup>3</sup>";
                        ViewBag.lblTankFormula2 =  @"<b>Total Quantity = </b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Volume</mi></msub></mrow><mo>&#xD7;</mo><msub><mi>1000</mi></msub></mrow>"
                                                                 + @"<br /><br /><b>Total Quantity = </b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + TankCubicMeterAndCMValue.ToString("0.0000") + "</mi></msub><mo>&#xD7;</mo><msub><mi>1000</mi></msub></mrow>"
                                                                 + @"<br /><br /><b>Total Quantity = </b>" + TankLiterVolumeValue.ToString("0.00") + " " + "lt";
                    }
                    else
                    {
                        ViewBag.lblTankFormula1 = @"<b>Total Volume = </b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                                                 + @"<br /><br /><b>Total Volume = </b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + tank.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + tank.WidthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + tank.DepthA + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><b>Total Volume = </b>" + TankCubicMeterAndCMValue.ToString("0.00") + " " + "m<sup>3</sup>";
                        ViewBag.lblTankFormula2 = @"<b>Total Quantity = </b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Volume</mi></msub></mrow><mo>&#xD7;</mo><msub><mi>1000</mi></msub></mrow>"
                                                                 + @"<br /><br /><b>Total Quantity = </b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + TankCubicMeterAndCMValue.ToString("0.0000") + "</mi></msub><mo>&#xD7;</mo><msub><mi>1000</mi></msub></mrow>"
                                                                 + @"<br /><br /><b>Total Quantity = </b>" + TankLiterVolumeValue.ToString("0.00") + " " + "lt";
                    }
                    #endregion Formula
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Tank Value For Meter And CM

        #region Function Calculate Tank Value For Feet And Inch

        protected void CalculateTankValueForFeetAndInch(TankCalculator tank)
        {
            try
            {
                if (tank.LengthA != null && tank.WidthA != null && tank.DepthA != null)
                {
                    #region Calculation

                    Decimal TankCubicFeetAndInchValue = CommonFunctions.Volume(Convert.ToDecimal(tank.LengthA + "." + tank.LengthB), Convert.ToDecimal(tank.WidthA + "." + tank.WidthB), Convert.ToDecimal(tank.DepthA + "." + tank.DepthB));
                    ViewBag.lblAnswerTankCubicFeetAndInchValue = TankCubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";

                    Decimal TankCubicMeterAndCMValue = CommonFunctions.ConvertMeterAndCMForVolume(TankCubicFeetAndInchValue);
                    ViewBag.lblAnswerTankCubicMeterAndCMValue = TankCubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";

                    Decimal TankLiterVolumeValue = 0;
                    TankLiterVolumeValue = TankCubicMeterAndCMValue * Convert.ToDecimal(1000);
                    ViewBag.lblAnswerTankLiterVolumeValue = TankLiterVolumeValue.ToString("0.00") + " lt";

                    #endregion Calculation

                    #region Formula
                    if (tank.LengthB != null || tank.WidthB != null || tank.DepthB != null)
                    {
                        ViewBag.lblTankFormula = @"Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                                                     + @"<br /><br />Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + tank.LengthA + "." + tank.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + tank.WidthA + "." + tank.WidthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + tank.DepthA + "." + tank.DepthB + "</mi></msub></mrow>"
                                                                     + @"<br /><br />Total Volume = " + TankCubicFeetAndInchValue.ToString("0.00") + " " + "ft<sup>3</sup>"
                                                                     + @"<br /><br />Total Quantity  = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Volume in Meter Cube</mi></msub></mrow><mo>&#xD7;</mo><msub><mi>1000</mi></msub></mrow>"
                                                                     + @"<br /><br />Total Quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + TankCubicMeterAndCMValue.ToString("0.0000") + "</mi></msub><mo>&#xD7;</mo><msub><mi>1000</mi></msub></mrow>"
                                                                     + @"<br /><br />Total Quantity = " + TankLiterVolumeValue.ToString("0.00") + " " + "lt";
                    }
                    else
                    {
                        ViewBag.lblTankFormula = @"Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                                                     + @"<br /><br />Total Volume = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + tank.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + tank.WidthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + tank.DepthA + "</mi></msub></mrow>"
                                                                     + @"<br /><br />Total Volume = " + TankCubicFeetAndInchValue.ToString("0.00") + " " + "ft<sup>3</sup>"
                                                                     + @"<br /><br />Total Quantity  = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Volume in Meter Cube</mi></msub></mrow><mo>&#xD7;</mo><msub><mi>1000</mi></msub></mrow>"
                                                                     + @"<br /><br />Total Quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + TankCubicMeterAndCMValue.ToString("0.0000") + "</mi></msub><mo>&#xD7;</mo><msub><mi>1000</mi></msub></mrow>"
                                                                     + @"<br /><br />Total Quantity = " + TankLiterVolumeValue.ToString("0.00") + " " + "lt";
                    }
                    #endregion Formula
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Tank Value For Feet And Inch

        #region Insert Log Function

        public void CalculatorLogInsert(TankCalculator tank)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Tank-Calculator";

                if (tank.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(tank.UnitID);

                if (tank.LengthA != null)
                {
                    if (tank.LengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(tank.LengthA + "." + tank.LengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(tank.LengthA);
                }


                if (tank.WidthA != null)
                {
                    if (tank.WidthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(tank.WidthA + "." + tank.WidthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(tank.WidthA);
                }


                if (tank.DepthA != null)
                {
                    if (tank.DepthB != null)
                        entLOG_Calculation.ParamD = Convert.ToString(tank.DepthA + "." + tank.DepthB);
                    else
                        entLOG_Calculation.ParamD = Convert.ToString(tank.DepthA);
                }

                if (tank.UnitID == 1)
                {
                    if (ViewBag.lblAnswerTankCubicMeterAndCMValue != String.Empty)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerTankCubicMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerTankCubicFeetAndInchValue;
                    }
                }
                else
                {
                    if (ViewBag.lblAnswerTankCubicFeetAndInchValue != String.Empty)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerTankCubicFeetAndInchValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerTankCubicMeterAndCMValue;
                    }
                }

                if (ViewBag.lblAnswerTankLiterVolumeValue != String.Empty)
                    entLOG_Calculation.ParamG = ViewBag.lblAnswerTankLiterVolumeValue;

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
