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
    public class FlooringCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Flooring-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Flooring-Calculator");


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

            return View("FlooringCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(FlooringCalculator flooring)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Flooring-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (flooring.UnitID == 1)
                CalculateFlooringValueForMeterAndCM(flooring);
            else
              CalculateFlooringValueForFeetAndInch(flooring);

           CalculatorLogInsert(flooring);

            return PartialView("_FlooringCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Flooring Value For Meter And CM

        protected void CalculateFlooringValueForMeterAndCM(FlooringCalculator flooring)
        {
            try
            {
                if (flooring.LengthA != null && flooring.WidthA != null)
                {
                    #region Calculation

                    Decimal FlooringSquareMeterAndCMValue = CommonFunctions.Area(Convert.ToDecimal(flooring.LengthA + "." + flooring.LengthB), Convert.ToDecimal(flooring.WidthA + "." + flooring.WidthB));
                   ViewBag.lblAnswerFlooringSquareMeterAndCMValue = FlooringSquareMeterAndCMValue.ToString("0.00") + " m<sup>2</sup>";

                    Decimal FlooringSquareFeetAndInchValue = CommonFunctions.ConvertFeetAndInchForArea(FlooringSquareMeterAndCMValue);
                   ViewBag.lblAnswerFlooringSquareFeetAndInchValue = FlooringSquareFeetAndInchValue.ToString("0.00") + " ft<sup>2</sup>";

                    #endregion Calculation

                    #region Calculate tiles

                    Decimal TileDimensionAreaMeter = (Convert.ToDecimal(flooring.TileLength) * Convert.ToDecimal(flooring.TileWidth));
                    Decimal TileDimensionAreaFeet = TileDimensionAreaMeter / 10.7639m;
                    Decimal Nooftiles = FlooringSquareMeterAndCMValue / TileDimensionAreaFeet;
                   ViewBag.lblAnswerTilesValue = Nooftiles.ToString("0") + " No. of Tiles";

                    #endregion Calculate tiles

                    #region Calculate cement

                    Decimal FlooringCementValue = 0;
                    Decimal FlooringCementBagVolume = 0.035m;
                    Decimal FlooringWetCement = FlooringSquareMeterAndCMValue * 0.07m;
                    FlooringCementValue = (((FlooringWetCement) / 7) / FlooringCementBagVolume);
                   ViewBag.lblAnswerFlooringCementValue = Math.Ceiling(FlooringCementValue).ToString("0") + " Bags";

                    #endregion Calculate cement

                    #region Calculate sand

                    Decimal FlooringSandValue = 0;
                    Decimal FlooringSandVolume = 1550;
                    FlooringSandValue = Math.Ceiling(((FlooringWetCement * 6) / 7) * FlooringSandVolume);
                 ViewBag.lblAnswerFlooringSandValue = (FlooringSandValue / 1000m).ToString("0.00") + " Ton";

                    #endregion Calculate sand

                    #region Formula

                    if (flooring.LengthB != null && flooring.WidthB != null)
                    {

                        ViewBag.lblFlooringFormula = @"<h4><b>Total Area</h4></b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = </mi><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = </mi><mrow><msub><mi>" + flooring.LengthA + "." + flooring.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + flooring.WidthA + "." + flooring.WidthB + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = " + FlooringSquareMeterAndCMValue.ToString("0.00") + " " + "</math><mo>&#xA0;</mo>m<sup>2</sup>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = " + FlooringSquareFeetAndInchValue.ToString("0.00") + " " + "</math><mo>&#xA0;</mo>ft<sup>2</sup>";

                    }
                    else
                    {
                        ViewBag.lblFlooringFormula = @"<h4><b>Total Area</h4></b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = </mi><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = </mi><mrow><msub><mi>" + flooring.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + flooring.WidthA + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = " + FlooringSquareMeterAndCMValue.ToString("0.00") + " " + "</math><mo>&#xA0;</mo>m<sup>2</sup>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = " + FlooringSquareFeetAndInchValue.ToString("0.00") + " " + "</math><mo>&#xA0;</mo>ft<sup>2</sup>";
                    }

                    ViewBag.lblNoofTiles = @"<h4><b>No. of Tiles</h4></b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Area of Tile = </mi></msub><msub><mi>Tile Length</mi></msub><mo>&#xD7;</mo><msub><mi>Tile Width</mi></msub></mrow>"
                                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Tile = </mi><mrow><msub><mi>" + TileDimensionAreaMeter.ToString("0.00") + "</mi></msub></mrow></math><mo>&#xA0;</mo>ft<sup>2</sup>"
                                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>No. of Tile Require = </mi><mfrac><mrow><msub><mi>Area of Flooring(ft)</mi></msub></mrow><msub><mn>Area of Tile (ft)</mn></msub></mfrac></math>"
                                                                  + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>= </mi><mfrac><mrow><msub><mi>" + FlooringSquareFeetAndInchValue.ToString("0.00") + "</mi></msub></mrow><msub><mn>" + TileDimensionAreaMeter.ToString("0.00") + "</mn></msub></mfrac></math>"
                                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Require Tile = " + (Nooftiles).ToString("0") + " No. of Tiles</mi></msub></mrow>";

                    ViewBag.lblFlooringSandFormula = @"<h4><b>Amount of Sand</h4></b> Volume With Bedding = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Flooring Area</mi></msub><mo>&#xD7;</mo><msub><mi>0.07m</mi></msub></mrow>"
                                                                + @"<br /><br />Volume With Bedding = " + (FlooringSquareMeterAndCMValue * 0.07m).ToString("0.00") + " " + "m<sup>3</sup>"
                                                                + @"<div class='alert alert-block alert-success text-center' style='margin: 5px;'><strong>Note: </strong> Assuming Thickness of Bedding is 0.07m<br />Ratio of Mortar 1:6 </div>"
                                                                + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>Volume With Bedding</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Sand Ratio</mi></mrow><mi>Total Ratio</mi></mfrac></mfenced><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                                + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>" + (FlooringSquareMeterAndCMValue * 0.07m).ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>6</mi></mrow><mi>7</mi></mfrac></mfenced><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                                + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Require Sand in ton = </mi><mo>&#xA0;</mo><mi>" + (FlooringSandValue / 1000).ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>Ton</mi>"
                                                                + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi> Require Sand in kg = </mi><mo>&#xA0;</mo><mi>" + FlooringSandValue.ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>kg.</mi>"
                                                                + @"<div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>By considering dry density of sand <strong>= 1550 kg/m<sup>3</sup>.</strong><br /><strong>1000 kg = 1 Ton</strong></div></h4>";

                    ViewBag.lblFlooringCementFormula = @"<h4><b>Amount of Cement</h4></b>  Volume With Bedding = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Flooring Area</mi></msub><mo>&#xD7;</mo><msub><mi>0.07m</mi></msub></mrow>"
                                                                + @"<br /><br />Volume With Bedding = " + (FlooringSquareMeterAndCMValue * 0.07m).ToString("0.00") + " " + "m<sup>3</sup>"
                                                                + @"<div class='alert alert-block alert-success text-center' style='margin: 5px;'><strong>Note: </strong> Assuming Thickness of Bedding is 0.07m<br />Ratio of Mortar 1:6 </div>"
                                                                + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>Volume With Bedding</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Cement Ratio</mi></mrow><mi>Total Ratio</mi></mfrac></mfenced><mo>&#xA0;</mo><mi>÷</mi><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>035</mn></math>"
                                                                + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>" + (FlooringSquareMeterAndCMValue * 0.07m).ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>1</mi></mrow><mi>7</mi></mfrac></mfenced><mo>&#xA0;</mo><mi>÷</mi><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>035</mn></math>"
                                                                + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>No. of Cement Bag Require = </mi><mo>&#xA0;</mo><mi>" + FlooringCementValue.ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>Bags</mi>"
                                                                + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Require Cement in kg = </mi><mo>&#xA0;</mo><mi>" + (FlooringCementValue * 50).ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>kg.</mi>"
                                                                + @"<div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>1 Bag of cement <strong>= 0.035 m<sup>3</sup>.</strong><br />1 Cement bag contains <strong>= 50 kg cement</strong></div>";

                    #endregion Formula

                    #region Chart Value

                   ViewBag.Cementchart = (FlooringCementValue * 50).ToString("0");
                   ViewBag.Sandchart = FlooringSandValue.ToString("0");
                   ViewBag.Tilechart = Nooftiles.ToString("0");
                    //ChartShow();

                    #endregion Chart Value
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function CalculateFlooringValueForMeterAndCM

        #region Function Calculate Flooring Value For Feet And Inch

        protected void CalculateFlooringValueForFeetAndInch(FlooringCalculator flooring)
        {
            try
            {
                if (flooring.LengthA != null && flooring.WidthA != null)
                {
                    #region Calculation

                    Decimal FlooringSquareFeetAndInchValue = CommonFunctions.Area(Convert.ToDecimal(flooring.LengthA + "." + flooring.LengthB), Convert.ToDecimal(flooring.WidthA + "." + flooring.WidthB));
                   ViewBag.lblAnswerFlooringSquareFeetAndInchValue = FlooringSquareFeetAndInchValue.ToString("0.00") + " ft<sup>2</sup>";
                    Decimal FlooringSquareMeterAndCMValue = CommonFunctions.ConvertMeterAndCMForArea(FlooringSquareFeetAndInchValue);
                    ViewBag.lblAnswerFlooringSquareMeterAndCMValue = FlooringSquareMeterAndCMValue.ToString("0.00") + " m<sup>2</sup>";

                    #endregion Calculation

                    #region Calculate Tiles

                    Decimal TileDimensionAreaMeter = (Convert.ToDecimal(flooring.TileLength) * Convert.ToDecimal(flooring.TileWidth));
                    Decimal TileDimensionAreaFeet = TileDimensionAreaMeter / 10.7639m;
                    Decimal Nooftiles = FlooringSquareMeterAndCMValue / TileDimensionAreaFeet;
                   ViewBag.lblAnswerTilesValue = Nooftiles.ToString("0") + " No. of Tiles";

                    #endregion Calculate Tiles

                    #region Calculate cement

                    Decimal FlooringCementValue = 0;
                    Decimal FlooringCementBagVolume = 0.035m;
                    Decimal FlooringWetCement = FlooringSquareMeterAndCMValue * 0.07m;
                    FlooringCementValue = (((FlooringWetCement) / 7) / FlooringCementBagVolume);
                   ViewBag.lblAnswerFlooringCementValue = Math.Ceiling(FlooringCementValue).ToString("0") + " Bags";

                    #endregion Calculate cement

                    #region Calculate sand

                    Decimal FlooringSandValue = 0;
                    Decimal FlooringSandVolume = 1550;
                    FlooringSandValue = Math.Ceiling(((FlooringWetCement * 6) / 7) * FlooringSandVolume);
                   ViewBag.lblAnswerFlooringSandValue = (FlooringSandValue / 1000m).ToString("0.00") + " Ton";

                    #endregion Calculate sand

                    #region Formula

                    if (flooring.LengthB != null && flooring.WidthB != null)
                    {

                        ViewBag.lblFlooringFormula = @"<h4><b>Flooring Area</h4></b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = </mi><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = </mi><mrow><msub><mi>" + flooring.LengthA + "." + flooring.LengthB + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + flooring.WidthA + "." + flooring.WidthB + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = " + FlooringSquareFeetAndInchValue.ToString("0.00") + " " + "</math><mo>&#xA0;</mo>ft<sup>2</sup>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = " + FlooringSquareMeterAndCMValue.ToString("0.00") + " " + "</math><mo>&#xA0;</mo>m<sup>2</sup>";
                    }
                    else
                    {
                        ViewBag.lblFlooringFormula = @"<h4><b>Flooring Area</h4></b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = </mi><mrow><msub><mi>Length</mi></msub><mo>&#xD7;</mo><msub><mi>Width</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = </mi><mrow><msub><mi>" + flooring.LengthA + "</mi></msub><mo>&#xD7;</mo><msub><mi> " + flooring.WidthA + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = " + FlooringSquareFeetAndInchValue.ToString("0.00") + " " + "</math><mo>&#xA0;</mo>ft<sup>2</sup>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Flooring = " + FlooringSquareMeterAndCMValue.ToString("0.00") + " " + "</math><mo>&#xA0;</mo>m<sup>2</sup>";
                    }


                   ViewBag.lblNoofTiles = @"<h4><b>No. of Tiles</h4></b><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Area of Tile = </mi></msub><msub><mi>Tile Length</mi></msub><mo>&#xD7;</mo><msub><mi>Tile Width</mi></msub></mrow>"
                                                              + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Area of Tile = </mi><mrow><msub><mi>" + TileDimensionAreaMeter.ToString("0.00") + "</mi></msub></mrow></math><mo>&#xA0;</mo>ft<sup>2</sup>"
                                                              + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>No. of Tile Require = </mi><mfrac><mrow><msub><mi>Area of Flooring(ft)</mi></msub></mrow><msub><mn>Area of Tile</mn></msub></mfrac></math>"
                                                              + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>= </mi><mfrac><mrow><msub><mi>" + FlooringSquareFeetAndInchValue.ToString("0.00") + "</mi></msub></mrow><msub><mn>" + TileDimensionAreaMeter.ToString("0.00") + "</mn></msub></mfrac></math>"
                                                              + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Require Tile = " + (Nooftiles).ToString("0") + " No. of Tiles</mi></msub></mrow>";

                  ViewBag.lblFlooringSandFormula = @"<h4><b>Amount of Sand</h4></b> Volume With Bedding = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Flooring Area</mi></msub><mo>&#xD7;</mo><msub><mi>0.07m</mi></msub></mrow>"
                                                                + @"<br /><br />Volume With Bedding = " + (FlooringSquareMeterAndCMValue * 0.07m).ToString("0.00") + " " + "m<sup>3</sup>"
                                                                + @"<div class='alert alert-block alert-success text-center' style='margin: 5px;'><strong>Note: </strong> Assuming Thickness of Bedding is 0.07m<br />Ratio of Mortar 1:6 </div>"
                                                                + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>Volume With Bedding</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Sand Ratio</mi></mrow><mi>Total Ratio</mi></mfrac></mfenced><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                                + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>" + (FlooringSquareMeterAndCMValue * 0.07m).ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>6</mi></mrow><mi>7</mi></mfrac></mfenced><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mn>1550</mn></math>"
                                                                + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Require Sand in ton = </mi><mo>&#xA0;</mo><mi>" + (FlooringSandValue / 1000).ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>Ton</mi>"
                                                                + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi> Require Sand in kg = </mi><mo>&#xA0;</mo><mi>" + FlooringSandValue.ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>kg.</mi>"
                                                                + @"<div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>By considering dry density of sand <strong>= 1550 kg/m<sup>3</sup>.</strong><br /><strong>1000 kg = 1 Ton</strong></div></h4>";

                   ViewBag.lblFlooringCementFormula = @"<h4><b>Amount of Cement</h4></b>  Volume With Bedding = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msub><mi>Flooring Area</mi></msub><mo>&#xD7;</mo><msub><mi>0.07m</mi></msub></mrow>"
                                                                + @"<br /><br />Volume With Bedding = " + (FlooringSquareMeterAndCMValue * 0.07m).ToString("0.00") + " " + "m<sup>3</sup>"
                                                                + @"<div class='alert alert-block alert-success text-center' style='margin: 5px;'><strong>Note: </strong> Assuming Thickness of Bedding is 0.07m<br />Ratio of Mortar 1:6 </div>"
                                                                + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>Volume With Bedding</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Cement Ratio</mi></mrow><mi>Total Ratio</mi></mfrac></mfenced><mo>&#xA0;</mo><mi>÷</mi><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>035</mn></math>"
                                                                + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>" + (FlooringSquareMeterAndCMValue * 0.07m).ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>1</mi></mrow><mi>7</mi></mfrac></mfenced><mo>&#xA0;</mo><mi>÷</mi><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>035</mn></math>"
                                                                + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>No. of Cement Bag Require = </mi><mo>&#xA0;</mo><mi>" + FlooringCementValue.ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>Bags</mi>"
                                                                + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Require Cement in kg = </mi><mo>&#xA0;</mo><mi>" + (FlooringCementValue * 50).ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>kg.</mi>"
                                                                + @"<div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>1 Bag of cement <strong>= 0.035 m<sup>3</sup>.</strong><br />1 Cement bag contains <strong>= 50 kg cement</strong></div>";

                    #endregion Formula

                    #region Chart Value

                   ViewBag.Cementchart = (FlooringCementValue * 50).ToString("0");
                   ViewBag.Sandchart = FlooringSandValue.ToString("0");
                   ViewBag.Tilechart = Nooftiles.ToString("0");
                    //ChartShow();

                    #endregion Chart Value
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function CalculateFlooringValueForFeetAndInch

        #region Insert Log Function

        public void CalculatorLogInsert(FlooringCalculator flooring)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Flooring Calculator";

                if (flooring.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(flooring.UnitID);

                if (flooring.LengthA != null)
                {
                    if (flooring.LengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(flooring.LengthA + "." + flooring.LengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(flooring.LengthA);
                }

                if (flooring.WidthA != null)
                {
                    if (flooring.WidthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(flooring.WidthA + "." + flooring.WidthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(flooring.WidthA);
                }

                if (flooring.TileLength != null)
                    entLOG_Calculation.ParamD = Convert.ToString(flooring.TileLength);

                if (flooring.TileWidth != null)
                    entLOG_Calculation.ParamE = Convert.ToString(flooring.TileWidth);

                if (flooring.UnitID == 1)
                {
                    if (ViewBag.lblAnswerFlooringSquareMeterAndCMValue.Trim() != String.Empty)
                    {
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerFlooringSquareMeterAndCMValue.Trim();
                        entLOG_Calculation.ParamG = ViewBag.lblAnswerFlooringSquareFeetAndInchValue.Trim();
                    }
                }
                else if (flooring.UnitID == 2)
                {
                    if (ViewBag.lblAnswerFlooringSquareFeetAndInchValue.Trim() != String.Empty)
                    {
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerFlooringSquareFeetAndInchValue.Trim();
                        entLOG_Calculation.ParamG = ViewBag.lblAnswerFlooringSquareMeterAndCMValue.Trim();
                    }
                }
                else
                {
                    if (ViewBag.lblAnswerFlooringSquareMeterAndCMValue.Trim() != String.Empty)
                    {
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerFlooringSquareMeterAndCMValue.Trim();
                        entLOG_Calculation.ParamG = ViewBag.lblAnswerFlooringSquareFeetAndInchValue.Trim();
                    }
                }

                if (ViewBag.lblAnswerFlooringCementValue.Trim() != String.Empty)
                    entLOG_Calculation.ParamH = ViewBag.lblAnswerFlooringCementValue.Trim() + " Bags";

                if (ViewBag.lblAnswerFlooringSandValue.Trim() != String.Empty)
                    entLOG_Calculation.ParamI = ViewBag.lblAnswerFlooringSandValue.Trim() + " Kg";

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
