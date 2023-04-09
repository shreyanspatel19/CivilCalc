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
    public class ConcreteTubeCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Concrete-Tube-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Concrete-Tube-Calculator");


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

            return View("ConcreteTubeCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(ConcreteTubeCalculator concretetube)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Concrete-Tube-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (concretetube.UnitID == 1)
                CalculateValueForMeterAndCM(concretetube);
            else
                CalculateValueForFeetAndInch(concretetube);

            CalculatorLogInsert(concretetube);

            return PartialView("_ConcreteTubeCalculatorResult", vModel);
        }
        #endregion

        #region Function: Calculate Value For Meter And CM

        protected void CalculateValueForMeterAndCM(ConcreteTubeCalculator concretetube)
        {
            try
            {
                if (concretetube.InnerDiameterA != null && concretetube.OuterDiameterA != null && concretetube.HeightA != null)
                {
                    #region variables

                    Decimal InnerDiameter = 0, OuterDiameter = 0, InnerArea = 0, OuterArea = 0, TotalArea = 0, Volume = 0m, DryVolume = 0m, Cement = 0m, CementBag = 0m, Sand = 0m, SandInTon = 0m, Aggregate = 0m, AggregateInTon = 0m;

                    #endregion variables

                    #region Convert Diameter To Radius

                    if (concretetube.InnerDiameterB != null)
                        InnerDiameter = Convert.ToDecimal(concretetube.InnerDiameterA + "." + concretetube.InnerDiameterB) / 2;
                    else
                        InnerDiameter = Convert.ToDecimal(concretetube.InnerDiameterA) / 2;

                    if (concretetube.OuterDiameterB != null)
                        OuterDiameter = Convert.ToDecimal(concretetube.OuterDiameterA + "." + concretetube.OuterDiameterB) / 2;
                    else
                        OuterDiameter = Convert.ToDecimal(concretetube.OuterDiameterA) / 2;

                    #endregion Convert Diameter To Radius

                    #region Calculate Inner Area

                    InnerArea = Convert.ToDecimal(Math.PI) * InnerDiameter * InnerDiameter;

                    #endregion Calculate Inner Area

                    #region Calculate Outer Area

                    OuterArea = Convert.ToDecimal(Math.PI) * OuterDiameter * OuterDiameter;

                    #endregion Calculate Outer Area

                    #region Calculate Total Area

                    TotalArea = OuterArea - InnerArea;

                    #endregion Calculate Total Area

                    #region Calculate Volume

                    if (concretetube.HeightB != null)
                        Volume = TotalArea * Convert.ToDecimal(concretetube.HeightA + "." + concretetube.HeightB) * Convert.ToDecimal(concretetube.ColumnNo);
                    else
                        Volume = TotalArea * Convert.ToDecimal(concretetube.HeightA) * Convert.ToDecimal(concretetube.ColumnNo);

                    #endregion Calculate Volume

                    #region Convert Meter/CM to Feet/Inch

                    Decimal CubicFeetInch = CommonFunctions.ConvertFeetAndInchForVolume(Volume);
                    ViewBag.lblAnswerCubicMeterAndCMValue = Volume.ToString("0.00") + " m<sup>3</sup>";
                   ViewBag.lblAnswerCubicFeetAndInchValue = CubicFeetInch.ToString("0.00") + " ft<sup>3</sup>";

                    #endregion Convert Meter/CM to Feet/Inch

                    #region Calculate Dry Volume

                    DryVolume = Volume * Convert.ToDecimal(1.524);

                    #endregion Calculate Dry Volume

                    #region Calculate Cement

                    Cement = (DryVolume * 1) / (Convert.ToDecimal(concretetube.GradeID));
                    CementBag = Cement / Convert.ToDecimal(0.035);
                    ViewBag.lblAnswerCement = CementBag.ToString("0") + " Bags";

                    #endregion Calculate Cement

                    #region Calculate Sand And Aggregate

                    if (concretetube.GradeID == Convert.ToDecimal(5.5))
                    {
                        #region Calculate Sand

                        Sand = (DryVolume * Convert.ToDecimal(1.5)) / (Convert.ToDecimal(concretetube.GradeID));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregate

                        Aggregate = (DryVolume * 3) / (Convert.ToDecimal(concretetube.GradeID));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregate
                    }
                    else if (concretetube.GradeID == 7)
                    {
                        #region Calculate Sand

                        Sand = (DryVolume * 2) / (Convert.ToDecimal(concretetube.GradeID));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregate

                        Aggregate = (DryVolume * 4) / (Convert.ToDecimal(concretetube.GradeID));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregate
                    }
                    else if (concretetube.GradeID == 10)
                    {
                        #region Calculate Sand

                        Sand = (DryVolume * 3) / (Convert.ToDecimal(concretetube.GradeID));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregate

                        Aggregate = (DryVolume * 6) / (Convert.ToDecimal(concretetube.GradeID));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregate
                    }
                    else if (concretetube.GradeID == 13)
                    {
                        #region Calculate Sand

                        Sand = (DryVolume * 4) / (Convert.ToDecimal(concretetube.GradeID));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregate

                        Aggregate = (DryVolume * 8) / (Convert.ToDecimal(concretetube.GradeID));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregate
                    }
                    #endregion Calculate Sand And Aggregate

                    #region Load Chart

                   // ChartShow(CementBag * 50, SandInTon * 1000, AggregateInTon * 1000);

                    #endregion Load Chart

                    #region Formula

                    #region InnerRadius Formula

                    if (concretetube.InnerDiameterB != null)
                    {
                        ViewBag.lblInnerAreaFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><msup><mi>&#x3C0;r</mi><mn>2</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>Diameter</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/> <math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>" + concretetube.InnerDiameterA + "." + concretetube.InnerDiameterB + "</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><mn>3.14</mn><mo>&#xD7;</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><mi>" + InnerArea.ToString("0.00") + "</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblInnerAreaFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><msup><mi>&#x3C0;r</mi><mn>2</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>Diameter</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mi>" + concretetube.InnerDiameterA + "</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><mn>3.14</mn><mo>&#xD7;</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><mi>" + InnerArea.ToString("0.00") + "</mi></math>";
                    }

                    #endregion InnerRadius Formula

                    #region OuterRadius Formula

                    if (concretetube.OuterDiameterB != null)
                    {
                        ViewBag.lblOuterAreaFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><msup><mi>&#x3C0;r</mi><mn>2</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>Diameter</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/> <math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>" + concretetube.OuterDiameterA + "." + concretetube.OuterDiameterB + "</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><mn>3.14</mn><mo>&#xD7;</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><mi>" + OuterArea.ToString("0.00") + "</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblOuterAreaFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Volume =</mo><msup><mi>&#x3C0;r</mi><mn>2</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>Diameter</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mi>" + concretetube.OuterDiameterA + "</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><mn>3.14</mn><mo>&#xD7;</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><mi>" + OuterArea.ToString("0.00") + "</mi></math>";
                    }

                    #endregion OuterRadius Formula

                    #region Volume Formula

                    if (concretetube.HeightB != null)
                    {
                        ViewBag.lblVolumeFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Area=</mo><mi>Outer Tube Area - Inner Tube Area</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Area =</mo><mi>" + InnerArea.ToString("0.00") + "</mi><mo>-</mo><mi>" + OuterArea.ToString("0.00") + "</mi><mo>=</mo><mi>" + TotalArea.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mi>Area</mi><mo>×</mo><mi>Height</mi><mo>×</mo><mi>No of Column</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mi>" + TotalArea.ToString("0.00") + "</mi><mo>×</mo><mi>" + concretetube.HeightA + "</mi><mi>.</mi><mi>" + concretetube.HeightB + "</mi><mo>×</mo><mi>" + concretetube.ColumnNo + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mn>" + Volume.ToString("0.00") + "</mn><msup><mi>m</mi><mn>3</mn></msup><mi> or </mi><mn>" + CubicFeetInch.ToString("0.00") + "</mn><msup><mi>ft</mi><mn>3</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Dry Volume=</mo><mn>" + Volume.ToString("0.00") + "</mn><mo>×</mo><mi>1.524 </mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Dry Volume=</mo><mn>" + DryVolume.ToString("0.00") + "</mn></math>";
                    }
                    else
                    {
                        ViewBag.lblVolumeFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Area=</mo><mi>Outer Tube Area - Inner Tube Area</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Area =</mo><mi>" + InnerArea.ToString("0.00") + "</mi><mo>-</mo><mi>" + OuterArea.ToString("0.00") + "</mi><mo>=</mo><mi>" + TotalArea.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mi>Area</mi><mo>×</mo><mi>Height</mi><mo>×</mo><mi>No of Column</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mi>" + TotalArea.ToString("0.00") + "</mi><mo>×</mo><mi>" + concretetube.HeightA + "</mi><mo>×</mo><mi>" + concretetube.ColumnNo + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mn>" + Volume.ToString("0.00") + "</mn><msup><mi>m</mi><mn>3</mn></msup><mi> or </mi><mn>" + CubicFeetInch.ToString("0.00") + "</mn><msup><mi>ft</mi><mn>3</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Dry Volume=</mo><mn>" + Volume.ToString("0.00") + "</mn><mo>×</mo><mi>1.524 </mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Dry Volume=</mo><mn>" + DryVolume.ToString("0.00") + "</mn></math>";
                    }

                    #endregion Volume Formula

                    #region Cement Formula

                    ViewBag.lblCementFormula = "<h4>Cement Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Cement</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>1</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Cement.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>No. of Cement Bags</h4>"
                                            + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Cement Volume</mi></mrow><mrow><mn>0.035</mn></mrow></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + Cement.ToString("0.00") + "</mi></mrow><mrow><mn>0.035</mn></mrow></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Math.Ceiling(CementBag).ToString("0") + " Bags</mi></math>";

                    #endregion Formula

                    #region Sand and Aggregate Formula

                    if (concretetube.GradeID == Convert.ToDecimal(5.5))
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>1.5</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Sand.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Sand in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Sand.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + SandInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Sand Formula

                        #region Aggregate Formula

                        ViewBag.lblAggregateFormula = "<h4>Aggregate Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Aggregate</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }
                    else if (concretetube.GradeID == 7)
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>1.5</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Sand.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Sand in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Sand.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + SandInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Sand Formula

                        #region Aggregate Formula

                        ViewBag.lblAggregateFormula = "<h4>Aggregate Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Aggregate</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }
                    else if (concretetube.GradeID == 10)
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Sand.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Sand in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Sand.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + SandInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Sand Formula

                        #region Aggregate Formula

                        ViewBag.lblAggregateFormula = "<h4>Aggregate Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Aggregate</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>6</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }
                    else if (concretetube.GradeID == 13)
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>4</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Sand.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Sand in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Sand.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + SandInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Sand Formula

                        #region Aggregate Formula

                        ViewBag.lblAggregateFormula = "<h4>Aggregate Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Aggregate</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>8</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }

                    #endregion Sand and Aggregate Formula

                    #endregion Formula
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function: Calculate Value For Meter And CM

        #region Function: Calculate Value For Feet And Inch

        protected void CalculateValueForFeetAndInch(ConcreteTubeCalculator concretetube)
        {
            try
            {
                if (concretetube.InnerDiameterA != null && concretetube.OuterDiameterA != null && concretetube.HeightA != null)
                {
                    #region Variables

                    Decimal InnerDiameter = 0, OuterDiameter = 0, InnerArea = 0, OuterArea = 0, TotalArea = 0, Volume = 0m, DryVolume = 0m, Cement = 0m, CementBag = 0m, Sand = 0m, SandInTon = 0m, Aggregate = 0m, AggregateInTon = 0m;

                    #endregion Variables

                    #region Calculate Diameter

                    if (concretetube.InnerDiameterB != null)
                        InnerDiameter = Convert.ToDecimal(concretetube.InnerDiameterA + "." + concretetube.InnerDiameterB) / 2;
                    else
                        InnerDiameter = Convert.ToDecimal(concretetube.InnerDiameterA) / 2;

                    if (concretetube.OuterDiameterB != null)
                        OuterDiameter = Convert.ToDecimal(concretetube.OuterDiameterA + "." + concretetube.OuterDiameterB) / 2;
                    else
                        OuterDiameter = Convert.ToDecimal(concretetube.OuterDiameterA) / 2;

                    #endregion Calculate Diameter

                    #region Calculate Inner Area

                    InnerArea = Convert.ToDecimal(Math.PI) * InnerDiameter * InnerDiameter;

                    #endregion Calculate Inner Area

                    #region Calculate Outer Area

                    OuterArea = Convert.ToDecimal(Math.PI) * OuterDiameter * OuterDiameter;

                    #endregion Calculate Outer Area

                    #region Calculate Total Area

                    TotalArea = OuterArea - InnerArea;

                    #endregion Calculate Total Area

                    #region Calculate Volume

                    if (concretetube.HeightB != null)
                        Volume = TotalArea * Convert.ToDecimal(concretetube.HeightA + "." + concretetube.HeightB) * Convert.ToDecimal(concretetube.ColumnNo);
                    else
                        Volume = TotalArea * Convert.ToDecimal(concretetube.HeightA) * Convert.ToDecimal(concretetube.ColumnNo);

                    #endregion Calculate Volume

                    #region Convert Feet/Inch to Meter/CM

                    Decimal CubicMeterCM = CommonFunctions.ConvertMeterAndCMForVolume(Volume);
                    ViewBag.lblAnswerCubicMeterAndCMValue = CubicMeterCM.ToString("0.00") + " m<sup>3</sup>";
                    ViewBag.lblAnswerCubicFeetAndInchValue = Volume.ToString("0.00") + " ft<sup>3</sup>";

                    #endregion Convert Feet/Inch to Meter/CM

                    #region Calculaute Dry Volume

                    DryVolume = CubicMeterCM * Convert.ToDecimal(1.524);

                    #endregion Calculaute Dry Volume

                    #region Calculaute Cement

                    Cement = (DryVolume * 1) / (Convert.ToDecimal(concretetube.GradeID));
                    CementBag = Cement / Convert.ToDecimal(0.035);
                    ViewBag.lblAnswerCement = CementBag.ToString("0.00") + " Bags";

                    #endregion Calculaute Cement

                    #region Calculaute Sand And Aggregate

                    if (concretetube.GradeID == Convert.ToDecimal(5.5))
                    {
                        #region Calculaute Sand

                        Sand = (DryVolume * Convert.ToDecimal(1.5)) / (Convert.ToDecimal(concretetube.GradeID));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculaute Sand

                        #region Calculaute Aggregate

                        Aggregate = (DryVolume * 3) / (Convert.ToDecimal(concretetube.GradeID));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculaute Aggregate
                    }
                    else if (concretetube.GradeID == 7)
                    {
                        #region Calculaute Sand

                        Sand = (DryVolume * 2) / (Convert.ToDecimal(concretetube.GradeID));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculaute Sand

                        #region Calculaute Aggregate

                        Aggregate = (DryVolume * 4) / (Convert.ToDecimal(concretetube.GradeID));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculaute Aggregate
                    }
                    else if (concretetube.GradeID == 10)
                    {
                        #region Calculaute Sand

                        Sand = (DryVolume * 3) / (Convert.ToDecimal(concretetube.GradeID));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculaute Sand

                        #region Calculaute Aggregate

                        Aggregate = (DryVolume * 6) / (Convert.ToDecimal(concretetube.GradeID));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculaute Aggregate
                    }
                    else if (concretetube.GradeID == 13)
                    {
                        #region Calculaute Sand

                        Sand = (DryVolume * 4) / (Convert.ToDecimal(concretetube.GradeID));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculaute Sand

                        #region Calculaute Aggregate

                        Aggregate = (DryVolume * 8) / (Convert.ToDecimal(concretetube.GradeID));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculaute Aggregate
                    }
                    #endregion Calculaute Sand And Aggregate

                    #region Load Chart

                    //ChartShow(CementBag * 50, SandInTon * 1000, AggregateInTon * 1000);

                    #endregion Load Chart

                    #region Formula

                    #region InnerRadius Formula
                    if (concretetube.InnerDiameterB != null)
                    {
                        ViewBag.lblInnerAreaFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><msup><mi>&#x3C0;r</mi><mn>2</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>Diameter</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/> <math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>" + concretetube.InnerDiameterA + "." + concretetube.InnerDiameterB + "</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><mn>3.14</mn><mo>&#xD7;</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><mi>" + InnerArea.ToString("0.00") + "</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblInnerAreaFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><msup><mi>&#x3C0;r</mi><mn>2</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>Diameter</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mi>" + concretetube.InnerDiameterA + "</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><mn>3.14</mn><mo>&#xD7;</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + InnerDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Inner Area =</mo><mi>" + InnerArea.ToString("0.00") + "</mi></math>";
                    }

                    #endregion InnerRadius Formula

                    #region OuterRadius Formula
                    if (concretetube.OuterDiameterB != null)
                    {
                        ViewBag.lblOuterAreaFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><msup><mi>&#x3C0;r</mi><mn>2</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>Diameter</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/> <math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>" + concretetube.OuterDiameterA + "." + concretetube.OuterDiameterB + "</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><mn>3.14</mn><mo>&#xD7;</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><mi>" + OuterArea.ToString("0.00") + "</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblOuterAreaFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><msup><mi>&#x3C0;r</mi><mn>2</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mfrac><mi>Diameter</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mi>" + concretetube.OuterDiameterA + "</mi><mn>2</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>r =</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><mn>3.14</mn><mo>&#xD7;</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + OuterDiameter.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Outer Area =</mo><mi>" + OuterArea.ToString("0.00") + "</mi></math>";
                    }

                    #endregion OuterRadius Formula

                    #region Volume Formula
                    if (concretetube.HeightB != null)
                    {
                        ViewBag.lblVolumeFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Area=</mo><mi>Outer Tube Area - Inner Tube Area</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Area =</mo><mi>" + InnerArea.ToString("0.00") + "</mi><mo>-</mo><mi>" + OuterArea.ToString("0.00") + "</mi><mo>=</mo><mi>" + TotalArea.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mi>Area</mi><mo>×</mo><mi>Height</mi><mo>×</mo><mi>No of Column</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mi>" + TotalArea.ToString("0.00") + "</mi><mo>×</mo><mi>" + concretetube.HeightA + "</mi><mi>.</mi><mi>" + concretetube.HeightB + "</mi><mo>×</mo><mi>" + concretetube.ColumnNo + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mn>" + CubicMeterCM.ToString("0.00") + "</mn><msup><mi>m</mi><mn>3</mn></msup><mi> or </mi><mn>" + Volume.ToString("0.00") + "</mn><msup><mi>ft</mi><mn>3</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Dry Volume=</mo><mn>" + CubicMeterCM.ToString("0.00") + "</mn><mo>×</mo><mi>1.524 </mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Dry Volume=</mo><mn>" + DryVolume.ToString("0.00") + "</mn></math>";
                    }
                    else
                    {
                        ViewBag.lblVolumeFormula = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Area=</mo><mi>Outer Tube Area - Inner Tube Area</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Area =</mo><mi>" + InnerArea.ToString("0.00") + "</mi><mo>-</mo><mi>" + OuterArea.ToString("0.00") + "</mi><mo>=</mo><mi>" + TotalArea.ToString("0.00") + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mi>Area</mi><mo>×</mo><mi>Height</mi><mo>×</mo><mi>No of Column</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mi>" + TotalArea.ToString("0.00") + "</mi><mo>×</mo><mi>" + concretetube.HeightA + "</mi><mo>×</mo><mi>" + concretetube.ColumnNo + "</mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Volume=</mo><mn>" + CubicMeterCM.ToString("0.00") + "</mn><msup><mi>m</mi><mn>3</mn></msup><mi> or </mi><mn>" + Volume.ToString("0.00") + "</mn><msup><mi>ft</mi><mn>3</mn></msup></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Dry Volume=</mo><mn>" + CubicMeterCM.ToString("0.00") + "</mn><mo>×</mo><mi>1.524 </mi></math>"
                                                + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>Dry Volume=</mo><mn>" + DryVolume.ToString("0.00") + "</mn></math>";
                    }
                    #endregion Volume Formula

                    #region Cement Formula

                    ViewBag.lblCementFormula = "<h4>Cement Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Cement</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>1</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Cement.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>No. of Cement Bags</h4>"
                                            + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Cement Volume</mi></mrow><mrow><mn>0.035</mn></mrow></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + Cement.ToString("0.00") + "</mi></mrow><mrow><mn>0.035</mn></mrow></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Math.Ceiling(CementBag).ToString("0") + " Bags</mi></math>";

                    #endregion Formula

                    #region Sand and Aggregate Formula

                    if (concretetube.GradeID == Convert.ToDecimal(5.5))
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>1.5</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Sand.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Sand in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Sand.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + SandInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Sand Formula

                        #region Aggregate Formula

                        ViewBag.lblAggregateFormula = "<h4>Aggregate Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Aggregate</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }
                    else if (concretetube.GradeID == 7)
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>1.5</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Sand.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Sand in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Sand.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + SandInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Sand Formula

                        #region Aggregate Formula

                        ViewBag.lblAggregateFormula = "<h4>Aggregate Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Aggregate</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }
                    else if (concretetube.GradeID == 10)
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Sand.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Sand in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Sand.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + SandInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Sand Formula

                        #region Aggregate Formula

                        ViewBag.lblAggregateFormula = "<h4>Aggregate Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Aggregate</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>6</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }
                    else if (concretetube.GradeID == 13)
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>4</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Sand.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Sand in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Sand.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + SandInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Sand Formula

                        #region Aggregate Formula

                        ViewBag.lblAggregateFormula = "<h4>Aggregate Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Aggregate</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>8</mi></mrow><mi>" + concretetube.GradeID + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }

                    #endregion Sand and Aggregate Formula

                    #endregion Formula
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function: Calculate Value For Feet And Inch

        #region Insert Log Function

        public void CalculatorLogInsert(ConcreteTubeCalculator concretetube)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Concrete-Tube-Calculator";

                if (concretetube.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(concretetube.UnitID);

                if (concretetube.GradeID != -1)
                    entLOG_Calculation.ParamB = Convert.ToString(concretetube.GradeID);

                if (concretetube.ColumnNo != null)
                    entLOG_Calculation.ParamC = Convert.ToString(concretetube.ColumnNo);

                if (concretetube.InnerDiameterA != null)
                {
                    if (concretetube.InnerDiameterB != null)
                        entLOG_Calculation.ParamD = Convert.ToString(concretetube.InnerDiameterA + "." + concretetube.InnerDiameterB);
                    else
                        entLOG_Calculation.ParamD = Convert.ToString(concretetube.InnerDiameterA);
                }

                if (concretetube.OuterDiameterA != null)
                {
                    if (concretetube.OuterDiameterB != null)
                        entLOG_Calculation.ParamE = Convert.ToString(concretetube.OuterDiameterA + "." + concretetube.OuterDiameterB);
                    else
                        entLOG_Calculation.ParamE = Convert.ToString(concretetube.OuterDiameterA);
                }

                if (concretetube.HeightA != null)
                {
                    if (concretetube.HeightB != null)
                        entLOG_Calculation.ParamF = Convert.ToString(concretetube.HeightA + "." + concretetube.HeightB);
                    else
                        entLOG_Calculation.ParamF = Convert.ToString(concretetube.HeightA);
                }

                entLOG_Calculation.ParamG = ViewBag.lblAnswerCubicMeterAndCMValue;
                entLOG_Calculation.ParamH = ViewBag.lblAnswerCubicFeetAndInchValue;
                entLOG_Calculation.ParamI = ViewBag.lblAnswerCement;
                entLOG_Calculation.ParamJ = ViewBag.lblAnswerSand;
                entLOG_Calculation.ParamK = ViewBag.lblAnswerAggregate;
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
