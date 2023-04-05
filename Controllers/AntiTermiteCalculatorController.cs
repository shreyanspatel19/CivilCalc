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
    public class AntiTermiteCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Anti-Termite-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Anti-Termite-Calculator");


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

            return View("AntiTermiteCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(AntiTermiteCalculator antitermite)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Anti-Termite-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            if (antitermite.UnitID == 1)
                CalculateAntiTermiteValueForMeterAndCM(antitermite);
            else
                CalculateAntiTermiteValueForFeetAndInch(antitermite);

            CalculatorLogInsert(antitermite);

            return PartialView("_AntiTermiteCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Anti Termite Value For Meter And CM

        protected void CalculateAntiTermiteValueForMeterAndCM(AntiTermiteCalculator antitermite)
        {
            try
            {
                if (antitermite.LengthA != null && antitermite.WidthA != null)
                {
                    #region Calculation

                    Decimal FinalLengthInMeter = 0;
                    Decimal FinalWidthInMeter = 0;

                    #region Length In Meter
                    Decimal InputLengthInMeter = 0;
                    Decimal InputLengthInCM = 0;
                    if (antitermite.LengthA != null && antitermite.WidthA != null)
                        InputLengthInMeter = Convert.ToDecimal(antitermite.LengthA);
                    else
                        InputLengthInMeter = 0;


                    if (antitermite.LengthB != null && antitermite.WidthB != null)
                        InputLengthInCM = Convert.ToDecimal(antitermite.LengthB);
                    else
                        InputLengthInCM = 0;

                    FinalLengthInMeter = CommonFunctions.MeterAndCMToMeter(InputLengthInMeter, InputLengthInCM);

                    #endregion Length In Meter

                    #region Width In Meter
                    Decimal InputWidthInMeter = 0;
                    Decimal InputWidthInCM = 0;
                    if (antitermite.WidthA != null && antitermite.WidthA != null)
                        InputWidthInMeter = Convert.ToDecimal(antitermite.WidthA);
                    else
                        InputWidthInMeter = 0;


                    if (antitermite.WidthB != null && antitermite.WidthB != null)
                        InputWidthInCM = Convert.ToDecimal(antitermite.WidthB);
                    else
                        InputWidthInCM = 0;

                    FinalWidthInMeter = CommonFunctions.MeterAndCMToMeter(InputWidthInMeter, InputWidthInCM);

                    #endregion Width In Meter

                    Decimal AntiTermiteSquareMeterAndCMValue = CommonFunctions.Area(FinalLengthInMeter, FinalWidthInMeter);
                   ViewBag.lblAreaAnswerAntiTermiteSquareMeterAndCMValue = AntiTermiteSquareMeterAndCMValue.ToString("0.00") + " m<sup>2</sup>";

                    Decimal AntiTermiteSquareFeetAndInchValue = CommonFunctions.ConvertFeetAndInchForArea(AntiTermiteSquareMeterAndCMValue);
                   ViewBag.lblAreaAnswerAntiTermiteSquareFeetAndInchValue = AntiTermiteSquareFeetAndInchValue.ToString("0.00") + " ft<sup>2</sup>";

                    #endregion Calculation

                    #region Calculate Quantity

                   ViewBag.lblQuantityAnswerAntiTermiteSquareMeterAndCMValue = Convert.ToDecimal(AntiTermiteSquareMeterAndCMValue * 30).ToString("0.00") + " ml";

                    #endregion Calculate Quantity

                    #region Formula For Meter/CM

                   ViewBag.lblAntiTermiteFormula = @"<strong>Total Area = </strong> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                                                 + @"<br /><br /><strong>Total Area = </strong><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + FinalLengthInMeter.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + FinalWidthInMeter.ToString("0.00") + "</mi></msub></mrow>"
                                                 + @"<br /><br /><strong>Total Area = </strong>" + AntiTermiteSquareMeterAndCMValue.ToString("0.00") + " " + "m<sup>2</sup>"
                                                 + @"<br /><br /><strong>Total Quantity = </strong><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Area</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                                                 + @"<br /><br /><strong>Total quantity =</strong> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + AntiTermiteSquareMeterAndCMValue.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                                                 + @"<br /><br /><strong>Total quantity = </strong>" + ViewBag.lblQuantityAnswerAntiTermiteSquareMeterAndCMValue;

                    //if (txtLengthB.Text.Trim() != String.Empty || txtWidthB.Text.Trim() != String.Empty)
                    //{
                    //    lblAntiTermiteFormula.Text = @"Total Area = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                    //                                         + @"<br /><br />Total Area = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + txtLengthA.Text.Trim() + "." + txtLengthB.Text.Trim() + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + txtWidthA.Text.Trim() + "." + txtWidthB.Text.Trim() + "</mi></msub></mrow>"
                    //                                         + @"<br /><br />Total Area = " + AntiTermiteSquareMeterAndCMValue.ToString("0.00") + " " + "m<sup>2</sup>"
                    //                                         + @"<br /><br />Total Quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Area</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                    //                                         + @"<br /><br />Total quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + AntiTermiteSquareMeterAndCMValue.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                    //                                         + @"<br /><br />Total quantity = " + lblQuantityAnswerAntiTermiteSquareMeterAndCMValue.Text;
                    //}
                    //else
                    //{
                    //    lblAntiTermiteFormula.Text = @"Total Area = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                    //                                         + @"<br /><br />Total Area = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + txtLengthA.Text.Trim() + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + txtWidthA.Text.Trim() + "</mi></msub></mrow>"
                    //                                         + @"<br /><br />Total Area = " + AntiTermiteSquareMeterAndCMValue.ToString("0.00") + " " + "m<sup>2</sup>"
                    //                                         + @"<br /><br />Total Quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Area</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                    //                                         + @"<br /><br />Total quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + AntiTermiteSquareMeterAndCMValue.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                    //                                         + @"<br /><br />Total quantity = " + lblQuantityAnswerAntiTermiteSquareMeterAndCMValue.Text;
                    //}

                    #endregion Formula For Meter/CM
                }
            }
            catch (Exception ex)
            {
                //ucMessage.ShowError(ex.Message);
            }
        }

        #endregion Function Calculate Anti Termite Value For Meter And CM

        #region Function Calculate Anti Termite Value For Feet And Inch

        protected void CalculateAntiTermiteValueForFeetAndInch(AntiTermiteCalculator antitermite)
        {
            try
            {
                if (antitermite.LengthA != null && antitermite.WidthA != null)
                {
                    #region Calculation

                    Decimal FinalLengthInFeet = 0;
                    Decimal FinalWidthInFeet = 0;

                    #region Length In Feet
                    Decimal InputLengthInFeet = 0;
                    Decimal InputLengthInInch = 0;
                    if (antitermite.LengthA != null && antitermite.LengthA != null)
                        InputLengthInFeet = Convert.ToDecimal(antitermite.LengthA);
                    else
                        InputLengthInFeet = 0;


                    if (antitermite.LengthB != null && antitermite.LengthB != null)
                        InputLengthInInch = Convert.ToDecimal(antitermite.LengthB);
                    else
                        InputLengthInInch = 0;

                    FinalLengthInFeet = CommonFunctions.FeetAndInchToFeet(InputLengthInFeet, InputLengthInInch);

                    #endregion Length In Feet

                    #region Width In Feet
                    Decimal InputWidthInFeet = 0;
                    Decimal InputWidthInInch = 0;
                    if (antitermite.WidthA != null && antitermite.WidthA != null)
                        InputWidthInFeet = Convert.ToDecimal(antitermite.WidthA);
                    else
                        InputWidthInFeet = 0;


                    if (antitermite.WidthB != null && antitermite.WidthB != null)
                        InputWidthInInch = Convert.ToDecimal(antitermite.WidthB);
                    else
                        InputWidthInInch = 0;

                    FinalWidthInFeet = CommonFunctions.FeetAndInchToFeet(InputWidthInFeet, InputWidthInInch);

                    #endregion Width In Feet

                    Decimal AntiTermiteSquareFeetAndInchValue = CommonFunctions.Area(FinalLengthInFeet, FinalWidthInFeet);
                    ViewBag.lblAreaAnswerAntiTermiteSquareFeetAndInchValue = AntiTermiteSquareFeetAndInchValue.ToString("0.00") + " ft<sup>2</sup>";

                    Decimal AntiTermiteSquareMeterAndCMValue = CommonFunctions.ConvertMeterAndCMForArea(AntiTermiteSquareFeetAndInchValue);
                    ViewBag.lblAreaAnswerAntiTermiteSquareMeterAndCMValue = AntiTermiteSquareMeterAndCMValue.ToString("0.00") + " m<sup>2</sup>";

                    #endregion Calculation

                    #region Calculate Quantity

                   ViewBag.lblQuantityAnswerAntiTermiteSquareMeterAndCMValue = Convert.ToDecimal(AntiTermiteSquareMeterAndCMValue * 30).ToString("0.00") + " ml";
                    //lblAntiTermiteQuantityValue.Text = "anti-termite ml (milliliter)";

                    #endregion Calculate Quantity

                    #region Formula For Feet/Inch

                   ViewBag.lblAntiTermiteFormula = @"<strong>Total Area = </strong><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                                                 + @"<br /><br /><strong>Total Area =</strong> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + FinalLengthInFeet.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + FinalWidthInFeet.ToString("0.00") + "</mi></msub></mrow>"
                                                 + @"<br /><br /><strong>Total Area =</strong> " + AntiTermiteSquareFeetAndInchValue.ToString("0.00") + " " + "ft<sup>2</sup>"
                                                 + @"<br /><br /><strong>Total Quantity = </strong><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Area (Square Meter)</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                                                 + @"<br /><br /><strong>Total Quantity = </strong><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + AntiTermiteSquareMeterAndCMValue.ToString("0.000") + "</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                                                 + @"<br /><br /><strong>Total Quantity = </strong>" + ViewBag.lblQuantityAnswerAntiTermiteSquareMeterAndCMValue;


                    //if (txtLengthB.Text.Trim() != String.Empty || txtWidthB.Text.Trim() != String.Empty)
                    //{
                    //    lblAntiTermiteFormula.Text = @"Total Area = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                    //                                             + @"<br /><br />Total Area = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + txtLengthA.Text.Trim() + "." + txtLengthB.Text.Trim() + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + txtWidthA.Text.Trim() + "." + txtWidthB.Text.Trim() + "</mi></msub></mrow>"
                    //                                             + @"<br /><br />Total Area = " + AntiTermiteSquareFeetAndInchValue.ToString("0.00") + " " + "ft<sup>2</sup>"
                    //                                             + @"<br /><br />Total Quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Area (Square Meter)</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                    //                                             + @"<br /><br />Total Quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + AntiTermiteSquareMeterAndCMValue.ToString("0.000") + "</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                    //                                             + @"<br /><br />Total Quantity = " + lblQuantityAnswerAntiTermiteSquareMeterAndCMValue.Text;
                    //}
                    //else
                    //{
                    //    lblAntiTermiteFormula.Text = @"Total Area = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                    //                                             + @"<br /><br />Total Area = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + txtLengthA.Text.Trim() + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + txtWidthA.Text.Trim() + "</mi></msub></mrow>"
                    //                                             + @"<br /><br />Total Area = " + AntiTermiteSquareFeetAndInchValue.ToString("0.00") + " " + "ft<sup>2</sup>"
                    //                                             + @"<br /><br />Total Quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Total Area (Square Meter)</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                    //                                             + @"<br /><br />Total Quantity = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + AntiTermiteSquareMeterAndCMValue.ToString("0.000") + "</mi></msub><mo>&#xD7;</mo><msub><mi>30</mi></msub></mrow>"
                    //                                             + @"<br /><br />Total Quantity = " + lblQuantityAnswerAntiTermiteSquareMeterAndCMValue.Text;
                    //}
                    #endregion Formula For Feet/Inch
                }
            }
            catch (Exception ex)
            {
               // ucMessage.ShowError(ex.Message);
            }
        }

        #endregion Function Calculate Anti Termite Value For Feet And Inch

        #region Insert Log Function
        public void CalculatorLogInsert(AntiTermiteCalculator antitermite)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "AntiTermite-Calculator";

                if (antitermite.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(antitermite.UnitID);

                if (antitermite.LengthA != null)
                {
                    if (antitermite.LengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(antitermite.LengthA + "." + antitermite.LengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(antitermite.LengthA);
                }

                if (antitermite.WidthA != null)
                {
                    if (antitermite.WidthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(antitermite.WidthA + "." + antitermite.WidthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(antitermite.WidthA);
                }

                if (antitermite.UnitID == 1)
                {
                    if (ViewBag.lblAreaAnswerAntiTermiteSquareMeterAndCMValue != null && ViewBag.lblQuantityAnswerAntiTermiteSquareMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamD = ViewBag.lblAreaAnswerAntiTermiteSquareMeterAndCMValue;
                        entLOG_Calculation.ParamE = ViewBag.lblAreaAnswerAntiTermiteSquareFeetAndInchValue;
                        entLOG_Calculation.ParamF = ViewBag.lblQuantityAnswerAntiTermiteSquareMeterAndCMValue;
                    }
                }
                else
                {
                    if (ViewBag.lblAreaAnswerAntiTermiteSquareFeetAndInchValue != null)
                    {
                        entLOG_Calculation.ParamD = ViewBag.lblAreaAnswerAntiTermiteSquareFeetAndInchValue;
                        entLOG_Calculation.ParamE = ViewBag.lblAreaAnswerAntiTermiteSquareMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblQuantityAnswerAntiTermiteSquareMeterAndCMValue;
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
