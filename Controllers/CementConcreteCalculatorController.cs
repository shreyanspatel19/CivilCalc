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
    public class CementConcreteCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Cement-Concrete-Calculator")]
       
        [Route("Quantity-Estimator/RCC-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Cement-Concrete-Calculator");


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

            return View("CementConcreteCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(CementConcreteCalculator cementcalculator)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Cement-Concrete-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);


            //ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByURLName(vModel.CalculatorID).ToList();

            if (cementcalculator.UnitID == 1)
                CalculateCementConcreteValueForMeterAndCM(cementcalculator);
            else
                CalculateCementConcreteValueForFeetAndInch(cementcalculator);
            CalculatorLogInsert(cementcalculator);

            return PartialView("_CementConcreteCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Cement Concrete Value For Meter And CM

        protected void CalculateCementConcreteValueForMeterAndCM(CementConcreteCalculator cementcalculator)
        {
            try
            {
                if (cementcalculator.LengthA != null && cementcalculator.WidthA != null && cementcalculator.DepthA != null)
                {
                    #region Calculation

                    Decimal FinalLengthInMeter = 0;
                    Decimal FinalWidthInMeter = 0;
                    Decimal FinalDepthInMeter = 0;

                    #region Length In Meter
                    Decimal InputLengthInMeter = 0;
                    Decimal InputLengthInCM = 0;
                    if (cementcalculator.LengthA != null && cementcalculator.WidthA != null)
                        InputLengthInMeter = Convert.ToDecimal(cementcalculator.LengthA);
                    else
                        InputLengthInMeter = 0;


                    if (cementcalculator.LengthB != null && cementcalculator.WidthB != null)
                        InputLengthInCM = Convert.ToDecimal(cementcalculator.LengthB);
                    else
                        InputLengthInCM = 0;

                    FinalLengthInMeter = CommonFunctions.MeterAndCMToMeter(InputLengthInMeter, InputLengthInCM);

                    #endregion Length In Meter

                    #region Width In Meter
                    Decimal InputWidthInMeter = 0;
                    Decimal InputWidthInCM = 0;
                    if (cementcalculator.WidthA != null)
                        InputWidthInMeter = Convert.ToDecimal(cementcalculator.WidthA);
                    else
                        InputWidthInMeter = 0;


                    if (cementcalculator.WidthB != null)
                        InputWidthInCM = Convert.ToDecimal(cementcalculator.WidthB);
                    else
                        InputWidthInCM = 0;

                    FinalWidthInMeter = CommonFunctions.MeterAndCMToMeter(InputWidthInMeter, InputWidthInCM);

                    #endregion Width In Meter

                    #region Depth In Meter
                    Decimal InputDepthInMeter = 0;
                    Decimal InputDepthInCM = 0;
                    if (cementcalculator.DepthA != null)
                        InputDepthInMeter = Convert.ToDecimal(cementcalculator.DepthA);
                    else
                        InputDepthInMeter = 0;


                    if (cementcalculator.DepthB != null)
                        InputDepthInCM = Convert.ToDecimal(cementcalculator.DepthB);
                    else
                        InputDepthInCM = 0;

                    FinalDepthInMeter = CommonFunctions.MeterAndCMToMeter(InputDepthInMeter, InputDepthInCM);

                    #endregion Depth In Meter

                    Decimal CementConcrete_CubicMeterAndCMValue = CommonFunctions.Volume(FinalLengthInMeter, FinalWidthInMeter, FinalDepthInMeter);
                   ViewBag.lblAnswerCementConcrete_CubicMeterAndCMValue = CementConcrete_CubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";

                    Decimal CementConcrete_CubicFeetAndInchValue = CommonFunctions.ConvertFeetAndInchForVolume(CementConcrete_CubicMeterAndCMValue);
                   ViewBag.lblAnswerCementConcrete_CubicFeetAndInchValue = CementConcrete_CubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";

                    #endregion Calculation

                    #region Wet Volume

                    Decimal WetVolume = 0;
                    WetVolume = CementConcrete_CubicMeterAndCMValue + ((Convert.ToDecimal(CementConcrete_CubicMeterAndCMValue) * Convert.ToDecimal(52.4)) / 100);

                    #endregion Wet Volume

                    #region Calculate Cement

                    Decimal CementVolume = 0, CementBags = 0, CementKG = 0;
                    CementVolume = (1 / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                    CementBags = CementVolume / Convert.ToDecimal(0.035);
                    CementKG = CementBags * 50;

                   ViewBag.lblAnswerCementConcrete_Cement = CementBags.ToString("0.00") + " Bags";

                    #endregion Calculate Cement

                    #region Calculate Aggregates And Sand

                    Decimal SandVolume = 0, SandKG = 0, SandTon = 0;
                    Decimal AggregatesVolume = 0, AggregatesKG = 0, AggregatesTon = 0;

                    if (cementcalculator.GradeID == Convert.ToDecimal(5.5))
                    {
                        #region Calculate Sand

                        SandVolume = (Convert.ToDecimal(1.5) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        SandKG = SandVolume * 1550;
                        SandTon = SandKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Sand = SandTon.ToString("0.00") + " Ton";
                        //lblAnswerCementConcrete_Sand.Text = SandKG.ToString("0.00") + " kg";

                        #endregion Calculate Sand

                        #region Calculate Aggregates

                        AggregatesVolume = (Convert.ToDecimal(3) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        AggregatesKG = AggregatesVolume * 1350;
                        AggregatesTon = AggregatesKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Aggregate = AggregatesTon.ToString("0.00") + " Ton";
                        //lblAnswerCementConcrete_Aggregate.Text = AggregatesKG.ToString("0.00") + " kg";

                        #endregion Calculate Aggregates
                    }
                    else if (cementcalculator.GradeID == 7)
                    {
                        #region Calculate Sand

                        SandVolume = (Convert.ToDecimal(2) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        SandKG = SandVolume * 1550;
                        SandTon = SandKG / 1000;
                      ViewBag.lblAnswerCementConcrete_Sand = SandTon.ToString("0.00") + " Ton";
                        //lblAnswerCementConcrete_Sand.Text = SandKG.ToString("0.00") + " kg";

                        #endregion Calculate Sand

                        #region Calculate Aggregates

                        AggregatesVolume = (Convert.ToDecimal(4) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        AggregatesKG = AggregatesVolume * 1350;
                        AggregatesTon = AggregatesKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Aggregate = AggregatesTon.ToString("0.00") + " Ton";
                        //lblAnswerCementConcrete_Aggregate.Text = AggregatesKG.ToString("0.00") + " kg";

                        #endregion Calculate Aggregates
                    }
                    else if (cementcalculator.GradeID == 10)
                    {
                        #region Calculate Sand

                        SandVolume = (Convert.ToDecimal(3) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        SandKG = SandVolume * 1550;
                        SandTon = SandKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Sand = SandTon.ToString("0.00") + " Ton";
                        //lblAnswerCementConcrete_Sand.Text = SandKG.ToString("0.00") + " kg";

                        #endregion Calculate Sand

                        #region Calculate Aggregates

                        AggregatesVolume = (Convert.ToDecimal(6) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        AggregatesKG = AggregatesVolume * 1350;
                        AggregatesTon = AggregatesKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Aggregate = AggregatesTon.ToString("0.00") + " Ton";
                        //lblAnswerCementConcrete_Aggregate.Text = AggregatesKG.ToString("0.00") + " kg";

                        #endregion Calculate Aggregates
                    }
                    else if (cementcalculator.GradeID == 13)
                    {
                        #region Calculate Sand

                        SandVolume = (Convert.ToDecimal(4) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        SandKG = SandVolume * 1550;
                        SandTon = SandKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Sand = SandTon.ToString("0.00") + " Ton";
                        //lblAnswerCementConcrete_Sand.Text = SandKG.ToString("0.00") + " kg";

                        #endregion Calculate Sand

                        #region Calculate Aggregates

                        AggregatesVolume = (Convert.ToDecimal(8) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        AggregatesKG = AggregatesVolume * 1350;
                        AggregatesTon = AggregatesKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Aggregate = AggregatesTon.ToString("0.00") + " Ton";
                        //lblAnswerCementConcrete_Aggregate.Text = AggregatesKG.ToString("0.00") + " kg";

                        #endregion Calculate Aggregates
                    }

                    #endregion Calculate Aggregates And Sand

                    #region Formula

                    #region Cement Concrete Volume Formula

                   ViewBag.lblCementConcrete_VolumeFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Length</mi><mo>&#xD7;</mo><mi>Width</mi><mo>&#xD7;</mo><mi>Depth</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + FinalLengthInMeter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + FinalWidthInMeter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + FinalDepthInMeter.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicFeetAndInchValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>ft</mi><mn>3</mn></msup></math>";

                    //if (txtLengthB.Text.Trim() != String.Empty || txtWidthB.Text.Trim() != String.Empty || txtDepthB.Text.Trim() != String.Empty)
                    //{
                    //    lblCementConcrete_VolumeFormula.Text = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Length</mi><mo>&#xD7;</mo><mi>Width</mi><mo>&#xD7;</mo><mi>Depth</mi></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + txtLengthA.Text.Trim() + "." + txtLengthB.Text.Trim() + "</mi><mo>&#xD7;</mo><mi>" + txtWidthA.Text.Trim() + "." + txtWidthB.Text.Trim() + "</mi><mo>&#xD7;</mo><mi>" + txtDepthA.Text.Trim() + "." + txtDepthB.Text.Trim() + "</mi></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicFeetAndInchValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>ft</mi><mn>3</mn></msup></math>";
                    //}
                    //else
                    //{
                    //    lblCementConcrete_VolumeFormula.Text = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Length</mi><mo>&#xD7;</mo><mi>Width</mi><mo>&#xD7;</mo><mi>Depth</mi></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + txtLengthA.Text.Trim() + "</mi><mo>&#xD7;</mo><mi>" + txtWidthA.Text.Trim() + "</mi><mo>&#xD7;</mo><mi>" + txtDepthA.Text.Trim() + "</mi></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicFeetAndInchValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>ft</mi><mn>3</mn></msup></math>";
                    //}

                    #endregion Cement Concrete Volume Formula

                    #region Wet Volume of Mix Formula

                   ViewBag.lblCementConcrete_WetVolumeFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Total Volume</mi><mo>+</mo><mfenced><mrow><mi>Total Volume</mi><mo>&#xD7;</mo><mfrac><mrow><mn>52</mn><mo>.</mo><mn>4</mn></mrow><mn>100</mn></mfrac></mrow></mfenced></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mi><mo>+</mo><mfenced><mrow><mi>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mi><mo>&#xD7;</mo><mfrac><mrow><mn>52</mn><mo>.</mo><mn>4</mn></mrow><mn>100</mn></mfrac></mrow></mfenced></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + WetVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>";

                    #endregion Wet Volume of Mix Formula

                    #region Cement Formula

                   ViewBag.lblCementConcrete_CementFormula = "<h4>Cement Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Cement Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>1</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>No. of Cement Bags</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Cement Volume</mi></mrow><mrow><mn>0</mn><mo>.</mo><mn>035</mn></mrow></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + CementVolume.ToString("0.00") + "</mi></mrow><mrow><mn>0</mn><mo>.</mo><mn>035</mn></mrow></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementBags.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Bags</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Cement in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>No. of Cement Bags</mi><mo>&#xD7;</mo><mn>50</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + CementBags.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>50</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>";

                    #endregion Cement Formula

                    #region Sand And Aggregate Formula

                    if (cementcalculator.GradeID == Convert.ToDecimal(5.5))
                    {
                       ViewBag.lblCementConcrete_SandFormula = "<h4>Sand Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Sand Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>1.5</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SandVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Ton</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Sand in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + SandKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";

                       ViewBag.lblCementConcrete_AggregateFormula = "<h4>Aggregate Volume</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Aggregate Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>3</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Kg</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + AggregatesVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Ton</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Aggregates in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + AggregatesKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";
                    }
                    else if (cementcalculator.GradeID == 7)
                    {
                       ViewBag.lblCementConcrete_SandFormula = "<h4>Sand Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Sand Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>2</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SandVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Ton</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Sand in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + SandKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";

                       ViewBag.lblCementConcrete_AggregateFormula = "<h4>Aggregate Volume</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Aggregate Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>4</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Kg</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + AggregatesVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Ton</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Aggregates in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + AggregatesKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";
                    }
                    else if (cementcalculator.GradeID == 10)
                    {
                       ViewBag.lblCementConcrete_SandFormula = "<h4>Sand Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Sand Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>3</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SandVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Ton</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Sand in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + SandKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";

                       ViewBag.lblCementConcrete_AggregateFormula = "<h4>Aggregate Volume</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Aggregate Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>6</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Kg</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + AggregatesVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesKG.ToString("0.000") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Ton</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Aggregates in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + AggregatesKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";
                    }
                    else if (cementcalculator.GradeID == 13)
                    {
                       ViewBag.lblCementConcrete_SandFormula = "<h4>Sand Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Sand Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>4</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SandVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Ton</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Sand in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + SandKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";

                       ViewBag.lblCementConcrete_AggregateFormula = "<h4>Aggregate Volume</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Aggregate Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>8</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Kg</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + AggregatesVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Ton</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Aggregates in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + AggregatesKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";
                    }

                    #endregion Sand And Aggregate Formula

                    #endregion Formula

                    #region ChartValue
                    //ChartShow(CementBags * 50, SandTon * 1000, AggregatesTon * 1000);
                    #region Load Chart
                    ViewBag.ChartCement = Math.Round(CementBags * 50);
                    ViewBag.ChartSand = Math.Round(SandTon * 1000);
                    ViewBag.ChartBrick = Math.Round(AggregatesTon * 1000);
                    #endregion Load Chart
                    #endregion ChartValue
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Cement Concrete Value For Meter And CM

        #region Function Calculate Cement Concrete Value For Feet And Inch

        protected void CalculateCementConcreteValueForFeetAndInch(CementConcreteCalculator cementcalculator)
        {
            try
            {
                if (cementcalculator.LengthA != null && cementcalculator.WidthA != null && cementcalculator.DepthA != null)
                {
                    #region Calculation

                    Decimal FinalLengthInFeet = 0;
                    Decimal FinalWidthInFeet = 0;
                    Decimal FinalDepthInFeet = 0;

                    #region Length In Feet
                    Decimal InputLengthInFeet = 0;
                    Decimal InputLengthInInch = 0;
                    if (cementcalculator.LengthA != null)
                        InputLengthInFeet = Convert.ToDecimal(cementcalculator.LengthA);
                    else
                        InputLengthInFeet = 0;


                    if (cementcalculator.LengthB != null)
                        InputLengthInInch = Convert.ToDecimal(cementcalculator.LengthB);
                    else
                        InputLengthInInch = 0;

                    FinalLengthInFeet = CommonFunctions.FeetAndInchToFeet(InputLengthInFeet, InputLengthInInch);

                    #endregion Length In Feet

                    #region Width In Feet
                    Decimal InputWidthInFeet = 0;
                    Decimal InputWidthInInch = 0;
                    if (cementcalculator.WidthA != null)
                        InputWidthInFeet = Convert.ToDecimal(cementcalculator.WidthA);
                    else
                        InputWidthInFeet = 0;


                    if (cementcalculator.WidthB != null)
                        InputWidthInInch = Convert.ToDecimal(cementcalculator.WidthB);
                    else
                        InputWidthInInch = 0;

                    FinalWidthInFeet = CommonFunctions.FeetAndInchToFeet(InputWidthInFeet, InputWidthInInch);

                    #endregion Width In Feet

                    #region Depth In Feet
                    Decimal InputDepthInFeet = 0;
                    Decimal InputDepthInInch = 0;
                    if (cementcalculator.DepthA != null)
                        InputDepthInFeet = Convert.ToDecimal(cementcalculator.DepthA);
                    else
                        InputDepthInFeet = 0;


                    if (cementcalculator.DepthB != null)
                        InputDepthInInch = Convert.ToDecimal(cementcalculator.DepthB);
                    else
                        InputDepthInInch = 0;

                    FinalDepthInFeet = CommonFunctions.FeetAndInchToFeet(InputDepthInFeet, InputDepthInInch);

                    #endregion Depth In Feet

                    // Decimal CementConcrete_CubicFeetAndInchValue = CommonFunctions.Volume(Convert.ToDecimal(txtLengthA.Text.Trim() + "." + txtLengthB.Text.Trim()), Convert.ToDecimal(txtWidthA.Text.Trim() + "." + txtWidthB.Text.Trim()), Convert.ToDecimal(txtDepthA.Text.Trim() + "." + txtDepthB.Text.Trim()));
                    Decimal CementConcrete_CubicFeetAndInchValue = CommonFunctions.Volume(FinalLengthInFeet, FinalWidthInFeet, FinalDepthInFeet);
                   ViewBag.lblAnswerCementConcrete_CubicFeetAndInchValue = CementConcrete_CubicFeetAndInchValue.ToString("0.00") + " ft<sup>3</sup>";

                    Decimal CementConcrete_CubicMeterAndCMValue = CommonFunctions.ConvertMeterAndCMForVolume(CementConcrete_CubicFeetAndInchValue);
                   ViewBag.lblAnswerCementConcrete_CubicMeterAndCMValue = CementConcrete_CubicMeterAndCMValue.ToString("0.00") + " m<sup>3</sup>";

                    #endregion Calculation

                    #region Wet Volume

                    Decimal WetVolume = 0;
                    WetVolume = CementConcrete_CubicMeterAndCMValue + ((Convert.ToDecimal(CementConcrete_CubicMeterAndCMValue) * Convert.ToDecimal(52.4)) / 100);

                    #endregion Wet Volume

                    #region Calculate Cement

                    Decimal CementVolume = 0, CementBags = 0, CementKG = 0;
                    CementVolume = (1 / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                    CementBags = CementVolume / Convert.ToDecimal(0.035);
                    CementKG = CementBags * 50;

                  ViewBag.lblAnswerCementConcrete_Cement = CementBags.ToString("0.00") + " Bags";
                    #endregion Calculate Cement

                    #region Calculate Aggregates And Sand

                    Decimal SandVolume = 0, SandKG = 0, SandTon = 0;
                    Decimal AggregatesVolume = 0, AggregatesKG = 0, AggregatesTon = 0;

                    if (cementcalculator.GradeID == Convert.ToDecimal(5.5))
                    {
                        #region Calculate Sand

                        SandVolume = (Convert.ToDecimal(1.5) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        SandKG = SandVolume * 1550;
                        SandTon = SandKG / 1000;
                        ViewBag.lblAnswerCementConcrete_Sand = SandTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregates

                        AggregatesVolume = (Convert.ToDecimal(3) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        AggregatesKG = AggregatesVolume * 1350;
                        AggregatesTon = AggregatesKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Aggregate = AggregatesTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregates
                    }
                    else if (cementcalculator.GradeID == 7)
                    {
                        #region Calculate Sand

                        SandVolume = (Convert.ToDecimal(2) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        SandKG = SandVolume * 1550;
                        SandTon = SandKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Sand = SandTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregates

                        AggregatesVolume = (Convert.ToDecimal(4) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        AggregatesKG = AggregatesVolume * 1350;
                        AggregatesTon = AggregatesKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Aggregate = AggregatesTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregates
                    }
                    else if (cementcalculator.GradeID == 10)
                    {
                        #region Calculate Sand

                        SandVolume = (Convert.ToDecimal(3) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        SandKG = SandVolume * 1550;
                        SandTon = SandKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Sand = SandTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregates

                        AggregatesVolume = (Convert.ToDecimal(6) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        AggregatesKG = AggregatesVolume * 1350;
                        AggregatesTon = AggregatesKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Aggregate = AggregatesTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregates
                    }
                    else if (cementcalculator.GradeID == 13)
                    {
                        #region Calculate Sand

                        SandVolume = (Convert.ToDecimal(4) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        SandKG = SandVolume * 1550;
                        SandTon = SandKG / 1000;
                      ViewBag.lblAnswerCementConcrete_Sand = SandTon.ToString("0.00") + " Ton";

                        #endregion Calculate Sand

                        #region Calculate Aggregates

                        AggregatesVolume = (Convert.ToDecimal(8) / Convert.ToDecimal(cementcalculator.GradeID)) * WetVolume;
                        AggregatesKG = AggregatesVolume * 1350;
                        AggregatesTon = AggregatesKG / 1000;
                       ViewBag.lblAnswerCementConcrete_Aggregate = AggregatesTon.ToString("0.00") + " Ton";

                        #endregion Calculate Aggregates
                    }

                    #endregion Calculate Aggregates And Sand

                    #region Formula

                    #region Cement Concrete Volume Formula


                   ViewBag.lblCementConcrete_VolumeFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Length</mi><mo>&#xD7;</mo><mi>Width</mi><mo>&#xD7;</mo><mi>Depth</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + FinalLengthInFeet.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + FinalWidthInFeet.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + FinalDepthInFeet.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicFeetAndInchValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>ft</mi><mn>3</mn></msup></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>";

                    //if (txtLengthB.Text.Trim() != String.Empty || txtWidthB.Text.Trim() != String.Empty || txtDepthB.Text.Trim() != String.Empty)
                    //{
                    //    lblCementConcrete_VolumeFormula.Text = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Length</mi><mo>&#xD7;</mo><mi>Width</mi><mo>&#xD7;</mo><mi>Depth</mi></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + txtLengthA.Text.Trim() + "." + txtLengthB.Text.Trim() + "</mi><mo>&#xD7;</mo><mi>" + txtWidthA.Text.Trim() + "." + txtWidthB.Text.Trim() + "</mi><mo>&#xD7;</mo><mi>" + txtDepthA.Text.Trim() + "." + txtDepthB.Text.Trim() + "</mi></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicFeetAndInchValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>ft</mi><mn>3</mn></msup></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>";
                    //}
                    //else
                    //{
                    //    lblCementConcrete_VolumeFormula.Text = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Length</mi><mo>&#xD7;</mo><mi>Width</mi><mo>&#xD7;</mo><mi>Depth</mi></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + txtLengthA.Text.Trim() + "</mi><mo>&#xD7;</mo><mi>" + txtWidthA.Text.Trim() + "</mi><mo>&#xD7;</mo><mi>" + txtDepthA.Text.Trim() + "</mi></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicFeetAndInchValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>ft</mi><mn>3</mn></msup></math>"
                    //                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>";
                    //}

                    #endregion Cement Concrete Volume Formula

                    #region Wet Volume of Mix Formula

                   ViewBag.lblCementConcrete_WetVolumeFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Total Volume</mi><mo>+</mo><mfenced><mrow><mi>Total Volume</mi><mo>&#xD7;</mo><mfrac><mrow><mn>52</mn><mo>.</mo><mn>4</mn></mrow><mn>100</mn></mfrac></mrow></mfenced></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mi><mo>+</mo><mfenced><mrow><mi>" + CementConcrete_CubicMeterAndCMValue.ToString("0.00") + "</mi><mo>&#xD7;</mo><mfrac><mrow><mn>52</mn><mo>.</mo><mn>4</mn></mrow><mn>100</mn></mfrac></mrow></mfenced></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + WetVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>";

                    #endregion Wet Volume of Mix Formula

                    #region Cement Formula

                   ViewBag.lblCementConcrete_CementFormula = "<h4>Cement Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Cement Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>1</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>No. of Cement Bags</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Cement Volume</mi></mrow><mrow><mn>0</mn><mo>.</mo><mn>035</mn></mrow></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + CementVolume.ToString("0.00") + "</mi></mrow><mrow><mn>0</mn><mo>.</mo><mn>035</mn></mrow></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementBags.ToString("0.000") + "</mn><mo>&#xA0;</mo><mi>Bags</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Cement in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>No. of Cement Bags</mi><mo>&#xD7;</mo><mn>50</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + CementBags.ToString("0.000") + "</mi><mo>&#xD7;</mo><mn>50</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + CementKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>";

                    #endregion Cement Formula

                    #region Sand And Aggregate Formula

                    if (cementcalculator.GradeID == Convert.ToDecimal(5.5))
                    {
                       ViewBag.lblCementConcrete_SandFormula = "<h4>Sand Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Sand Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>1.5</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SandVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Ton</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Sand in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + SandKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";

                       ViewBag.lblCementConcrete_AggregateFormula = "<h4>Aggregate Volume</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Aggregate Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>3</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Kg</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + AggregatesVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Ton</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Aggregates in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + AggregatesKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";
                    }
                    else if (cementcalculator.GradeID == 7)
                    {
                          ViewBag.lblCementConcrete_SandFormula = "<h4>Sand Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Sand Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>2</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SandVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Ton</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Sand in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + SandKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";

                       ViewBag.lblCementConcrete_AggregateFormula = "<h4>Aggregate Volume</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Aggregate Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>4</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Kg</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + AggregatesVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Ton</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Aggregates in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + AggregatesKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";
                    }
                    else if (cementcalculator.GradeID == 10)
                    {
                       ViewBag.lblCementConcrete_SandFormula = "<h4>Sand Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Sand Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>3</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SandVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Ton</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Sand in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + SandKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";

                       ViewBag.lblCementConcrete_AggregateFormula = "<h4>Aggregate Volume</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Aggregate Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>6</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Kg</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + AggregatesVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Ton</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Aggregates in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + AggregatesKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";
                    }
                    else if (cementcalculator.GradeID == 13)
                    {
                       ViewBag.lblCementConcrete_SandFormula = "<h4>Sand Volume</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Sand Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>4</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Kg</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Sand Volume</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SandVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                           + "<hr/>"
                                                           + "<h4>Sand in Ton</h4>"
                                                           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Sand in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + SandKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                           + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + SandTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";

                       ViewBag.lblCementConcrete_AggregateFormula = "<h4>Aggregate Volume</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>Aggregate Ratio</mn><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Wet Volume of Mix</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mn>8</mn><mrow><mi>" + cementcalculator.GradeID + "</mi></mrow></mfrac><mo>&#xD7;</mo><mi>" + WetVolume.ToString("0.00") + "</mi></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesVolume.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Kg</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Aggregate Volume</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + AggregatesVolume.ToString("0.00") + "</mi><mo>&#xD7;</mo><mn>1350</mn></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesKG.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>kg</mi></math>"
                                                               + "<hr/>"
                                                               + "<h4>Aggregate in Ton</h4>"
                                                               + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>Aggregates in Kg.</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mrow><mi>" + AggregatesKG.ToString("0.00") + "</mi></mrow><mn>1000</mn></mfrac></math>"
                                                               + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AggregatesTon.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi>Ton</mi></math>";
                    }

                    #endregion Sand And Aggregate Formula

                    #endregion Formula

                    #region ChartValue
                    // ChartShow(CementBags * 50, SandTon * 1000, AggregatesTon * 1000);
                    #region Load Chart
                    ViewBag.ChartCement = Math.Round(CementBags * 50);
                    ViewBag.ChartSand = Math.Round(SandTon*1000);
                    ViewBag.ChartBrick = Math.Round(AggregatesTon*1000);
                    #endregion Load Chart
                    #endregion ChartValue
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Cement Concrete Value For Feet And Inch

        #region Insert Log Function
        public void CalculatorLogInsert(CementConcreteCalculator cementcalculator)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Cement Concrete Calculator";

                if (cementcalculator.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(cementcalculator.UnitID);

                if (cementcalculator.LengthA != null)
                {
                    if (cementcalculator.LengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(cementcalculator.LengthA + "." + cementcalculator.LengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(cementcalculator.LengthA);
                }

                if (cementcalculator.WidthA != null)
                {
                    if (cementcalculator.WidthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(cementcalculator.WidthA + "." + cementcalculator.WidthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(cementcalculator.WidthA);
                }


                if (cementcalculator.DepthA != null)
                {
                    if (cementcalculator.DepthB != null)
                        entLOG_Calculation.ParamD = Convert.ToString(cementcalculator.DepthA + "." + cementcalculator.DepthB);
                    else
                        entLOG_Calculation.ParamD = Convert.ToString(cementcalculator.DepthA);
                }

                if (cementcalculator.UnitID == 1)
                {
                    if (ViewBag.lblAnswerCementConcrete_CubicMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerCementConcrete_CubicMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerCementConcrete_CubicFeetAndInchValue;
                    }
                }
                else if (cementcalculator.UnitID == 2)
                {
                    if (ViewBag.lblAnswerCementConcrete_CubicFeetAndInchValue != null)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerCementConcrete_CubicFeetAndInchValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerCementConcrete_CubicMeterAndCMValue;
                    }
                }
                else
                {
                    if (ViewBag.lblAnswerCementConcrete_CubicMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamE = ViewBag.lblAnswerCementConcrete_CubicMeterAndCMValue;
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerCementConcrete_CubicFeetAndInchValue;
                    }
                }

                entLOG_Calculation.ParamG = ViewBag.lblAnswerCementConcrete_Cement;
                entLOG_Calculation.ParamH = ViewBag.lblAnswerCementConcrete_Sand + "Sand";
                entLOG_Calculation.ParamI = ViewBag.lblAnswerCementConcrete_Aggregate + "Aggregate";

                if (cementcalculator.GradeID == Convert.ToDecimal(5.5))
                    entLOG_Calculation.ParamJ = Convert.ToString(cementcalculator.GradeID);
                else if (cementcalculator.GradeID == 7)
                    entLOG_Calculation.ParamJ = Convert.ToString(cementcalculator.GradeID);
                else if (cementcalculator.GradeID == 10)
                    entLOG_Calculation.ParamJ = Convert.ToString(cementcalculator.GradeID);
                else if (cementcalculator.GradeID == 13)
                    entLOG_Calculation.ParamJ = Convert.ToString(cementcalculator.GradeID);

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
