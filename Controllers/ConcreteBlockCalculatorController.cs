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
    public class ConcreteBlockCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Concrete-Block-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Concrete-Block-Calculator");


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

            return View("ConcreteBlockCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(ConcreteBlockCalculator concreteblock)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Concrete-Block-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            CalculateBlockValue(concreteblock);
            CalculatorLogInsert(concreteblock);

            return PartialView("_ConcreteBlockCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Block Value

        protected void CalculateBlockValue(ConcreteBlockCalculator concreteblock)
        {
            try
            {
                if (concreteblock.WallLengthA != null && concreteblock.WallDepthA != null)
                {
                    #region Variables

                    Decimal WallLenthInMeter = 0, WallDepthInMeter = 0;
                    Decimal WallThicknessInMeter = 0;
                    Decimal BlockLength = 0, BlockWidth = 0, BlockHeight = 0;
                    Decimal BlockLengthmeter = 0, BlockWidthmeter = 0, BlockHeightmeter = 0;
                    Decimal BlockLengthWithMortar = 0, BlockWidthWithMortar = 0, BlockHeightWithMortar = 0;
                    Decimal VolumeBlockWithoutMortar = 0;
                    Decimal ActualVolumeBlockWithoutMortar = 0;
                    Decimal QuantityMortar = 0, Quantity15Wastage = 0, Quantity25Wastage = 0;
                    Decimal RatioCement = 0, RatioSand = 0;
                    Decimal AmountCement = 0, AmountsandTon = 0;
                    Decimal BlockValueForMeterAndCM = 0;
                    Decimal BlockValueForFeetAndInch = 0;
                    Decimal NoofBlocks = 0;
                    Decimal KGofSands = 0, KGofSandsTon = 0;
                    Decimal NoofCementBags = 0, KGofCement = 0;

                    #endregion Variables

                    #region Load drop down value

                    #region Get Wall Thickness value

                    if (concreteblock.WallThicknessID == 3)
                        WallThicknessInMeter = Convert.ToDecimal(concreteblock.OtherWallThickness) / 100m;
                    else
                        WallThicknessInMeter = Convert.ToDecimal(concreteblock.WallThicknessID);

                    #endregion Get Wall Thickness value

                    #region Get concreteblock.RatioID Value
                    RatioCement = 1;
                    RatioSand = Convert.ToDecimal(concreteblock.RatioID);
                    #endregion Get concreteblock.RatioID Value

                    #endregion Load drop down value

                    #region Load TextBoxes Value

                    BlockLength = Convert.ToDecimal(concreteblock.LengthBrick.ToString());
                    BlockWidth = Convert.ToDecimal(concreteblock.WidthBrick.ToString());
                    BlockHeight = Convert.ToDecimal(concreteblock.HeightBrick.ToString());
                    //WallLenth = Convert.ToDecimal(txtWallLengthA.Text.Trim() + "." + txtWallLengthB.Text.Trim());
                    //WallDepth = Convert.ToDecimal(txtWallDepthA.Text.Trim() + "." + txtWallDepthB.Text.Trim());

                    #endregion Load TextBoxes Value

                    #region Calculate Wall Length & Depth
                    if (concreteblock.UnitID == 1)
                    {
                        #region Length In Meter
                        Decimal InputLengthInMeter = 0;
                        Decimal InputLengthInCM = 0;
                        if (concreteblock.WallLengthA != null)
                            InputLengthInMeter = Convert.ToDecimal(concreteblock.WallLengthA);
                        else
                            InputLengthInMeter = 0;


                        if (concreteblock.WallLengthB != null)
                            InputLengthInCM = Convert.ToDecimal(concreteblock.WallLengthB);
                        else
                            InputLengthInCM = 0;

                        WallLenthInMeter = CommonFunctions.MeterAndCMToMeter(InputLengthInMeter, InputLengthInCM);

                        #endregion Length In Meter

                        #region Depth In Meter
                        Decimal InputDepthInMeter = 0;
                        Decimal InputDepthInCM = 0;
                        if (concreteblock.WallDepthA != null)
                            InputDepthInMeter = Convert.ToDecimal(concreteblock.WallDepthA);
                        else
                            InputDepthInMeter = 0;


                        if (concreteblock.WallDepthB != null)
                            InputDepthInCM = Convert.ToDecimal(concreteblock.WallDepthB);
                        else
                            InputDepthInCM = 0;

                        WallDepthInMeter = CommonFunctions.MeterAndCMToMeter(InputDepthInMeter, InputDepthInCM);

                        #endregion Depth In Meter
                    }
                    else
                    {
                        #region Length In Feet
                        Decimal InputLengthInFeet = 0;
                        Decimal InputLengthInInch = 0;
                        if (concreteblock.WallLengthA != null)
                            InputLengthInFeet = Convert.ToDecimal(concreteblock.WallLengthA);
                        else
                            InputLengthInFeet = 0;


                        if (concreteblock.WallLengthB != null)
                            InputLengthInInch = Convert.ToDecimal(concreteblock.WallLengthB);
                        else
                            InputLengthInInch = 0;

                        Decimal WallLenthInFeet = CommonFunctions.FeetAndInchToFeet(InputLengthInFeet, InputLengthInInch);

                        WallLenthInMeter = CommonFunctions.FeetToMeter(WallLenthInFeet);

                        #endregion Length In Feet

                        #region Depth In Feet
                        Decimal InputDepthInFeet = 0;
                        Decimal InputDepthInInch = 0;
                        if (concreteblock.WallDepthA != null)
                            InputDepthInFeet = Convert.ToDecimal(concreteblock.WallDepthA);
                        else
                            InputDepthInFeet = 0;


                        if (concreteblock.WallDepthB != null )
                            InputDepthInInch = Convert.ToDecimal(concreteblock.WallDepthB);
                        else
                            InputDepthInInch = 0;

                        Decimal WallDepthInFeet = CommonFunctions.FeetAndInchToFeet(InputDepthInFeet, InputDepthInInch);

                        WallDepthInMeter = CommonFunctions.FeetToMeter(WallDepthInFeet);

                        #endregion Depth In Feet
                    }

                    #endregion Calculate Wall Length & Depth

                    #region caluclation

                    BlockValueForMeterAndCM = CommonFunctions.Volume(WallLenthInMeter, WallDepthInMeter, WallThicknessInMeter);
                    BlockValueForFeetAndInch = CommonFunctions.ConvertFeetAndInchForVolume(BlockValueForMeterAndCM);
                   ViewBag.lblAnswerBlockMeterAndCMValue = BlockValueForMeterAndCM.ToString("0.00") + " m<sup>3</sup>";
                    ViewBag.lblAnswerBlockFeetAndInchValue = BlockValueForFeetAndInch.ToString("0.00") + " ft<sup>3</sup>";

                    //if (ddlUnit == "Meter/CM")
                    //{
                    //    BlockValueForMeterAndCM = CommonFunctions.Volume(WallLenth, WallDepth, WallThickness);
                    //    BlockValueForFeetAndInch = CommonFunctions.ConvertFeetAndInchForVolume(BlockValueForMeterAndCM);
                    //    lblAnswerBlockMeterAndCMValue.Text = BlockValueForMeterAndCM.ToString("0.00") + " m<sup>3</sup>";
                    //    lblAnswerBlockFeetAndInchValue.Text = BlockValueForFeetAndInch.ToString("0.00") + " ft<sup>3</sup>";
                    //}
                    //else
                    //{
                    //    BlockValueForFeetAndInch = CommonFunctions.Volume(WallLenth, WallDepth, WallThickness);
                    //    BlockValueForMeterAndCM = CommonFunctions.ConvertMeterAndCMForVolume(BlockValueForFeetAndInch);
                    //    lblAnswerBlockFeetAndInchValue.Text = BlockValueForFeetAndInch.ToString("0.00") + " ft<sup>3</sup>";
                    //    lblAnswerBlockMeterAndCMValue.Text = BlockValueForMeterAndCM.ToString("0.00") + " m<sup>3</sup>";
                    //}

                    #region Convert Block size in meter

                    BlockLengthmeter = BlockLength * 0.0254m;
                    BlockWidthmeter = BlockWidth * 0.0254m;
                    BlockHeightmeter = BlockHeight * 0.0254m;
                    VolumeBlockWithoutMortar = (BlockLengthmeter * BlockWidthmeter * BlockHeightmeter);

                    #endregion Convert Block size in meter

                    #region Volume of block with mortar block

                    BlockLengthWithMortar = BlockLengthmeter + 0.015m;
                    BlockWidthWithMortar = BlockWidthmeter + 0.015m;
                    BlockHeightWithMortar = BlockHeightmeter + 0.015m;

                    #endregion Volume of block with mortar

                    #region No of blocks

                    NoofBlocks = BlockValueForMeterAndCM / (BlockLengthWithMortar * BlockWidthWithMortar * BlockHeightWithMortar);
                    ViewBag.lblAmountBlocks = NoofBlocks.ToString("0");
                    ViewBag.lblAnswerTotalBlocksValue = NoofBlocks.ToString("0");

                    #endregion No of blocks

                    #region Volume of block without mortar

                    ActualVolumeBlockWithoutMortar = NoofBlocks * VolumeBlockWithoutMortar;

                    #endregion Volume of block without mortar

                    #region Quantity of Mortar

                    QuantityMortar = (BlockValueForMeterAndCM) - ActualVolumeBlockWithoutMortar;

                    #endregion Quantity of Mortar

                    #region Add 15% Mortar For Wastage

                    Quantity15Wastage = QuantityMortar * 1.15m;

                    #endregion Add 15% Mortar For Wastage

                    #region Add 25%  For Dry Volume

                    Quantity25Wastage = Quantity15Wastage * 1.25m;

                    #endregion Add 25%  For Dry Volume

                    #region Amount of cement Require

                    AmountCement = (RatioCement / (RatioSand + RatioCement)) * Quantity25Wastage;
                    KGofCement = AmountCement / 0.035m;
                    NoofCementBags = Math.Ceiling(KGofCement);
                    ViewBag.lblAmountCement = NoofCementBags == 1 ? NoofCementBags.ToString("0") + " Bag" : NoofCementBags.ToString("0") + " Bags";

                    #endregion Amount of cement Require

                    #region Amount of sand Require

                    KGofSands = (RatioSand / (RatioCement + RatioSand)) * Quantity25Wastage;
                    KGofSandsTon = KGofSands * Convert.ToDecimal(1550);
                    AmountsandTon = (KGofSandsTon / 1000m);
                    ViewBag.lblAmountSand = AmountsandTon.ToString("0.00") + " ton";

                    #endregion Amount of sand Require

                    #endregion end caluclation

                    #region Load Chart
                    ViewBag.ChartCement = (KGofCement * 50);
                    ViewBag.ChartSand = KGofSandsTon;
                    ViewBag.ChartBlock = NoofBlocks;
                    //ChartShow();
                    #endregion Load Chart

                    #region Formula

                    #region First Step Formula

                    ViewBag.lblStepI = @"<h4><b>Step 1 :</h4></b>"
                   + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Volume</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>C</mi><mi>o</mi><mi>n</mi><mi>c</mi><mi>r</mi><mi>e</mi><mi>t</mi><mi>e</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mi>L</mi><mi>e</mi><mi>n</mi><mi>g</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mi>D</mi><mi>e</mi><mi>p</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mi>W</mi><mi>a</mi><mi>l</mi><mi>l</mi><mo>&#xA0;</mo><mi>T</mi><mi>h</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>n</mi><mi>e</mi><mi>s</mi><mi>s</mi><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo></math>"
                   + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>C</mi><mi>o</mi><mi>n</mi><mi>c</mi><mi>r</mi><mi>e</mi><mi>t</mi><mi>e</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + WallLenthInMeter.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + WallDepthInMeter.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + WallThicknessInMeter.ToString("0.00") + "</mn></math>"
                   + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>C</mi><mi>o</mi><mi>n</mi><mi>c</mi><mi>r</mi><mi>e</mi><mi>t</mi><mi>e</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BlockValueForMeterAndCM.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                   + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>C</mi><mi>o</mi><mi>n</mi><mi>c</mi><mi>r</mi><mi>e</mi><mi>t</mi><mi>e</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BlockValueForFeetAndInch.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>ft</mi><mn>3</mn></msup></math>";

                    ViewBag.lblStepI += @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BlockLength + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>inch</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockWidth + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>inch</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockHeight + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>inch</mi><mo>)</mo></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BlockLength + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>0254</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BlockWidth + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>0254</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BlockHeight + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>02454</mn><mo>&#xA0;</mo><mo>)</mo></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BlockLengthmeter + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockWidthmeter + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockHeightmeter + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>w</mi><mi>i</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BlockLengthmeter + "</mn><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>015</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BlockWidthmeter + "</mn><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>015</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BlockHeightmeter + "</mn><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>015</mn><mo>&#xA0;</mo><mo>)</mo></math>"
                    + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>w</mi><mi>i</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BlockLengthWithMortar + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockWidthWithMortar + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockHeightWithMortar + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mfrac><mrow><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>C</mi><mi>o</mi><mi>n</mi><mi>c</mi><mi>r</mi><mi>e</mi><mi>t</mi><mi>e</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo></mrow><mrow><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>o</mi><mi>n</mi><mi>e</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi></mrow></mfrac></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mfrac><mrow><mn>" + BlockValueForMeterAndCM.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></mrow><mrow><mn>" + BlockLengthWithMortar + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockWidthWithMortar + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockHeightWithMortar + "</mn></mrow></mfrac></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + NoofBlocks.ToString("0") + "</mn><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>W</mi><mi>i</mi><mi>t</mi><mi>h</mi><mi>o</mi><mi>u</mi><mi>t</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + NoofBlocks.ToString("0") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BlockLengthmeter.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockWidthmeter.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BlockHeightmeter.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>)</mo></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + NoofBlocks.ToString("0") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + VolumeBlockWithoutMortar.ToString("0.0000") + "</mn></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + ActualVolumeBlockWithoutMortar.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>&#xA0;</mo><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>n</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>a</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>-</mo><mo>&#xA0;</mo><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>l</mi><mi>o</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>w</mi><mi>i</mi><mi>t</mi><mi>h</mi><mi>o</mi><mi>u</mi><mi>t</mi><mo>&#xA0;</mo><mi>m</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>&#xA0;</mo><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BlockValueForMeterAndCM.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>-</mo><mo>&#xA0;</mo><mn>" + ActualVolumeBlockWithoutMortar.ToString("0.0000") + "</mn></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>&#xA0;</mo><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + QuantityMortar.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                    + @"<div class=""alert alert-block alert-success text-center"" style=""margin: 10px;"">Add 15% more for wastage, Non - uniform thickness of mortar joins</div>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + QuantityMortar.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mfenced><mrow><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + QuantityMortar.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mfrac><mn>15</mn><mn>100</mn></mfrac><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo></mrow></mfenced></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + Quantity15Wastage.ToString("0.0000") + "</mn></math>"
                    + @"<div class=""alert alert-block alert-success text-center"" style=""margin: 10px;"">Add 25% more for Dry Volume</div>"
                    + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + Quantity15Wastage.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mfenced><mrow><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + Quantity15Wastage.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mfrac><mn>25</mn><mn>100</mn></mfrac><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo></mrow></mfenced></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + Quantity25Wastage.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>";

                    #endregion First Step Formula

                    #region Second Step Formula

                    ViewBag.lblStepII = @"<h4><b>Step 2 :</h4></b>"

                        + @"<h4><b>Amount of Cement</h4></b><h5><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo></mo><mfrac><mi>Cement</mi><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Qauntity Of Mortar</mi></math>"
                        + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mo>=</mo><mfrac><mn>1</mn><mrow><mn>" + (RatioSand + RatioCement) + "</mn><mo>&#xA0;</mo></mrow></mfrac><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + Quantity25Wastage.ToString("0.0000") + "</mn></math>"
                        + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mo>=</mo><mn>" + AmountCement.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                        + @"</h5><div class=""alert alert-block alert-success"" style=""margin: 10px;"">1 Bag of Cement  = 0.035 m<sup>3</sup></div>"
                        + @"<h5><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>f</mi><mi>o</mi><mo>&#xA0;</mo><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mi>B</mi><mi>a</mi><mi>g</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mfrac><mn>" + AmountCement.ToString("0.0000") + "</mn><mn>0.035</mn></mfrac></math>"
                        + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>0</mi><mi>f</mi><mo>&#xA0;</mo><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mi>B</mi><mi>a</mi><mi>g</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + Convert.ToDecimal(KGofCement * 50).ToString("0") + "</mn><mo>&#xA0;</mo><mi>k</mi><mi>g</mi></math></h5>";

                    #endregion Second Step Formula

                    #region Third Step Formula

                    ViewBag.lblStepIII = @"<h4><b>Step 3 :</h4></b>"
                        + @"<h4><b>Amount of Sand Required</h4></b><h5><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo></mo><mfrac><mi>Sand</mi><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Qauntity Of Mortar</mi></math>"
                        + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>a</mi><mi>n</mi><mi>d</mi><mo>&#xA0;</mo><mo>=</mo><mfrac><mn>" + RatioSand + "</mn><mn>" + (RatioCement + RatioSand) + "</mn></mfrac><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + Quantity25Wastage.ToString("0.0000") + "</mn></math>"
                        + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>a</mi><mi>n</mi><mi>d</mi><mo>&#xA0;</mo><mo>=</mo><mn>" + KGofSands.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                        + @"</h5><div class=""alert alert-block alert-success text-center"" style=""margin: 10px;"">By Considering dry loose bulk density of sand 1550 kg/m3</div>"
                        + @"<h5><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>a</mi><mi>n</mi><mi>d</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + KGofSands.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>1550</mn></math>"
                        + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi> Sand = " + KGofSandsTon.ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>k</mi><mi>g</mi></h5>";

                    #endregion Third Step Formula

                    #endregion Formula
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }

        }
        #endregion Function CalculateBlockValue

        #region Insert Log Function

        public void CalculatorLogInsert(ConcreteBlockCalculator concreteblock)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Block Calculator";

                if (concreteblock.UnitID != -1)
                    entLOG_Calculation.ParamA =Convert.ToString(concreteblock.UnitID);

                if (concreteblock.WallLengthA != null)
                {
                    if (concreteblock.WallLengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(concreteblock.WallLengthA + "." + concreteblock.WallLengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(concreteblock.WallLengthA);
                }

                if (concreteblock.WallDepthA != null)
                {
                    if (concreteblock.WallDepthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(concreteblock.WallDepthA + "." + concreteblock.WallDepthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(concreteblock.WallDepthA);
                }

                if (concreteblock.WallThicknessID != -1)
                {
                    if (concreteblock.WallThicknessID == 3 && concreteblock.OtherWallThickness != null)
                        entLOG_Calculation.ParamD = concreteblock.OtherWallThickness.ToString();
                    else
                        entLOG_Calculation.ParamD = Convert.ToString(concreteblock.WallThicknessID);
                }

                if (concreteblock.RatioID != -1)
                    entLOG_Calculation.ParamE = Convert.ToString(concreteblock.RatioID);

                if (concreteblock.UnitID == 1)
                {
                    if (ViewBag.lblAnswerBlockMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerBlockMeterAndCMValue;
                        entLOG_Calculation.ParamG = ViewBag.lblAnswerBlockFeetAndInchValue;
                    }
                }
                else if (concreteblock.UnitID == 2)
                {
                    if (ViewBag.lblAnswerBlockFeetAndInchValue != null)
                    {
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerBlockFeetAndInchValue;
                        entLOG_Calculation.ParamG = ViewBag.lblAnswerBlockMeterAndCMValue;
                    }
                }
                else
                {
                    if (ViewBag.lblAnswerBlockMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerBlockMeterAndCMValue;
                        entLOG_Calculation.ParamG = ViewBag.lblAnswerBlockFeetAndInchValue;
                    }
                }

                if (concreteblock.LengthBrick != null)
                    entLOG_Calculation.ParamH = Convert.ToString(concreteblock.LengthBrick);

                if (concreteblock.WidthBrick != null)
                    entLOG_Calculation.ParamI = Convert.ToString(concreteblock.WidthBrick);

                if (concreteblock.HeightBrick != null)
                    entLOG_Calculation.ParamJ = Convert.ToString(concreteblock.HeightBrick);

                entLOG_Calculation.ParamK = ViewBag.lblAmountBlocks + " Blocks";
                entLOG_Calculation.ParamL = ViewBag.lblAmountCement;
                entLOG_Calculation.ParamM = ViewBag.lblAmountSand;
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
