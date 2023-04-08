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
    public class CostEstimatorCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Construction-Cost-Estimator-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Construction-Cost-Estimator-Calculator");


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

            return View("CostEstimatorCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(CostEstimatorCalculator costestimator)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Construction-Cost-Estimator-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            Calculate(costestimator);
            CalculatorLogInsert(costestimator);

            return PartialView("_CostEstimatorCalculatorResult", vModel);
        }
        #endregion

        #region Function: Calculate

        protected void Calculate(CostEstimatorCalculator costestimator)
        {
            try
            {
                if (costestimator.MeBuiltupmberType != null && costestimator.ApproxCost != null)
                {
                    ViewBag.lblAnswerSquareFeet = costestimator.MeBuiltupmberType;
                    ViewBag.lblAnswerSquareFeet2 = costestimator.MeBuiltupmberType;
                    Decimal Amount = Convert.ToDecimal(costestimator.MeBuiltupmberType) * Convert.ToDecimal(costestimator.ApproxCost);
                    ViewBag.lblAnswerAmount = Amount.ToString("#,##0.00") + " Rs.";

                    #region Cement Calculation

                    Decimal Cement = Convert.ToDecimal(costestimator.MeBuiltupmberType) * Convert.ToDecimal(0.4);
                    ViewBag.lblAnswerCement = Cement.ToString("0.00") + " Bags";
                    Decimal CementAmount = (Convert.ToDecimal(16.4) / 100) * Amount;
                    ViewBag.lblAnswerCementAmount = CementAmount.ToString("#,##0.00") + " Rs.";

                    #endregion Cement Calculation

                    #region Sand Calculation

                    Decimal Sand = Convert.ToDecimal(costestimator.MeBuiltupmberType) * Convert.ToDecimal(0.816);
                    ViewBag.lblAnswerSand = Sand.ToString("0.00") + " Ton";
                    Decimal SandAmount = (Convert.ToDecimal(12.3) / 100) * Amount;
                    ViewBag.lblAnswerSandAmount = SandAmount.ToString("#,##0.00") + " Rs.";

                    #endregion Sand Calculation

                    #region Aggregate Calculation

                    Decimal Aggregate = Convert.ToDecimal(costestimator.MeBuiltupmberType) * Convert.ToDecimal(0.608);
                    ViewBag.lblAnswerAggregate = Aggregate.ToString("0.00") + " Ton";
                    Decimal AggregateAmount = (Convert.ToDecimal(7.4) / 100) * Amount;
                    ViewBag.lblAnswerAggregateAmount = AggregateAmount.ToString("#,##0.00") + " Rs.";

                    #endregion Aggregate Calculation

                    #region Steel Calculation

                    Decimal Steel = Convert.ToDecimal(costestimator.MeBuiltupmberType) * Convert.ToDecimal(4);
                    ViewBag.lblAnswerSteel = Steel.ToString("0.00") + " Kg.";
                    Decimal SteelAmount = (Convert.ToDecimal(24.6) / 100) * Amount;
                    ViewBag.lblAnswerSteelAmount = SteelAmount.ToString("#,##0.00") + " Rs.";

                    #endregion Steel Calculation

                    #region Paint Calculation

                    Decimal Paint = Convert.ToDecimal(costestimator.MeBuiltupmberType) * Convert.ToDecimal(0.18);
                    ViewBag.lblAnswerPaint = Paint.ToString("0.00") + " lt";

                    #endregion Paint Calculation

                    #region Bricks Calculation

                    Decimal Bricks = Convert.ToDecimal(costestimator.MeBuiltupmberType) * Convert.ToDecimal(8);
                    ViewBag.lblAnswerBricks = Bricks.ToString("0.00") + " Pcs.";

                    #endregion Bricks Calculation

                    #region Flooring Calculation

                    Decimal Flooring = Convert.ToDecimal(costestimator.MeBuiltupmberType) * Convert.ToDecimal(1.3);
                    ViewBag.lblAnswerFlooring = Flooring.ToString("0.00") + " ft<sup>2</sup>";

                    #endregion Flooring Calculation

                    #region Finishers Amount Calculation

                    Decimal FinishersAmount = (Convert.ToDecimal(16.5) / 100) * Amount;
                    ViewBag.lblFinishersAmount = FinishersAmount.ToString("#,##0.00") + " Rs.";

                    #endregion Finishers Amount Calculation

                    #region Fittings Amount Calculation

                    Decimal FittingsAmount = (Convert.ToDecimal(22.8) / 100) * Amount;
                    ViewBag.lblFittingsAmount = FittingsAmount.ToString("#,##0.00") + " Rs.";

                    #endregion Fittings Amount Calculation

                    #region Total Cost Calculation

                    Decimal TotalCost = CementAmount + SandAmount + AggregateAmount + SteelAmount + FinishersAmount + FittingsAmount;
                    ViewBag.lblTotalCost = TotalCost.ToString("#,##0.00") + " Rs.";

                    #endregion Total Cost Calculation

                    #region Formula

                    ViewBag.lblTotalCoseFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Builtup area</mi><mo>&#xD7;</mo><mi>Approx cost per sq. ft.</mi></math>"
                                             + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + costestimator.MeBuiltupmberType + "</mi><mo>&#xD7;</mo><mi>" + costestimator.ApproxCost + "</mi></math>"
                                             + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Amount.ToString("0.00") + " Rs.</mi></math>";

                    ViewBag.lblCementFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Builtup area</mi><mo>&#xD7;</mo><mi>0.4</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + costestimator.MeBuiltupmberType + "</mi><mo>&#xD7;</mo><mi>0.4</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Cement.ToString("0.00") + " Bags</mi></math>"
                                            + "<hr/> <h4 class='bold'>Cement Amount</h4>"
                                            + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>16.4</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>Total cost</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>16.4</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>" + Amount.ToString("0.00") + "</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + CementAmount.ToString("0.00") + " Rs.</mi></math>";

                    ViewBag.lblSandFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Builtup area</mi><mo>&#xD7;</mo><mi>0.816</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + costestimator.MeBuiltupmberType + "</mi><mo>&#xD7;</mo><mi>0.816</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Sand.ToString("0.00") + " Ton</mi></math>"
                                            + "<hr/> <h4 class='bold'>Sand Amount</h4>"
                                            + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>12.3</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>Total cost</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>12.3</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>" + Amount.ToString("0.00") + "</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SandAmount.ToString("0.00") + " Rs.</mi></math>";

                    ViewBag.lblAggregateFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Builtup area</mi><mo>&#xD7;</mo><mi>0.608</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + costestimator.MeBuiltupmberType + "</mi><mo>&#xD7;</mo><mi>0.608</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Aggregate.ToString("0.00") + " Ton</mi></math>"
                                            + "<hr/> <h4 class='bold'>Aggregate Amount</h4>"
                                            + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>7.4</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>Total cost</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>7.4</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>" + Amount.ToString("0.00") + "</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + AggregateAmount.ToString("0.00") + " Rs.</mi></math>";

                    ViewBag.lblSteelFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Builtup area</mi><mo>&#xD7;</mo><mi>4</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + costestimator.MeBuiltupmberType + "</mi><mo>&#xD7;</mo><mi>4</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Steel.ToString("0.00") + " Kg.</mi></math>"
                                            + "<hr/> <h4 class='bold'>Steel Amount</h4>"
                                            + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>24.6</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>Total cost</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>24.6</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>" + Amount.ToString("0.00") + "</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SteelAmount.ToString("0.00") + " Rs.</mi></math>";

                    ViewBag.lblPaintFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Builtup area</mi><mo>&#xD7;</mo><mi>0.18</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + costestimator.MeBuiltupmberType + "</mi><mo>&#xD7;</mo><mi>0.18</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Paint.ToString("0.00") + " lt</mi></math>";

                    ViewBag.lblBricksFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Builtup area</mi><mo>&#xD7;</mo><mi>8</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + costestimator.MeBuiltupmberType + "</mi><mo>&#xD7;</mo><mi>8</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Bricks.ToString("0.00") + " Pcs.</mi></math>";

                    ViewBag.lblFlooringFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Builtup area</mi><mo>&#xD7;</mo><mi>1.3</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + costestimator.MeBuiltupmberType + "</mi><mo>&#xD7;</mo><mi>1.3</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Flooring.ToString("0.00") + " Sq. ft.</mi></math>";

                    ViewBag.lblFinishersFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>16.5</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>Total cost</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>16.5</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>" + Amount.ToString("0.00") + "</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + FinishersAmount.ToString("0.00") + " Rs.</mi></math>";

                    ViewBag.lblFittingsFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>22.8</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>Total cost</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mn>22.8</mn></mrow><mn>100</mn></mfrac><mo>&#xD7;</mo><mi>" + Amount.ToString("0.00") + "</mi></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + FittingsAmount.ToString("0.00") + " Rs.</mi></math>";

                    #endregion Formula

                   // ChartShow(TotalCost);
                }
            }
            catch (Exception ex)
            {
              //  ucMessage.ShowError(ex.Message);
            }
        }

        #endregion Function: Calculate

        #region Insert Log Function

        public void CalculatorLogInsert(CostEstimatorCalculator costestimator)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();


                #region Gather Data

                entLOG_Calculation.ScreenName = "Cost-Estimator-Calculator";

                if (costestimator.MeBuiltupmberType != null)
                    entLOG_Calculation.ParamA = Convert.ToString(costestimator.MeBuiltupmberType);

                if (costestimator.ApproxCost != null)
                    entLOG_Calculation.ParamB = Convert.ToString(costestimator.ApproxCost);

                if (ViewBag.lblAnswerAmount != null)
                    entLOG_Calculation.ParamC = Convert.ToString(ViewBag.lblAnswerAmount);

                if (ViewBag.lblAnswerCement != null)
                    entLOG_Calculation.ParamD = Convert.ToString(ViewBag.lblAnswerCement) + " Cement";

                if (ViewBag.lblAnswerCementAmount != null)
                    entLOG_Calculation.ParamE = Convert.ToString(ViewBag.lblAnswerCementAmount);

                if (ViewBag.lblAnswerSand != null)
                    entLOG_Calculation.ParamF = Convert.ToString(ViewBag.lblAnswerSand) + " Sand";

                if (ViewBag.lblAnswerSandAmount != null)
                    entLOG_Calculation.ParamG = Convert.ToString(ViewBag.lblAnswerSandAmount);

                if (ViewBag.lblAnswerAggregate != null)
                    entLOG_Calculation.ParamH = Convert.ToString(ViewBag.lblAnswerAggregate) + " Aggregate";

                if (ViewBag.lblAnswerAggregateAmount != null)
                    entLOG_Calculation.ParamI = Convert.ToString(ViewBag.lblAnswerAggregateAmount);

                if (ViewBag.lblAnswerSteel != null)
                    entLOG_Calculation.ParamJ = Convert.ToString(ViewBag.lblAnswerSteel) + " Steel";

                if (ViewBag.lblAnswerSteelAmount != null)
                    entLOG_Calculation.ParamK = Convert.ToString(ViewBag.lblAnswerSteelAmount);

                if (ViewBag.lblAnswerPaint != null)
                    entLOG_Calculation.ParamL = Convert.ToString(ViewBag.lblAnswerPaint) + " Paint";

                if (ViewBag.lblAnswerBricks != null)
                    entLOG_Calculation.ParamM = Convert.ToString(ViewBag.lblAnswerBricks) + " Bricks";

                if (ViewBag.lblAnswerFlooring != null)
                    entLOG_Calculation.ParamN = Convert.ToString(ViewBag.lblAnswerFlooring) + " Flooring";

                if (ViewBag.lblFinishersAmount != null)
                    entLOG_Calculation.ParamO = Convert.ToString(ViewBag.lblFinishersAmount) + " Finishers Amount";

                if (ViewBag.lblFittingsAmount != null)
                    entLOG_Calculation.ParamP = Convert.ToString(ViewBag.lblFittingsAmount) + " Fittings Amount";

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
