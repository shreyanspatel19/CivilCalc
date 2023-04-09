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
    public class PlasteringCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Plastering-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Plastering-Calculator");


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

            return View("PlasteringCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(PlasteringCalculator plasteringmodel)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Plastering-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (plasteringmodel.UnitID == 2)
                CalculatePlasterValueForFeetAndInch(plasteringmodel);
            else
                CalculatePlasterValueForMeterAndCM(plasteringmodel);

            CalculatorLogInsert(plasteringmodel);

            return PartialView("_PlasteringCalculatorResult", vModel);
        }
        #endregion

        #region Function CalculatePlasterValueForFeetAndInch

        protected void CalculatePlasterValueForFeetAndInch(PlasteringCalculator plasteringmodel)
        {
            try
            {
                if (plasteringmodel.UnitID == 1)
                {
                    CalculatePlasterValueForMeterAndCM(plasteringmodel);
                }
                else
                {
                    if (plasteringmodel.LengthA != null && plasteringmodel.WidthA != null)
                    {
                        #region Local Varible
                        Decimal PlasterCubicMeterAndCMValue = 0;
                        Decimal PlasterCubicFeetAndInchValue = 0;
                        Decimal PlasterDryVolumeofMortar = 0;
                        Decimal PlasterTotalDryVolumeofMortar = 0;
                        Decimal PlasterSandValue = 0;
                        Decimal PlasterSandVolume = 1550;
                        Decimal PlasterCementValue = 0;
                        Decimal PlasterwetVolume = 0;
                        Decimal PlasterCementBagVolume = 0.035m;
                        Decimal Thickness = Convert.ToDecimal(plasteringmodel.PlasterID);
                        Decimal Cement = 1;
                        Decimal sand = Convert.ToDecimal(plasteringmodel.GradeID);
                        #endregion Local Varible

                        PlasterCubicFeetAndInchValue = CommonFunctions.Area(Convert.ToDecimal(plasteringmodel.LengthA + "." + plasteringmodel.LengthB), Convert.ToDecimal(plasteringmodel.WidthA + "." + plasteringmodel.WidthB));
                        PlasterCubicMeterAndCMValue = CommonFunctions.ConvertMeterAndCMForArea(PlasterCubicFeetAndInchValue);

                        #region Dry Volune of Mortar
                        //Add 30% to fill up joins and Cover surface

                        PlasterwetVolume = (PlasterCubicMeterAndCMValue * (Thickness));
                        PlasterDryVolumeofMortar = PlasterwetVolume * 1.3m;
                        #endregion Dry Volune of Mortar

                        #region Total Dry Volume of Mortar
                        //Increases By 25% of total dry volume

                        PlasterTotalDryVolumeofMortar = PlasterDryVolumeofMortar * 1.25m;

                        #endregion Total Dry Volume of Mortar

                        #region Cement Require for Plaster
                        //Check the Ratio first if 1:4 then there are 1 part of cement and 4 part of sand
                        //1 bag of cement is = 0.035 m^3 cement

                        PlasterCementValue = Math.Ceiling(((Cement / (Cement + sand)) * PlasterTotalDryVolumeofMortar) / 0.035m);
                       ViewBag.lblAnswerPlasterCement = Math.Floor(PlasterCementValue).ToString("0") + " Bags";

                        #endregion Cement Require for Plaster

                        #region sand Require for Plaster
                        //by the considering dry loose basic density of sand is 1550 kg/m^3

                        PlasterSandValue = Math.Round(((sand / (Cement + sand)) * PlasterTotalDryVolumeofMortar) * 1550);
                       ViewBag.lblAnswerPlasterSand = (PlasterSandValue / 1000).ToString("0.00") + " Ton";

                        #endregion sand Require for Plaster

                        #region Formula

                        if (plasteringmodel.LengthB != null || plasteringmodel.WidthB != null)
                        {
                           ViewBag.lblPlasterAreaFormula = @"<h4><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>PlasterArea = </mi></msup><msup><mi> Length</mi></msup><msup><mi> &#xD7</mi></msup><msup><mi> Width</mi></msup></mrow> "
                                                                 + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + plasteringmodel.LengthA + "." + plasteringmodel.LengthB + "</mi></msup><mo>&#xD7;</mo><msub><mi>" + plasteringmodel.WidthA + "." + plasteringmodel.WidthB + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + PlasterCubicMeterAndCMValue.ToString("0.00") + "</mi></msup><msup><mi>m</mi><mn>2</mn></msup></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + PlasterCubicFeetAndInchValue.ToString("0.00") + "</mi></msup><msup><mi>ft</mi><mn>2</mn></h4></msup></mrow>";
                        }
                        else
                        {
                           ViewBag.lblPlasterAreaFormula = @"<h4><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>PlasterArea = </mi></msup><msup><mi> Length</mi></msup><msup><mi> &#xD7</mi></msup><msup><mi> Width</mi></msup></mrow> "
                                                                 + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + plasteringmodel.LengthA + "</mi></msup><mo>&#xD7;</mo><msub><mi>" + plasteringmodel.WidthA + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + PlasterCubicMeterAndCMValue.ToString("0.00") + "</mi></msup><msup><mi>m</mi><mn>2</mn></msup></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + PlasterCubicFeetAndInchValue.ToString("0.00") + "</mi></msup><msup><mi>ft</mi><mn>2</mn></h4></msup></mrow>";
                        }



                        #region Mortar Formula

                       ViewBag.lblPlasterMortar = @"<h4 class=""bold"">Step 1:</h4>"
                                                                 + @"<h6><div class='table-responsive'> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Volume Of Mortar =</mi><mrow><mi>PlasterArea</mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Plaster Thickness In Meter</mi></mrow></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Volume Of Mortar =</mi><mrow><mi> " + PlasterCubicMeterAndCMValue.ToString("0.00") + " </mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi> " + Thickness + " </mi></mrow></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><msup><mi>Volume Of Mortar =</mi></msup><msup><mi> " + PlasterwetVolume.ToString("0.00") + "</msup><msup><mi> m</mi><mn>3</mn></msup></mi>"
                                                                 + @"<br /><br /><div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>Add 30% to fill up join & Cover surface</div>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Volume Of Mortar =</mi><mi>" + PlasterwetVolume.ToString("0.00") + "</mi><mi><mn> +</mn></mi><mfenced><mrow><mn>" + PlasterwetVolume.ToString("0.00") + "</mn><mo>&#xD7;</mo><mn>" + 0.3 + "</mn></mrow></mfenced></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><msup><mi>Volume Of Mortar =</mi></msup><msup><mi>" + PlasterDryVolumeofMortar.ToString("0.00") + "</msup><msup><mi> m </mi><mn>3</mn></msup></mi>"
                                                                 + @"<br /><br /><div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong> Increases by 25% of the total dry volume</div>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Dry Volume Of Mortar =</mi><mi>" + PlasterDryVolumeofMortar.ToString("0.00") + "</mi><mi><mn> +</mn></mi><mfenced><mrow><mn>" + PlasterDryVolumeofMortar.ToString("0.00") + "</mn><mo>&#xD7;</mo><mn>" + 0.25 + "</mn></mrow></mfenced></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><msup><mi>Dry Volume Of Mortar =</mi></msup><msup><mi>" + PlasterTotalDryVolumeofMortar.ToString("0.00") + "</mi></msup><msup><mi> m </mi><mn>3</mn></msup></mi></div></h6>";

                        #endregion Mortar Formula

                        #region Cement Formula

                       ViewBag.lblPlasterCementFormula = @"<h4 class=""bold"">Step 2: Amount of Cement Require </h4>"
                                                                 + @"<h6><div class='table-responsive'> = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mi>D</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mi>R</mi><mi>a</mi><mi>t</mi><mi>i</mi><mi>o</mi></mrow><mrow><mi>S</mi><mi>u</mi><mi>m</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>R</mi><mi>a</mi><mi>t</mi><mi>i</mi><mi>o</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo><mi>÷</mi><mi>Volume of Cement Bag</mi></math>"
                                                                 + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>" + PlasterTotalDryVolumeofMortar.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>" + Cement + "</mi></mrow><mi>" + (Cement + sand) + "</mi></mfrac></mfenced><mo>&#xA0;</mo><mi>÷</mi><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>035</mn></math>"
                                                                 + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>No. of Cement Bag Require = </mi><mo>&#xA0;</mo><mi>" + PlasterCementValue.ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>Bags</mi>"
                                                                 + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Require Cement in Kg = </mi><mo>&#xA0;</mo><mi>" + (PlasterCementValue * 50).ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>kg.</mi>"
                                                                 + @"<div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>1 Bag of cement <strong>= 0.035 m<sup>3</sup>.</strong><br />1 Cement bag contains <strong>= 50 kg cement</strong></div></div></h6><hr/>";

                        #endregion Cement Formula

                        #region Sand Formula

                       ViewBag.lblPlasterSandFormula = @"<h4 class=""bold"">Step 3: Quantity of Sand Require </h4>"
                                                                 + @"<h6><div class='table-responsive'>= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mi>D</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Sand</mi><mo>&#xA0;</mo><mi>R</mi><mi>a</mi><mi>t</mi><mi>i</mi><mi>o</mi></mrow><mrow><mi>S</mi><mi>u</mi><mi>m</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>R</mi><mi>a</mi><mi>t</mi><mi>i</mi><mi>o</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Density of Sand</mi></math>"
                                                                 + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>" + PlasterTotalDryVolumeofMortar.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + sand + "</mn></mrow><mn>" + (Cement + sand) + "</mn></mfrac></mfenced><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>1550</mn></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Qauntity of Sand require :</mi><mo>&#xA0;</mo><mi> " + PlasterSandValue.ToString("0") + "</mi><mo>&#xA0;</mo><mi>Kg</mi>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Qauntity of Sand require :</mi><mo>&#xA0;</mo><mi> " + (PlasterSandValue / 1000).ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>Ton</mi></div></h6>"
                                                                 + @"<div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>By considering dry density of sand <strong>= 1550 kg/m<sup>3</sup>.</strong><br /><strong>1000 kg = 1 Ton</strong></div>";
                        #endregion Sand Formula

                        #endregion Formula

                        #region ChartValue
                       ViewBag.Cementchart = (PlasterCementValue * 50).ToString("0");
                        ViewBag.Sandchart = PlasterSandValue.ToString("0");
                        #endregion ChartValue
                    }
                }
            }
            catch (Exception e)
            {
                //Message.ShowError(e.Message);
            }
        }
        #endregion Function CalculatePlasterValueForFeetAndInch   

        #region Function CalculatePlasterValueForMeterAndCM

        protected void CalculatePlasterValueForMeterAndCM(PlasteringCalculator plasteringmodel)
        {
            try
            {
                if (plasteringmodel.UnitID == 2)
                {
                    CalculatePlasterValueForFeetAndInch(plasteringmodel);
                }
                else
                {
                    if (plasteringmodel.LengthA != null && plasteringmodel.WidthA != null)
                    {
                        #region Local Varible
                        Decimal PlasterCubicMeterAndCMValue = 0;
                        Decimal PlasterCubicFeetAndInchValue = 0;
                        Decimal PlasterDryVolumeofMortar = 0;
                        Decimal PlasterTotalDryVolumeofMortar = 0;
                        Decimal PlasterSandValue = 0;
                        Decimal PlasterCementValue = 0;
                        Decimal PlasterwetVolume = 0;
                        Decimal Thickness = Convert.ToDecimal(plasteringmodel.PlasterID);
                        Decimal Cement = 1;
                        Decimal sand = Convert.ToDecimal(plasteringmodel.GradeID);
                        #endregion Local Varible

                        PlasterCubicMeterAndCMValue = CommonFunctions.Area(Convert.ToDecimal(plasteringmodel.LengthA + "." + plasteringmodel.LengthB), Convert.ToDecimal(plasteringmodel.WidthA + "." + plasteringmodel.WidthB));
                        PlasterCubicFeetAndInchValue = CommonFunctions.ConvertFeetAndInchForArea(PlasterCubicMeterAndCMValue);
                       ViewBag.lblAnswerPlaster = PlasterCubicMeterAndCMValue.ToString("0.00") + " m<sup>2</sup>" + " <br /><small>or</small><br /> " + PlasterCubicFeetAndInchValue.ToString("0.00") + " ft<sup>2</sup>";

                        #region Dry Volune of Mortar
                        //Add 30% to fill up joins and Cover surface

                        PlasterwetVolume = (PlasterCubicMeterAndCMValue * (Thickness));
                        PlasterDryVolumeofMortar = PlasterwetVolume * 1.3m;
                        #endregion Dry Volune of Mortar

                        #region Total Dry Volume of Mortar
                        //Increases By 25% of total dry volume

                        PlasterTotalDryVolumeofMortar = PlasterDryVolumeofMortar * 1.25m;

                        #endregion Total Dry Volume of Mortar

                        #region Cement Require for Plaster
                        //Check the Ratio first if 1:4 then there are 1 part of cement and 4 part of sand
                        //1 bag of cement is = 0.035 m^3 cement

                        PlasterCementValue = (((Cement / (Cement + sand)) * PlasterTotalDryVolumeofMortar) / 0.035m);
                       ViewBag.lblAnswerPlasterCement = Math.Ceiling(PlasterCementValue).ToString("0") + " Bags";

                        #endregion Cement Require for Plaster

                        #region sand Require for Plaster
                        //by the considering dry loose basic density of sand is 1550 kg/m^3

                        PlasterSandValue = Math.Round(((sand / (Cement + sand)) * PlasterTotalDryVolumeofMortar) * 1550);
                       ViewBag.lblAnswerPlasterSand = (PlasterSandValue / 1000).ToString("0.00") + " Ton";

                        #endregion sand Require for Plaster

                        #region Formula

                        #region Area Formula
                        if (plasteringmodel.LengthB != null || plasteringmodel.WidthB != null)
                        {


                           ViewBag.lblPlasterAreaFormula = @"<h4><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>PlasterArea = </mi></msup><msup><mi> Length</mi></msup><msup><mi> &#xD7</mi></msup><msup><mi> Width</mi></msup></mrow> "
                                                                 + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + plasteringmodel.LengthA + "." + plasteringmodel.LengthB + "</mi></msup><mo>&#xD7;</mo><msub><mi>" + plasteringmodel.WidthA + "." + plasteringmodel.WidthB + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + PlasterCubicMeterAndCMValue.ToString("0.00") + "</mi></msup><msup><mi>m</mi><mn>2</mn></msup></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + PlasterCubicFeetAndInchValue.ToString("0.00") + "</mi></msup><msup><mi>ft</mi><mn>2</mn></h4></msup></mrow>";

                        }
                        else
                        {
                           ViewBag.lblPlasterAreaFormula = @"<h4><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>PlasterArea = </mi></msup><msup><mi> Length</mi></msup><msup><mi> &#xD7</mi></msup><msup><mi> Width</mi></msup></mrow> "
                                                                 + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + plasteringmodel.LengthA + "</mi></msup><mo>&#xD7;</mo><msub><mi>" + plasteringmodel.WidthA + "</mi></msub></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + PlasterCubicMeterAndCMValue.ToString("0.00") + "</mi></msup><msup><mi>m</mi><mn>2</mn></msup></mrow>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mrow><msup><mi>Plaster Area = </mi></msup><msup><mi> " + PlasterCubicFeetAndInchValue.ToString("0.00") + "</mi></msup><msup><mi>ft</mi><mn>2</mn></h4></msup></mrow>";
                        }
                        #endregion Area Formula

                        #region Mortar Formula

                       ViewBag.lblPlasterMortar = @"<h4 class=""bold"">Step 1:</h4>"
                                                                 + @"<h6><div class='table-responsive'> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Volume Of Mortar =</mi><mrow><mi>PlasterArea</mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Plaster Thickness In Meter</mi></mrow></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Volume Of Mortar =</mi><mrow><mi> " + PlasterCubicMeterAndCMValue.ToString("0.00") + " </mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi> " + Thickness + " </mi></mrow></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><msup><mi>Volume Of Mortar =</mi></msup><msup><mi> " + PlasterwetVolume.ToString("0.00") + "</msup><msup><mi> m</mi><mn>3</mn></msup></mi>"
                                                                 + @"<br /><br /><div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>Add 30% to fill up join & Cover surface</div>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Volume Of Mortar =</mi><mi>" + PlasterwetVolume.ToString("0.00") + "</mi><mi><mn> +</mn></mi><mfenced><mrow><mn>" + PlasterwetVolume.ToString("0.00") + "</mn><mo>&#xD7;</mo><mn>" + 0.3 + "</mn></mrow></mfenced></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><msup><mi>Volume Of Mortar =</mi></msup><msup><mi>" + PlasterDryVolumeofMortar.ToString("0.00") + "</msup><msup><mi> m </mi><mn>3</mn></msup></mi>"
                                                                 + @"<br /><br /><div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong> Increases by 25% of the total dry volume</div>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Dry Volume Of Mortar =</mi><mi>" + PlasterDryVolumeofMortar.ToString("0.00") + "</mi><mi><mn> +</mn></mi><mfenced><mrow><mn>" + PlasterDryVolumeofMortar.ToString("0.00") + "</mn><mo>&#xD7;</mo><mn>" + 0.25 + "</mn></mrow></mfenced></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><msup><mi>Dry Volume Of Mortar =</mi></msup><msup><mi>" + PlasterTotalDryVolumeofMortar.ToString("0.00") + "</mi></msup><msup><mi> m </mi><mn>3</mn></msup></mi></div></h6>";

                        #endregion Mortar Formula

                        #region Cement Formula

                       ViewBag.lblPlasterCementFormula = @"<h4 class=""bold"">Step 2: Amount of Cement Require </h4>"
                                                                 + @"<h6><div class='table-responsive'> = <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mi>D</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mi>R</mi><mi>a</mi><mi>t</mi><mi>i</mi><mi>o</mi></mrow><mrow><mi>S</mi><mi>u</mi><mi>m</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>R</mi><mi>a</mi><mi>t</mi><mi>i</mi><mi>o</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo><mi>÷</mi><mi>Volume of Cement Bag</mi></math>"
                                                                 + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>" + PlasterTotalDryVolumeofMortar.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>" + Cement + "</mi></mrow><mi>" + (Cement + sand) + "</mi></mfrac></mfenced><mo>&#xA0;</mo><mi>÷</mi><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>035</mn></math>"
                                                                 + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>No. of Cement Bag Require = </mi><mo>&#xA0;</mo><mi>" + PlasterCementValue.ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>Bags</mi>"
                                                                 + @"<br /><br /> <math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Require Cement in Kg = </mi><mo>&#xA0;</mo><mi>" + (PlasterCementValue * 50).ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>kg.</mi>"
                                                                 + @"<div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>1 Bag of cement <strong>= 0.035 m<sup>3</sup>.</strong><br />1 Cement bag contains <strong>= 50 kg cement</strong></div></div></h6><hr/>";

                        #endregion Cement Formula

                        #region Sand Formula

                       ViewBag.lblPlasterSandFormula = @"<h4 class=""bold"">Step 3: Quantity of Sand Require </h4>"
                                                                 + @"<h6><div class='table-responsive'>= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mi>D</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Sand</mi><mo>&#xA0;</mo><mi>R</mi><mi>a</mi><mi>t</mi><mi>i</mi><mi>o</mi></mrow><mrow><mi>S</mi><mi>u</mi><mi>m</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>R</mi><mi>a</mi><mi>t</mi><mi>i</mi><mi>o</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Density of Sand</mi></math>"
                                                                 + @"<br /><br />= <math xmlns=""http://www.w3.org/1998/Math/MathML""><mfenced><mfrac><mrow><mn>" + PlasterTotalDryVolumeofMortar.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + sand + "</mn></mrow><mn>" + (Cement + sand) + "</mn></mfrac></mfenced><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>1550</mn></math>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Qauntity of Sand require :</mi><mo>&#xA0;</mo><mi> " + PlasterSandValue.ToString("0") + "</mi><mo>&#xA0;</mo><mi>Kg</mi>"
                                                                 + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Qauntity of Sand require :</mi><mo>&#xA0;</mo><mi> " + (PlasterSandValue / 1000).ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>Ton</mi></div></h6>"
                                                                 + @"<div class='alert alert-block alert-success text-center' style='margin: 30px;'><strong>Note: </strong>By considering dry density of sand <strong>= 1550 kg/m<sup>3</sup>.</strong><br /><strong>1000 kg = 1 Ton</strong></div>";
                        #endregion Sand Formula

                        #endregion Formula

                        #region ChartValue
                       ViewBag.Cementchart = (PlasterCementValue * 50).ToString("0");
                       ViewBag.Sandchart = PlasterSandValue.ToString("0");
                        //ChartShow();
                        #endregion ChartValue
                    }
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function CalculatePlasterValueForMeterAndCM

        #region Insert Log Function
        public void CalculatorLogInsert(PlasteringCalculator plasteringmodel)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region  gather data
                entLOG_Calculation.ScreenName = "Plaster Calculator";

                if (plasteringmodel.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(plasteringmodel.UnitID);

                if (plasteringmodel.LengthA != null)
                {
                    if (plasteringmodel.LengthB != null)
                    {
                        entLOG_Calculation.ParamB = Convert.ToString(plasteringmodel.LengthA + "." + plasteringmodel.LengthB);
                    }
                    else
                    {
                        entLOG_Calculation.ParamB = Convert.ToString(plasteringmodel.LengthA);
                    }
                }

                if (plasteringmodel.WidthA != null)
                {
                    if (plasteringmodel.WidthB != null)
                    {
                        entLOG_Calculation.ParamC = Convert.ToString(plasteringmodel.WidthA + "." + plasteringmodel.WidthB);
                    }
                    else
                    {
                        entLOG_Calculation.ParamC = Convert.ToString(plasteringmodel.WidthA);
                    }
                }

                entLOG_Calculation.ParamD = ViewBag.lblAnswerPlaster;

                if (plasteringmodel.PlasterID != -1)
                    entLOG_Calculation.ParamE = Convert.ToString(plasteringmodel.PlasterID);

                if (plasteringmodel.GradeID != -1)
                    entLOG_Calculation.ParamF = Convert.ToString(plasteringmodel.GradeID);

                entLOG_Calculation.ParamG = "Cement " + ViewBag.lblAnswerPlasterCement;
                entLOG_Calculation.ParamH = "Sand " + ViewBag.lblAnswerPlasterSand;

                entLOG_Calculation.Created = DateTime.Now;
                entLOG_Calculation.Modified = DateTime.Now;
                #endregion  gather data

                #region insert
                DBConfig.dbLOGCalculation.Insert(entLOG_Calculation);
                #endregion insert
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Insert Log Function

    }
}
