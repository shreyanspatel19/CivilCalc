using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.Areas.LOG_Calculation.Models;
using CivilEngineeringCalculators;
using CivilCalc.DAL;
using CivilCalc.DAL.LOG.LOG_Calculation;
using CivilCalc.Models;
using Microsoft.AspNetCore.Mvc;
using SelectForSearch_Result = CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result;

namespace CivilCalc.Controllers
{
    public class StairCaseCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Stair-Case-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Stair-Case-Calculator");


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

            return View("StairCaseCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(StairCaseCalculator staircase)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Stair-Case-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            CalculateValueForStairCase(staircase);
            CalculatorLogInsert(staircase);

            return PartialView("_StairCaseCalculatorResult", vModel);
        }
        #endregion

        #region Function: Calculate Value For Stair Case

        protected void CalculateValueForStairCase(StairCaseCalculator staircase)
        {
            #region Variables

            Decimal DryVolume = 0m, Cement = 0m, CementBag = 0m, Sand = 0m, SandInTon = 0m, Aggregate = 0m, AggregateInTon = 0m;
            Decimal Riser = 0, Tread = 0, WidthofStair = 0, HeightOfStair = 0, ThicknessOfWasitSalb = 0;
            Decimal WaistSlab = 0, NoofRiser = 0, Volumeof1Step = 0, TotalVolumeofStep = 0;
            Decimal LengthOfWaistSlab = 0, HeightofWaistSlab = 0, SlantHeightofWaistSlab = 0, VolumeoOfWaistSlab = 0;
            Decimal TotalVolumeofStaircaseInfeet = 0, TotalVolumeofStaircaseInmeter = 0;

            #endregion Variables

            try
            {
                if (staircase.RiserA != null && staircase.TreadA != null && staircase.WidthOfStair != null && staircase.HeightOfStair != null && staircase.WaistSlab != null)
                {
                    #region Load TextBox Value

                    Riser = Convert.ToDecimal(staircase.RiserA);
                    if (staircase.RiserB != null)
                        Riser += Convert.ToDecimal(staircase.RiserB) / 12;
                    Tread = Convert.ToDecimal(staircase.TreadA);
                    if (staircase.TreadB != null)
                        Tread += Convert.ToDecimal(staircase.TreadB) / 12;
                    WidthofStair = Convert.ToDecimal(staircase.WidthOfStair);
                    HeightOfStair = Convert.ToDecimal(staircase.HeightOfStair);
                    ThicknessOfWasitSalb = Convert.ToDecimal(staircase.WaistSlab) / 12;

                    #endregion Load TextBox Value

                    #region Waist Slab

                    WaistSlab = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((Riser * Riser) + (Tread * Tread))));

                    #endregion Waist Slab

                    #region No of Riser

                    NoofRiser = HeightOfStair / Riser;

                    #endregion WaistSlab

                    #region Volume of 1 Step

                    Volumeof1Step = 0.5m * Riser * Tread * WidthofStair;

                    #endregion Volume of 1 Step

                    #region Total Volume of Step

                    TotalVolumeofStep = Volumeof1Step * NoofRiser;

                    #endregion Total Volume of Step

                    #region Length of Wasit Slab

                    LengthOfWaistSlab = NoofRiser / Tread;

                    #endregion Length of Wasit Slab

                    #region Height of Waist Slab

                    HeightofWaistSlab = HeightOfStair;

                    #endregion Height of Waist Slab

                    #region Slant Height of  Waist Slab

                    SlantHeightofWaistSlab = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((LengthOfWaistSlab * LengthOfWaistSlab) + (HeightofWaistSlab * HeightofWaistSlab))));

                    #endregion Slant Height of  Waist Slab

                    #region Volume of Waist Slab

                    VolumeoOfWaistSlab = WidthofStair * ThicknessOfWasitSalb * SlantHeightofWaistSlab;

                    #endregion Volume of Waist Slab

                    #region Total Volume of Stair Case

                    TotalVolumeofStaircaseInfeet = TotalVolumeofStep + VolumeoOfWaistSlab;
                    TotalVolumeofStaircaseInmeter = CommonFunctions.ConvertMeterAndCMForVolume(TotalVolumeofStaircaseInfeet);
                    ViewBag.lblAnswerStairCase_CubicFeetAndInchValue = TotalVolumeofStaircaseInfeet.ToString("0.00") + "ft<sup>3</sup>";
                    ViewBag.lblAnswerStairCase_CubicMeterAndCMValue = TotalVolumeofStaircaseInmeter.ToString("0.00") + "m<sup>3</sup>";

                    #endregion Total Volume of Stair Case

                    #region Dry volume of stair case

                    DryVolume = TotalVolumeofStaircaseInmeter * 1.524m;

                    #endregion Dry volume of stair case

                    #region Calculate Cement

                    Cement = (DryVolume * 1) / (Convert.ToDecimal(staircase.GradeofConcrete));
                    CementBag = Cement / Convert.ToDecimal(0.035);
                    ViewBag.lblAnswerCement = CementBag.ToString("0") + " Bags";

                    #endregion Calculate Cement

                    #region Calculate Sand And Aggregate

                    if (staircase.GradeofConcrete == Convert.ToDecimal(5.5))
                    {
                        #region Calculate Sand

                        Sand = (DryVolume * Convert.ToDecimal(1.5)) / (Convert.ToDecimal(staircase.GradeofConcrete));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregate

                        Aggregate = (DryVolume * 3) / (Convert.ToDecimal(staircase.GradeofConcrete));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregate
                    }
                    else if (staircase.GradeofConcrete == 7)
                    {
                        #region Calculate Sand

                        Sand = (DryVolume * 2) / (Convert.ToDecimal(staircase.GradeofConcrete));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregate

                        Aggregate = (DryVolume * 4) / (Convert.ToDecimal(staircase.GradeofConcrete));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregate
                    }
                    else if (staircase.GradeofConcrete == 10)
                    {
                        #region Calculate Sand

                        Sand = (DryVolume * 3) / (Convert.ToDecimal(staircase.GradeofConcrete));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregate

                        Aggregate = (DryVolume * 6) / (Convert.ToDecimal(staircase.GradeofConcrete));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregate
                    }
                    else if (staircase.GradeofConcrete == 13)
                    {
                        #region Calculate Sand

                        Sand = (DryVolume * 4) / (Convert.ToDecimal(staircase.GradeofConcrete));
                        SandInTon = (Sand * 1550) / 1000;
                        ViewBag.lblAnswerSand = SandInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregate

                        Aggregate = (DryVolume * 8) / (Convert.ToDecimal(staircase.GradeofConcrete));
                        AggregateInTon = (Aggregate * 1350) / 1000;
                        ViewBag.lblAnswerAggregate = AggregateInTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregate
                    }

                    #endregion Calculate Sand And Aggregate

                    #region Load Chart

                    //  ChartShow(CementBag * 50, SandInTon * 1000, AggregateInTon * 1000);
                    ViewBag.ChartCementBag = CementBag * 50;
                    ViewBag.ChartSandInTon = SandInTon * 1000;
                    ViewBag.ChartAggregateInTon = AggregateInTon * 1000;


                    #endregion Load Chart

                    #region Formula

                    #region Volume of Step

                    ViewBag.lblVolumeofStep = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mi mathsize='1.5em'>Volume of Total Step:</mi></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Waist Slab(ft)</mi><mo>=</mo><msqrt><msup><mfenced><mrow><mi>Riser</mi></mrow></mfenced><mn>2</mn></msup><mo>+</mo><msup><mrow><mo>(</mo><mi>Tread</mi><mo>)</mo></mrow><mn>2</mn></msup></msqrt></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Waist Slab(ft)</mi><mo>=</mo><msqrt><msup><mfenced><mrow><mi>" + Riser.ToString("0.00") + "</mi></mrow></mfenced><mn>2</mn></msup><mo>+</mo><msup><mrow><mo>(</mo><mi>" + Tread.ToString("0.00") + "</mi><mo>)</mo></mrow><mn>2</mn></msup></msqrt></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Waist Slab</mi><mo>=</mo><mi>" + WaistSlab.ToString("0.00") + " ft</mi></math>"

                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Number of Riser</mi><mo>=</mo><mi>Height of Stair</mi><mo>/</mo><mi>Riser</mi></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Number of Riser</mi><mo>=</mo><mi>" + HeightOfStair.ToString("0.00") + "</mi><mo>/</mo><mi>" + Riser.ToString("0.00") + "</mi></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Number of Riser</mi><mo>=</mo><mi>" + NoofRiser.ToString("0") + " Nos</mi></math>"

                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Volume of 1 Step</mi><mo>=</mo><mi>0.5</mi><mo>×</mo><mi>Riser</mi><mo>×</mo><mi>Tread</mi><mo>×</mo><mi>Width of Stair</mi></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Volume of 1 Step</mi><mo>=</mo><mi>0.5</mi><mo>×</mo><mi>" + Riser.ToString("0.00") + "</mi><mo>×</mo><mi>" + Tread.ToString("0.00") + "</mi><mo>×</mo><mi>" + WidthofStair.ToString("0.00") + "</mi></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Volume of 1 Step</mi><mo>=</mo><mi>" + Volumeof1Step.ToString("0.00") + "</mi><msup><mi>ft</mi><mn>3</mn></msup></math>"

                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Volume of Total Step</mi><mo>=</mo><mi>Volume of 1 Step</mi><mo>×</mo><mi>No of Riser</mi></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Volume of Total Step</mi><mo>=</mo><mi>" + Volumeof1Step.ToString("0.00") + "</mi><mo>×</mo><mi>" + NoofRiser.ToString("0") + "</mi></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Volume of Total Step</mi><mo>=</mo><mi>" + TotalVolumeofStep.ToString("0.00") + "</mi><msup><mi>ft</mi><mn>3</mn></msup></math>";

                    #endregion Volume of Step

                    #region Volume of Waist Slab

                    ViewBag.lblVolumeofWaistSlab = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mi mathsize='1.5em'>Volume of Waist Slab:</mi></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Length of Waist Slab</mi><mo>=</mo><mi>Number of Riser</mi><mo>/</mo><mi>Tread</mi></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Length of Waist Slab</mi><mo>=</mo><mi>" + NoofRiser.ToString("0.00") + "</mi><mo>/</mo><mi>" + Tread.ToString("0.00") + "</mi></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Length of Waist Slab</mi><mo>=</mo><mi>" + LengthOfWaistSlab.ToString("0.00") + " ft</mi></math>"

                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Height of Waist Slab</mi><mo>=</mo><mi>" + HeightOfStair.ToString("0.00") + " ft</mi></math>"

                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Slant Height of Waist Slab</mi><mo>=</mo></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><msqrt><msup><mfenced><mrow><mi>Length of Waist Slab</mi></mrow></mfenced><mn>2</mn></msup><mo>+</mo><msup><mrow><mo>(</mo><mi>Height of Waist Slab</mi><mo>)</mo></mrow><mn>2</mn></msup></msqrt></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Slant Height of Waist Slab</mi><mo>=</mo><msqrt><msup><mfenced><mrow><mi>" + LengthOfWaistSlab.ToString("0.00") + "</mi></mrow></mfenced><mn>2</mn></msup><mo>+</mo><msup><mrow><mo>(</mo><mi>" + HeightofWaistSlab.ToString("0.00") + "</mi><mo>)</mo></mrow><mn>2</mn></msup></msqrt></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Slant Height of Waist Slab</mi><mo>=</mo><mi>" + SlantHeightofWaistSlab.ToString("0.00") + " ft</mi></math>"

                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Volume of Waist Slab</mi><mo>=</mo></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Width of Stair</mi><mo>×</mo><mi>Thickness of WaistSlab</mi><mo>×</mo><mi>Slant Height</mi></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Volume of Waist Slab</mi><mo>=</mo><mi>" + WidthofStair.ToString("0.00") + "</mi><mo>×</mo><mi>" + ThicknessOfWasitSalb.ToString("0.00") + "</mi><mo>×</mo><mi>" + SlantHeightofWaistSlab.ToString("0.00") + "</mi></math>"
                                           + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Volume of Waist Slab</mi><mo>=</mo><mi>" + VolumeoOfWaistSlab.ToString("0.00") + "</mi><msup><mi>ft</mi><mn>3</mn></msup></math>";

                    #endregion Volume of Waist Slab

                    #region Volume of Stair

                    ViewBag.lblVolumeofStair = @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mi mathsize='1.5em'>Total Volume of Stair:</mi></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Total Volume of Stair</mi><mo>=</mo><mi>Volume of Total Step</mi><mo>+</mo><mi>Volume of Waist Slab</mi></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Total Volume of Stair</mi><mo>=</mo><mi>" + TotalVolumeofStep.ToString("0.00") + "</mi><mo>+</mo><mi>" + VolumeoOfWaistSlab.ToString("0.00") + "</mi></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Total Volume of Stair</mi><mo>=</mo><mi>" + TotalVolumeofStaircaseInfeet.ToString("0.00") + "</mi><msup><mi>ft</mi><mn>3</mn></msup></math>"
                                              + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Total Volume of Stair</mi><mo>=</mo><mi>" + TotalVolumeofStaircaseInmeter.ToString("0.00") + "</mi><msup><mi>m</mi><mn>3</mn></msup></math>";

                    #endregion Volume of Stair

                    #region Dry Volume of Stair

                    ViewBag.lblDryVolumeofStair = @"</br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi mathsize='1.5em'>Dry Volume of Stair:</mi></math>"
                                             + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Dry Volume of Stair</mi><mo>=</mo><mi>Volume of Stair</mi><mo>×</mo><mi>1.524</mi></math>"
                                             + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Dry Volume of Stair</mi><mo>=</mo><mi>" + TotalVolumeofStaircaseInmeter.ToString("0.00") + "</mi><mo>×</mo><mi>1.524</mi></math>"
                                             + @"</br></br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Dry Volume of Stair</mi><mo>=</mo><mi>" + DryVolume.ToString("0.00") + "</mi><msup><mi>m</mi><mn>3</mn></msup></math>"
                                             + @"</br></br><math xmlns='http://www.w3.org/1998/Math/MathML'><mi>Dry Volume of Stair</mi><mo>=</mo><mi>" + (DryVolume / 35.3147m).ToString("0.00") + "</mi><msup><mi>ft</mi><mn>3</mn></msup></math>";


                    #endregion Dry Volume of Stair

                    #region Cement Formula

                    ViewBag.lblCementFormula = "<h4>Cement Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Cement</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>1</mi></mrow><mi>" + staircase.GradeofConcrete + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Cement.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>No. of Cement Bags</h4>"
                                            + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Cement Volume</mi></mrow><mrow><mn>0.035</mn></mrow></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + Cement.ToString("0.00") + "</mi></mrow><mrow><mn>0.035</mn></mrow></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + CementBag.ToString("0") + " Bags</mi></math>";

                    #endregion Cement Formula

                    #region Sand and Aggregate Formula

                    if (staircase.GradeofConcrete == Convert.ToDecimal(5.5))
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>1.5</mi></mrow><mi>" + staircase.GradeofConcrete + "</mi></mfrac></math>"
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
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mrow><mi>" + staircase.GradeofConcrete + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }
                    else if (staircase.GradeofConcrete == 7)
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>1.5</mi></mrow><mi>" + staircase.GradeofConcrete + "</mi></mfrac></math>"
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
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mrow><mi>" + staircase.GradeofConcrete + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }
                    else if (staircase.GradeofConcrete == 10)
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mrow><mi>" + staircase.GradeofConcrete + "</mi></mfrac></math>"
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
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>6</mi></mrow><mi>" + staircase.GradeofConcrete + "</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + Aggregate.ToString("0.00") + "</mi></math>"
                                            + "<hr/>"
                                            + "<h4>Aggregate in Ton</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + Aggregate.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></mrow><mn>1000</mn></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mi>" + AggregateInTon.ToString("0.00") + " Tons</mi></math>";

                        #endregion Aggregate Formula
                    }
                    else if (staircase.GradeofConcrete == 13)
                    {
                        #region Sand Formula

                        ViewBag.lblSandFormula = "<h4>Sand Volume</h4>"
                                            + @"<math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>Dry Volume</mi><mo>&#xD7;</mo><mi>Sand</mi></mrow><mi>Sum of Ratio</mi></mfrac></math>"
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>4</mi></mrow><mi>" + staircase.GradeofConcrete + "</mi></mfrac></math>"
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
                                            + @"<br/><br/><math xmlns='http://www.w3.org/1998/Math/MathML'><mo>=</mo><mfrac><mrow><mi>" + DryVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>8</mi></mrow><mi>" + staircase.GradeofConcrete + "</mi></mfrac></math>"
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

        #endregion Function: Calculate Value For Stair Case

        #region Insert Log Function

        public void CalculatorLogInsert(StairCaseCalculator staircase)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Stair Case Calculator";

                if (staircase.RiserA != null)
                {
                    if (staircase.RiserB != null)
                        entLOG_Calculation.ParamA = Convert.ToString(staircase.RiserA + "." + staircase.RiserB);
                    else
                        entLOG_Calculation.ParamA = Convert.ToString(staircase.RiserA);
                }
                if (staircase.TreadA != null)
                {
                    if (staircase.TreadB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(staircase.TreadA + "." + staircase.TreadB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(staircase.TreadA);
                }

                if (staircase.WidthOfStair != null)
                    entLOG_Calculation.ParamC = Convert.ToString(staircase.WidthOfStair);

                if (staircase.HeightOfStair != null)
                    entLOG_Calculation.ParamD = Convert.ToString(staircase.HeightOfStair);

                if (staircase.WaistSlab != null)
                    entLOG_Calculation.ParamE = Convert.ToString(staircase.WaistSlab);

                entLOG_Calculation.ParamF = ViewBag.lblAnswerStairCase_CubicMeterAndCMValue;
                entLOG_Calculation.ParamG = ViewBag.lblAnswerStairCase_CubicFeetAndInchValue;
                entLOG_Calculation.ParamH = ViewBag.lblAnswerCement;
                entLOG_Calculation.ParamI = ViewBag.lblAnswerSand;
                entLOG_Calculation.ParamJ = ViewBag.lblAnswerAggregate;
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
