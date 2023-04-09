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
    public class PCCCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/PCC-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/PCC-Calculator");


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

            return View("PCCCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(PCCCalculator pcc)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/PCC-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (pcc.UnitID == 1)
                CalculatePCCValueForMeterAndCM(pcc);
            else if (pcc.UnitID == 2)
                CalculatePCCValueForFeetAndInch(pcc);
            else
                CalculatePCCValueForMeterAndCM(pcc);

            CalculatorLogInsert(pcc);

            return PartialView("_PCCCalculatorResult", vModel);
        }
        #endregion

        #region Function CalculatePCCValueForMeterAndCM

        protected void CalculatePCCValueForMeterAndCM(PCCCalculator pcc)
        {
            if (pcc.LengthA != null && pcc.BreadthA != null && pcc.DepthA != null)
            {
                #region Calculate Meter/CM Value

                Decimal PCCCubicMeterAndCMValue = 0;
                PCCCubicMeterAndCMValue = Convert.ToDecimal(pcc.LengthA + "." + pcc.LengthB) * Convert.ToDecimal(pcc.BreadthA + "." + pcc.BreadthB) * Convert.ToDecimal(pcc.DepthA + "." + pcc.DepthB);
                ViewBag.lblAnswerPCCCubicMeterAndCMValue = PCCCubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";
                //ViewBag.lblPCCMeterAndCMValue = "PCC M<sup>3</sup> Cubic Meter";

                #endregion Calculate Meter/CM Value

                #region Calculate Feet/Inch Value

                Decimal PCCCubicFeetAndInchValue = 0;
                PCCCubicFeetAndInchValue = PCCCubicMeterAndCMValue * Convert.ToDecimal(35.3147);
                ViewBag.lblAnswerPCCCubicFeetAndInchValue = PCCCubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";
                //ViewBag.lblPCCFeetAndInchValue = "PCC Ft<sup>3</sup> Cubic Feet";

                #endregion Calculate Feet/Inch Value

                #region Calculate Cement, Sand and Aggregate

                Decimal PCCCubicValue = 0;
                PCCCubicValue = Convert.ToDecimal(pcc.LengthA + "." + pcc.LengthB) * Convert.ToDecimal(pcc.BreadthA + "." + pcc.BreadthB) * Convert.ToDecimal(pcc.DepthA + "." + pcc.DepthB);

                Decimal Cement = 0, Sand = 0, Aggregate = 0, Wtofcement = 0;

                Wtofcement = Convert.ToDecimal(1 / Convert.ToDecimal(pcc.GradeID.ToString()) * Convert.ToDecimal(1.52)) * PCCCubicValue;
                Cement = Wtofcement / Convert.ToDecimal(0.035);
                Decimal CementAnswer = Convert.ToDecimal(Cement.ToString("0.00"));
                ViewBag.lblAnswerPCCCement = Math.Ceiling(CementAnswer).ToString("0.00");

                if (pcc.GradeID == Convert.ToDecimal(5.5))
                {
                    Sand = Wtofcement * Convert.ToDecimal(1.5) * 1550;
                    ViewBag.lblAnswerPCCSand = Sand.ToString("0.00");
                    Aggregate = Wtofcement * Convert.ToDecimal(3) * 1350;
                    ViewBag.lblAnswerPCCAggregate = Aggregate.ToString("0.00");
                }
                else if (pcc.GradeID == 7)
                {
                    Sand = Wtofcement * Convert.ToDecimal(2) * 1550;
                    ViewBag.lblAnswerPCCSand = Sand.ToString("0.00");
                    Aggregate = Wtofcement * Convert.ToDecimal(4) * 1350;
                    ViewBag.lblAnswerPCCAggregate = Aggregate.ToString("0.00");
                }
                else if (pcc.GradeID == 10)
                {
                    Sand = Wtofcement * Convert.ToDecimal(3) * 1550;
                    ViewBag.lblAnswerPCCSand = Sand.ToString("0.00");
                    Aggregate = Wtofcement * Convert.ToDecimal(6) * 1350;
                    ViewBag.lblAnswerPCCAggregate = Aggregate.ToString("0.00");
                }
                else if (pcc.GradeID == 13)
                {
                    Sand = Wtofcement * Convert.ToDecimal(4) * 1550;
                    ViewBag.lblAnswerPCCSand = Sand.ToString("0.00");
                    Aggregate = Wtofcement * Convert.ToDecimal(8) * 1350;
                    ViewBag.lblAnswerPCCAggregate = Aggregate.ToString("0.00");
                }

                #endregion Calculate Cement, Sand and Aggregate

                #region Formula For Meter/CM

                if (pcc.GradeID == Convert.ToDecimal(5.5))
                {
                    if (pcc.LengthB != null || pcc.BreadthB != null || pcc.DepthB != null)
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "." + pcc.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "." + pcc.BreadthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "." + pcc.DepthB + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicMeterAndCMValue.ToString("0.00") + " " + "Cubic meter (m<sup>3</sup>)";
                    }
                    else
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicMeterAndCMValue.ToString("0.00") + " " + "Cubic meter (m<sup>3</sup>)";
                    }

                    ViewBag.lblPCCCement = @"<h4><b>Amount of Cement Required</b></h4>"
                                        + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>Wt. of Cement</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>" + Wtofcement.ToString("0.00") + "</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= " + Math.Ceiling(CementAnswer).ToString("0.00") + " bags";

                    ViewBag.lblPCCSand = @"<h4><b>Amount of Sand Required</b></h4>"
                                    + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>1.5</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>1.5</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= " + Sand.ToString("0.00") + " kg";

                    ViewBag.lblPCCAggregate = @"<h4><b>Amount of Aggregate Required</b></h4>"
                                            + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>3</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>3</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= " + Aggregate.ToString("0.00") + " kg";
                }
                else if (pcc.GradeID == 7)
                {
                    if (pcc.LengthB != null || pcc.BreadthB != null || pcc.DepthB != null)
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "." + pcc.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "." + pcc.BreadthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "." + pcc.DepthB + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicMeterAndCMValue.ToString("0.00") + " " + "Cubic meter (m<sup>3</sup>)";
                    }
                    else
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicMeterAndCMValue.ToString("0.00") + " " + "Cubic meter (m<sup>3</sup>)";
                    }

                    ViewBag.lblPCCCement = @"<h4><b>Amount of Cement Required</b></h4>"
                                        + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>Wt. of Cement</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>" + Wtofcement.ToString("0.00") + "</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= " + Math.Ceiling(CementAnswer).ToString("0.00") + " bags";

                    ViewBag.lblPCCSand = @"<h4><b>Amount of Sand Required</b></h4>"
                                    + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>2</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>2</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= " + Sand.ToString("0.00") + " kg";

                    ViewBag.lblPCCAggregate = @"<h4><b>Amount of Aggregate Required</b></h4>"
                                        + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>4</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>4</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                        + @"<br /><br />= " + Aggregate.ToString("0.00") + " kg";
                }
                else if (pcc.GradeID == 10)
                {
                    if (pcc.LengthB != null || pcc.BreadthB != null || pcc.DepthB != null)
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "." + pcc.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "." + pcc.BreadthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "." + pcc.DepthB + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicMeterAndCMValue.ToString("0.00") + " " + "Cubic meter (m<sup>3</sup>)";
                    }
                    else
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicMeterAndCMValue.ToString("0.00") + " " + "Cubic meter (m<sup>3</sup>)";
                    }

                    ViewBag.lblPCCCement = @"<h4><b>Amount of Cement Required</b></h4>"
                                        + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>Wt. of Cement</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>" + Wtofcement.ToString("0.00") + "</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= " + Math.Ceiling(CementAnswer).ToString("0.00") + " bags";

                    ViewBag.lblPCCSand = @"<h4><b>Amount of Sand Required</b></h4>"
                                    + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>3</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>3</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= " + Sand.ToString("0.00") + " kg";

                    ViewBag.lblPCCAggregate = @"<h4><b>Amount of Aggregate Required</b></h4>"
                                            + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>6</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>6</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= " + Aggregate.ToString("0.00") + " kg";
                }
                else if (pcc.GradeID == 13)
                {
                    if (pcc.LengthB != null || pcc.BreadthB != null || pcc.DepthB != null)
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "." + pcc.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "." + pcc.BreadthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "." + pcc.DepthB + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicMeterAndCMValue.ToString("0.00") + " " + "Cubic meter (m<sup>3</sup>)";
                    }
                    else
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicMeterAndCMValue.ToString("0.00") + " " + "Cubic meter (m<sup>3</sup>)";
                    }

                    ViewBag.lblPCCCement = @"<h4><b>Amount of Cement Required</b></h4>"
                                        + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>Wt. of Cement</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>" + Wtofcement.ToString("0.00") + "</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= " + Math.Ceiling(CementAnswer).ToString("0.00") + " bags";

                    ViewBag.lblPCCSand = @"<h4><b>Amount of Sand Required</b></h4>"
                                    + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>4</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>4</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= " + Sand.ToString("0.00") + " kg";

                    ViewBag.lblPCCAggregate = @"<h4><b>Amount of Aggregate Required</b></h4>"
                                            + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>8</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>8</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= " + Aggregate.ToString("0.00") + " kg";
                }
                #endregion Formula For Meter/CM

                #region ChartValue
                //ChartShow(CementBags * 50, SandTon * 1000, AggregatesTon * 1000);
                #region Load Chart
                ViewBag.ChartCement = Math.Round(CementAnswer * 50);
                ViewBag.ChartSand = Math.Round(Sand * 1000);
                ViewBag.ChartBrick = Math.Round(Aggregate * 1000);
                #endregion Load Chart
                #endregion ChartValue
            }
        }

        #endregion Function CalculatePCCValueForMeterAndCM

        #region Function CalculatePCCValueForFeetAndInch

        protected void CalculatePCCValueForFeetAndInch(PCCCalculator pcc)
        {
            if (pcc.LengthA != null && pcc.BreadthA != null && pcc.DepthA != null)
            {
                #region Calculate Feet/Inch Value

                Decimal PCCCubicFeetAndInchValue = 0;
                PCCCubicFeetAndInchValue = Convert.ToDecimal(pcc.LengthA + "." + pcc.LengthB) * Convert.ToDecimal(pcc.BreadthA + "." + pcc.BreadthB) * Convert.ToDecimal(pcc.DepthA + "." + pcc.DepthB);
                ViewBag.lblAnswerPCCCubicFeetAndInchValue = PCCCubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";
                //ViewBag.lblPCCFeetAndInchValue = "PCC Ft<sup>3</sup> Cubic Feet";

                #endregion Calculate Feet/Inch Value

                #region Calculate Meter/CM Value

                Decimal PCCCubicMeterAndCMValue = 0;
                PCCCubicMeterAndCMValue = PCCCubicFeetAndInchValue / Convert.ToDecimal(35.3147);
                ViewBag.lblAnswerPCCCubicMeterAndCMValue = PCCCubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";
                //ViewBag.lblPCCMeterAndCMValue = "PCC M<sup>3</sup> Cubic Meter";

                #endregion Calculate Meter/CM Value

                #region Calculate Cement, Sand and Aggregate

                Decimal PCCCubicValue = 0;
                PCCCubicValue = Convert.ToDecimal(pcc.LengthA + "." + pcc.LengthB) * Convert.ToDecimal(pcc.BreadthA + "." + pcc.BreadthB) * Convert.ToDecimal(pcc.DepthA + "." + pcc.DepthB) / 35.3147m;

                Decimal Cement = 0, Sand = 0, Aggregate = 0, Wtofcement = 0;

                Wtofcement = Convert.ToDecimal(1 / Convert.ToDecimal(pcc.GradeID.ToString()) * Convert.ToDecimal(1.52)) * PCCCubicValue;
                Cement = Wtofcement / Convert.ToDecimal(0.035);
                Decimal CementAnswer = Convert.ToDecimal(Cement.ToString("0.00"));
                ViewBag.lblAnswerPCCCement = Math.Ceiling(CementAnswer).ToString("0.00");

                if (pcc.GradeID == Convert.ToDecimal(5.5))
                {
                    Sand = Wtofcement * Convert.ToDecimal(1.5) * 1550;
                    ViewBag.lblAnswerPCCSand = Sand.ToString("0.00");
                    Aggregate = Wtofcement * Convert.ToDecimal(3) * 1350;
                    ViewBag.lblAnswerPCCAggregate = Aggregate.ToString("0.00");
                }
                else if (pcc.GradeID == 7)
                {
                    Sand = Wtofcement * Convert.ToDecimal(2) * 1550;
                    ViewBag.lblAnswerPCCSand = Sand.ToString("0.00");
                    Aggregate = Wtofcement * Convert.ToDecimal(4) * 1350;
                    ViewBag.lblAnswerPCCAggregate = Aggregate.ToString("0.00");
                }
                else if (pcc.GradeID == 10)
                {
                    Sand = Wtofcement * Convert.ToDecimal(3) * 1550;
                    ViewBag.lblAnswerPCCSand = Sand.ToString("0.00");
                    Aggregate = Wtofcement * Convert.ToDecimal(6) * 1350;
                    ViewBag.lblAnswerPCCAggregate = Aggregate.ToString("0.00");
                }
                else if (pcc.GradeID == 13)
                {
                    Sand = Wtofcement * Convert.ToDecimal(4) * 1550;
                    ViewBag.lblAnswerPCCSand = Sand.ToString("0.00");
                    Aggregate = Wtofcement * Convert.ToDecimal(8) * 1350;
                    ViewBag.lblAnswerPCCAggregate = Aggregate.ToString("0.00");
                }

                #endregion Calculate Cement, Sand and Aggregate

                #region Formula For Meter/CM

                if (pcc.GradeID == Convert.ToDecimal(5.5))
                {

                    if (pcc.LengthB != null || pcc.BreadthB != null || pcc.DepthB != null)
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "." + pcc.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "." + pcc.BreadthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "." + pcc.DepthB + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicFeetAndInchValue.ToString("0.00") + " " + "Cubic feet (ft<sup>3</sup>)";
                    }
                    else
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicFeetAndInchValue.ToString("0.00") + " " + "Cubic feet (ft<sup>3</sup>)";
                    }

                    ViewBag.lblPCCCement = @"<h4><b>Amount of Cement Required</b></h4>"
                                        + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>Wt. of Cement</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>" + Wtofcement.ToString("0.00") + "</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= " + Math.Ceiling(CementAnswer).ToString("0.00") + " bags";

                    ViewBag.lblPCCSand = @"<h4><b>Amount of Sand Required</b></h4>"
                                    + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>1.5</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>1.5</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= " + Sand.ToString("0.00") + " kg";

                    ViewBag.lblPCCAggregate = @"<h4><b>Amount of Aggregate Required</b></h4>"
                                            + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>3</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>3</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= " + Aggregate.ToString("0.00") + " kg";
                }
                else if (pcc.GradeID == 7)
                {
                    if (pcc.LengthB != null || pcc.BreadthB != null || pcc.DepthB != null)
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "." + pcc.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "." + pcc.BreadthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "." + pcc.DepthB + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicFeetAndInchValue.ToString("0.00") + " " + "Cubic feet (ft<sup>3</sup>)";
                    }
                    else
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicFeetAndInchValue.ToString("0.00") + " " + "Cubic feet (ft<sup>3</sup>)";
                    }

                    ViewBag.lblPCCCement = @"<h4><b>Amount of Cement Required</b></h4>"
                                        + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>Wt. of Cement</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>" + Wtofcement.ToString("0.00") + "</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= " + Math.Ceiling(CementAnswer).ToString("0.00") + " bags";

                    ViewBag.lblPCCSand = @"<h4><b>Amount of Sand Required</b></h4>"
                                    + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>2</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>2</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= " + Sand.ToString("0.00") + " kg";

                    ViewBag.lblPCCAggregate = @"<h4><b>Amount of Aggregate Required</b></h4>"
                                            + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>4</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>4</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= " + Aggregate.ToString("0.00") + " kg";
                }
                else if (pcc.GradeID == 10)
                {
                    if (pcc.LengthB != null || pcc.BreadthB != null || pcc.DepthB != null)
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "." + pcc.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "." + pcc.BreadthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "." + pcc.DepthB + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicFeetAndInchValue.ToString("0.00") + " " + "Cubic feet (ft<sup>3</sup>)";
                    }
                    else
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicFeetAndInchValue.ToString("0.00") + " " + "Cubic feet (ft<sup>3</sup>)";
                    }

                    ViewBag.lblPCCCement = @"<h4><b>Amount of Cement Required</b></h4>"
                                        + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>Wt. of Cement</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>" + Wtofcement.ToString("0.00") + "</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= " + Math.Ceiling(CementAnswer).ToString("0.00") + " bags";

                    ViewBag.lblPCCSand = @"<h4><b>Amount of Sand Required</b></h4>"
                                    + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>3</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>3</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= " + Sand.ToString("0.00") + " kg";

                    ViewBag.lblPCCAggregate = @"<h4><b>Amount of Aggregate Required</b></h4>"
                                            + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>6</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>6</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= " + Aggregate.ToString("0.00") + " kg";
                }
                else if (pcc.GradeID == 13)
                {
                    if (pcc.LengthB != null || pcc.BreadthB != null || pcc.DepthB != null)
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "." + pcc.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "." + pcc.BreadthB + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "." + pcc.DepthB + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicFeetAndInchValue.ToString("0.00") + " " + "Cubic feet (ft<sup>3</sup>)";
                    }
                    else
                    {
                        ViewBag.lblPCCVolume = @"<h4><b>PCC Volume</b></h4>"
                                        + @"=<math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Breadth</mi></msub><mo>&#xD7;</mo><msub><mi>Depth</mi></msub></mrow>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + pcc.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + pcc.BreadthA + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + pcc.DepthA + "</mi></msub></mrow>"
                                        + @"<br /><br />= " + PCCCubicFeetAndInchValue.ToString("0.00") + " " + "Cubic feet (ft<sup>3</sup>)";
                    }

                    ViewBag.lblPCCCement = @"<h4><b>Amount of Cement Required</b></h4>"
                                        + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>Wt. of Cement</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfrac><mrow><msub><mn>" + Wtofcement.ToString("0.00") + "</mn></msub></mrow><msub><mn>0.035</mn></msub></mfrac></math>"
                                        + @"<br /><br />= " + Math.Ceiling(CementAnswer).ToString("0.00") + " bags";

                    ViewBag.lblPCCSand = @"<h4><b>Amount of Sand Required</b></h4>"
                                    + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>4</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>4</mi></msub><mo>&#xD7;</mo><msub><mi>1550</mi></msub></mrow>"
                                    + @"<br /><br />= " + Sand.ToString("0.00") + " kg";

                    ViewBag.lblPCCAggregate = @"<h4><b>Amount of Aggregate Required</b></h4>"
                                            + @"= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Wt. of Cement</mi></msub><mo>&#xD7;</mo><msub><mi>8</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>" + Wtofcement.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>8</mi></msub><mo>&#xD7;</mo><msub><mi>1350</mi></msub></mrow>"
                                            + @"<br /><br />= " + Aggregate.ToString("0.00") + " kg";
                }
                #endregion Formula For Meter/CM

                #region ChartValue
                //ChartShow(CementBags * 50, SandTon * 1000, AggregatesTon * 1000);
                #region Load Chart
                ViewBag.ChartCement = Math.Round(CementAnswer * 50);
                ViewBag.ChartSand = Math.Round(Sand * 1000);
                ViewBag.ChartBrick = Math.Round(Aggregate * 1000);
                #endregion Load Chart
                #endregion ChartValue
            }
        }

        #endregion Function CalculatePCCValueForFeetAndInch

        #region Insert Log Function
        public void CalculatorLogInsert(PCCCalculator pcc)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region 15.2 Gather Data
                entLOG_Calculation.ScreenName = "PCC-Calculator";

                if (pcc.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(pcc.UnitID);

                if (pcc.LengthA != null)
                {
                    if (pcc.LengthB != null)
                    {
                        entLOG_Calculation.ParamB = Convert.ToString(pcc.LengthA + "." + pcc.LengthB);
                    }
                    else
                    {
                        entLOG_Calculation.ParamB = Convert.ToString(pcc.LengthA);
                    }
                }

                if (pcc.BreadthA != null)
                {
                    if (pcc.BreadthB != null)
                    {
                        entLOG_Calculation.ParamC = Convert.ToString(pcc.BreadthA + "." + pcc.BreadthB);
                    }
                    else
                    {
                        entLOG_Calculation.ParamC = Convert.ToString(pcc.BreadthA);
                    }
                }


                if (pcc.DepthA != null)
                {
                    if (pcc.DepthB != null)
                    {
                        entLOG_Calculation.ParamD = Convert.ToString(pcc.DepthA + "." + pcc.DepthB);
                    }
                    else
                    {
                        entLOG_Calculation.ParamD = Convert.ToString(pcc.DepthA);
                    }
                }


                if (pcc.UnitID == 0)
                {
                    if (ViewBag.lblAnswerPCCCubicMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerPCCCubicMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerPCCCubicFeetAndInchValue;
                    }
                }
                else if (pcc.UnitID == 1)
                {
                    if (ViewBag.lblAnswerPCCCubicFeetAndInchValue != null)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerPCCCubicFeetAndInchValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerPCCCubicMeterAndCMValue;
                    }
                }
                else
                {
                    if (ViewBag.lblAnswerPCCCubicMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerPCCCubicMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerPCCCubicFeetAndInchValue;
                    }
                }

                entLOG_Calculation.ParamG = ViewBag.lblAnswerPCCCement;
                entLOG_Calculation.ParamH = ViewBag.lblAnswerPCCSand;
                entLOG_Calculation.ParamI = ViewBag.lblAnswerPCCAggregate;

                if (pcc.GradeID == 0)
                {
                    entLOG_Calculation.ParamJ = Convert.ToString(pcc.GradeID);
                }
                else if (pcc.GradeID == 1)
                {
                    entLOG_Calculation.ParamJ = Convert.ToString(pcc.GradeID);
                }
                else if (pcc.GradeID == 2)
                {
                    entLOG_Calculation.ParamJ = Convert.ToString(pcc.GradeID);
                }
                else if (pcc.GradeID == 3)
                {
                    entLOG_Calculation.ParamJ = Convert.ToString(pcc.GradeID);
                }

                entLOG_Calculation.Created = DateTime.Now;
                entLOG_Calculation.Modified = DateTime.Now;
                #endregion 15.2 Gather Data

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
