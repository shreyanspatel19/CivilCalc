using CivilCalc.DAL.LOG.LOG_Calculation;
using CivilCalc.Models;
using CivilEngineeringCalculators;
using Microsoft.AspNetCore.Mvc;
using CivilCalc.DAL;
using CivilCalc.Areas.LOG_Calculation.Models;
using System.Collections.Generic;
using CivilCalc.DAL.CAL.CAL_Calculator;
using System.Data;
using AutoMapper;
using CivilCalc.Areas.MST_Configuration.Models;
using CivilCalc.Areas.CAL_Calculator.Models;
using SelectForSearch_Result = CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result;

namespace CivilCalc.Controllers
{
    public class BrickCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Brick-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Brick-Calculator");


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

            return View("BrickCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(BrickCalculator brickMasonry)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Brick-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            CalculateBrickValue(brickMasonry);
            CalculatorLogInsert(brickMasonry);
            return PartialView("_BrickCalculatorResult", vModel);
        }
        #endregion

        #region Insert Log Function
        public void CalculatorLogInsert(BrickCalculator brickMasonry)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Brick Calculator";

                if (brickMasonry.UnitID == 1)
                    entLOG_Calculation.ParamA = "Meter/CM";
                else
                    entLOG_Calculation.ParamA = "Feet/Inch";

                if (brickMasonry.txtWallLengthA != null)
                {
                    if (brickMasonry.txtWallLengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(brickMasonry.txtWallLengthA + "." + brickMasonry.txtWallLengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(brickMasonry.txtWallLengthA);
                }

                if (brickMasonry.txtWallDepthA != null)
                {
                    if (brickMasonry.txtWallDepthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(brickMasonry.txtWallDepthA + "." + brickMasonry.txtWallDepthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(brickMasonry.txtWallDepthA);
                }

                if (brickMasonry.WallThicknessID != -1)
                {
                    if (brickMasonry.WallThicknessID == 3 && brickMasonry.txtOtherWallThickness != null)
                        entLOG_Calculation.ParamD = ""+brickMasonry.txtOtherWallThickness;
                    else
                        entLOG_Calculation.ParamD =""+ (Convert.ToDouble(brickMasonry.WallThicknessID)*100)+" "+ "CM Wall";
                }

                if (Convert.ToDouble(brickMasonry.RatioID) > 0)
                    entLOG_Calculation.ParamE = "C.M 1:" + brickMasonry.RatioID;

                if (brickMasonry.UnitID == 1)
                {
                    if (ViewBag.lblAnswerBrickMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerBrickMeterAndCMValue;
                        entLOG_Calculation.ParamG = ViewBag.lblAnswerBrickFeetAndInchValue;
                    }
                }
                else
                {
                    if (ViewBag.lblAnswerBrickFeetAndInchValue != null)
                    {
                        entLOG_Calculation.ParamF = ViewBag.lblAnswerBrickFeetAndInchValue;
                        entLOG_Calculation.ParamG = ViewBag.lblAnswerBrickMeterAndCMValue;
                    }
                }

                if (brickMasonry.txtLengthBrick != null)
                    entLOG_Calculation.ParamH = ""+brickMasonry.txtLengthBrick;

                if (brickMasonry.txtWidthBrick != null)
                    entLOG_Calculation.ParamI = "" + brickMasonry.txtWidthBrick;

                if (brickMasonry.txtHeightBrick != null)
                    entLOG_Calculation.ParamJ = "" + brickMasonry.txtHeightBrick;

                entLOG_Calculation.ParamK = ViewBag.lblAmountBricks + " Bricks";
                entLOG_Calculation.ParamL = ViewBag.lblAmountCement;
                entLOG_Calculation.ParamM = ViewBag.lblAmountSand;
               

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
        #endregion

        #region Calculate Brick Value
        public void CalculateBrickValue(BrickCalculator brickMasonry)
        {
            try
            {
                if (brickMasonry.txtWallLengthA != null)
                {
                    #region variable
                    Decimal WallLenthInMeter = 0, WallDepthInMeter = 0;
                    Decimal WallThicknessInMeter = 0;
                    Decimal BrickLength = 0, BrickWidth = 0, BrickHeight = 0;
                    Decimal BrickLengthmeter = 0, BrickWidthmeter = 0, BrickHeightmeter = 0;
                    Decimal BrickLengthWithMortar = 0, BrickWidthWithMortar = 0, BrickHeightWithMortar = 0;
                    Decimal VolumeBrickWithoutMortar = 0;
                    Decimal ActualVolumeBrickWithoutMortar = 0;
                    Decimal QuantityMortar = 0, Quantity15Wastage = 0, Quantity25Wastage = 0;
                    Decimal RatioCement = 0, RatioSand = 0;
                    Decimal AmountCement = 0, AmountsandTon = 0;
                    Decimal BrickValueForMeterAndCM = 0;
                    Decimal BrickValueForFeetAndInch = 0;
                    Decimal NoofBricks = 0;
                    Decimal KGofSands = 0, KGofSandsTon = 0;
                    Decimal NoofCementBags = 0, KGofCement = 0;
                    #endregion variable

                    #region Load DropDown Value

                    #region Get ddlWall Thickness value
                    if (brickMasonry.WallThicknessID == 3)
                        WallThicknessInMeter = Convert.ToDecimal(brickMasonry.txtOtherWallThickness) / 100m;
                    else
                        WallThicknessInMeter = Convert.ToDecimal(brickMasonry.WallThicknessID);
                    #endregion Get ddlWall Thickness value

                    #region Get RatioID Value
                    RatioCement = 1;
                    RatioSand = Convert.ToDecimal(brickMasonry.RatioID);
                    #endregion Get RatioID Value

                    #endregion Load DropDown value

                    BrickLength = Convert.ToDecimal(brickMasonry.txtLengthBrick);
                    BrickWidth = Convert.ToDecimal(brickMasonry.txtWidthBrick);
                    BrickHeight = Convert.ToDecimal(brickMasonry.txtHeightBrick);


                    #region Calculate Wall Length & Depth
                    if (brickMasonry.UnitID == 1)
                    {
                        #region Length In Meter
                        Decimal InputLengthInMeter = 0;
                        Decimal InputLengthInCM = 0;
                        if (brickMasonry.txtWallLengthA != null)
                            InputLengthInMeter = Convert.ToDecimal(brickMasonry.txtWallLengthA);
                        else
                            InputLengthInMeter = 0;


                        if (brickMasonry.txtWallLengthB != null)
                            InputLengthInCM = Convert.ToDecimal(brickMasonry.txtWallLengthB);
                        else
                            InputLengthInCM = 0;

                        WallLenthInMeter = CommonFunctions.MeterAndCMToMeter(InputLengthInMeter, InputLengthInCM);

                        #endregion Length In Meter

                        #region Depth In Meter
                        Decimal InputDepthInMeter = 0;
                        Decimal InputDepthInCM = 0;
                        if (brickMasonry.txtWallDepthA != null)
                            InputDepthInMeter = Convert.ToDecimal(brickMasonry.txtWallDepthA);
                        else
                            InputDepthInMeter = 0;


                        if (brickMasonry.txtWallDepthB != null)
                            InputDepthInCM = Convert.ToDecimal(brickMasonry.txtWallDepthB);
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
                        if (brickMasonry.txtWallLengthA != null)
                            InputLengthInFeet = Convert.ToDecimal(brickMasonry.txtWallLengthA);
                        else
                            InputLengthInFeet = 0;


                        if (brickMasonry.txtWallLengthB != null)
                            InputLengthInInch = Convert.ToDecimal(brickMasonry.txtWallLengthB);
                        else
                            InputLengthInInch = 0;

                        Decimal WallLenthInFeet = CommonFunctions.FeetAndInchToFeet(InputLengthInFeet, InputLengthInInch);

                        WallLenthInMeter = CommonFunctions.FeetToMeter(WallLenthInFeet);

                        #endregion Length In Feet

                        #region Depth In Feet
                        Decimal InputDepthInFeet = 0;
                        Decimal InputDepthInInch = 0;
                        if (brickMasonry.txtWallDepthA != null)
                            InputDepthInFeet = Convert.ToDecimal(brickMasonry.txtWallDepthA);
                        else
                            InputDepthInFeet = 0;


                        if (brickMasonry.txtWallDepthB != null)
                            InputDepthInInch = Convert.ToDecimal(brickMasonry.txtWallDepthB);
                        else
                            InputDepthInInch = 0;

                        Decimal WallDepthInFeet = CommonFunctions.FeetAndInchToFeet(InputDepthInFeet, InputDepthInInch);

                        WallDepthInMeter = CommonFunctions.FeetToMeter(WallDepthInFeet);

                        #endregion Depth In Feet
                    }

                    #endregion Calculate Wall Length & Depth

                    #region caluclation

                    #region Calculate the Volume

                    BrickValueForMeterAndCM = CommonFunctions.Volume(WallLenthInMeter, WallDepthInMeter, WallThicknessInMeter);
                    BrickValueForFeetAndInch = CommonFunctions.ConvertFeetAndInchForVolume(BrickValueForMeterAndCM);
                    ViewBag.lblAnswerBrickMeterAndCMValue = BrickValueForMeterAndCM.ToString("0.00") + " m<sup>3</sup>";
                    ViewBag.lblAnswerBrickFeetAndInchValue = BrickValueForFeetAndInch.ToString("0.00") + " ft<sup>3</sup>";

                    #endregion Calculate the Volume

                    #region Convert Brick size in meter
                    BrickLengthmeter = BrickLength / 100m;
                    BrickWidthmeter = BrickWidth / 100m;
                    BrickHeightmeter = BrickHeight / 100m;
                    VolumeBrickWithoutMortar = (BrickLengthmeter * BrickWidthmeter * BrickHeightmeter);
                    #endregion Convert Brick size in meter

                    #region Volume of brick with mortar
                    BrickLengthWithMortar = BrickLengthmeter + 0.01m;
                    BrickWidthWithMortar = BrickWidthmeter + 0.01m;
                    BrickHeightWithMortar = BrickHeightmeter + 0.01m;
                    #endregion Volume of brick with mortar

                    #region No of Bricks
                    NoofBricks = BrickValueForMeterAndCM / (BrickLengthWithMortar * BrickWidthWithMortar * BrickHeightWithMortar);
                    ViewBag.lblAmountBricks = NoofBricks.ToString("0");
                    ViewBag.lblAnswerTotalBricksValue = NoofBricks.ToString("0");
                    #endregion No of Bricks

                    #region Volume of brick without mortar
                    ActualVolumeBrickWithoutMortar = NoofBricks * VolumeBrickWithoutMortar;
                    #endregion Volume of brick without mortar

                    #region Quantity of Mortar
                    QuantityMortar = (BrickValueForMeterAndCM) - ActualVolumeBrickWithoutMortar;

                    #region Add 15% Mortar For Wastage
                    Quantity15Wastage = QuantityMortar * 1.15m;
                    #endregion Add 15% Mortar For Wastage

                    #region Add 25%  For Dry Volume
                    Quantity25Wastage = Quantity15Wastage * 1.25m;
                    #endregion Add 25%  For Dry Volume

                    #endregion Quantity of Mortar

                    #region Amount of cement Require
                    AmountCement = (RatioCement / (RatioSand + RatioCement)) * Quantity25Wastage;
                    KGofCement = AmountCement / 0.035m;
                    NoofCementBags = Math.Ceiling(KGofCement);
                    ViewBag.lblAmountCement = NoofCementBags == 1 ? NoofCementBags.ToString("0") + " Bag" : NoofCementBags.ToString("0") + " Bags";
                    #endregion Amount of cement Require

                    #region Amount of sand Require
                    KGofSands = (RatioSand / (RatioCement + RatioSand)) * Quantity25Wastage;
                    KGofSandsTon = KGofSands * Convert.ToDecimal(1500);
                    AmountsandTon = (KGofSandsTon / 1000m);
                    ViewBag.lblAmountSand = AmountsandTon.ToString("0.00") + " ton";
                    #endregion Amount of sand Require

                    #endregion end caluclation

                    #region Load Chart
                    ViewBag.ChartCement = Math.Round((KGofCement * 50),2);
                    ViewBag.ChartSand = Math.Round(KGofSandsTon,2);
                    ViewBag.ChartBrick = Math.Round(NoofBricks,2);
                    #endregion Load Chart

                    #region Formula For Meter/Cm

                    #region First Step Formula

                    ViewBag.lblStepI = @"<h4><b>Step 1 :</h4></b>"
                   + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Volume</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mi>L</mi><mi>e</mi><mi>n</mi><mi>g</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mi>D</mi><mi>e</mi><mi>p</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mi>W</mi><mi>a</mi><mi>l</mi><mi>l</mi><mo>&#xA0;</mo><mi>T</mi><mi>h</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>n</mi><mi>e</mi><mi>s</mi><mi>s</mi><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo></math>"
                   + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + WallLenthInMeter.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + WallDepthInMeter.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + WallThicknessInMeter + "</mn></math>"
                   + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickValueForMeterAndCM.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>";
                    //+ @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickValueForFeetAndInch.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>ft</mi><mn>3</mn></msup></math>";

                    ViewBag.lblStepI += @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickLength + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>cm</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidth + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>cm</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeight + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>cm</mi><mo>)</mo></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickLength + "</mn><mo>&#xA0;</mo><mo>/</mo><mo>&#xA0;</mo><mn></mn><mo></mo><mn>100</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickWidth + "</mn><mo>&#xA0;</mo><mo>/</mo><mo>&#xA0;</mo><mn></mn><mo></mo><mn>100</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickHeight + "</mn><mo>&#xA0;</mo><mo>/</mo><mo>&#xA0;</mo><mn></mn><mo></mo><mn>100</mn><mo>&#xA0;</mo><mo>)</mo></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickLengthmeter + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidthmeter + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeightmeter + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>w</mi><mi>i</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickLengthmeter + "</mn><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>01</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickWidthmeter + "</mn><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>01</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickHeightmeter + "</mn><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>01</mn><mo>&#xA0;</mo><mo>)</mo></math>"
                    + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>w</mi><mi>i</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickLengthWithMortar + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidthWithMortar + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeightWithMortar + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mfrac><mrow><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>n</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>a</mi><mi>r</mi><mi>y</mi></mrow><mrow><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>o</mi><mi>n</mi><mi>e</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi></mrow></mfrac></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mfrac><mrow><mn>" + BrickValueForMeterAndCM.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></mrow><mrow><mn>" + BrickLengthWithMortar + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidthWithMortar + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeightWithMortar + "</mn></mrow></mfrac></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + NoofBricks.ToString("0") + "</mn><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>W</mi><mi>i</mi><mi>t</mi><mi>h</mi><mi>o</mi><mi>u</mi><mi>t</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + NoofBricks.ToString("0") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickLengthmeter.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidthmeter.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeightmeter.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>)</mo></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + NoofBricks.ToString("0") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + VolumeBrickWithoutMortar.ToString("0.0000") + "</mn></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + ActualVolumeBrickWithoutMortar.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
                    + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>&#xA0;</mo><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>n</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>a</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>-</mo><mo>&#xA0;</mo><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>w</mi><mi>i</mi><mi>t</mi><mi>h</mi><mi>o</mi><mi>u</mi><mi>t</mi><mo>&#xA0;</mo><mi>m</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi></math>"
                    + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>&#xA0;</mo><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickValueForMeterAndCM.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>-</mo><mo>&#xA0;</mo><mn>" + ActualVolumeBrickWithoutMortar.ToString("0.0000") + "</mn></math>"
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
                        + @"</h5><div class=""alert alert-block alert-success text-center"" style=""margin: 10px;"">By Considering dry loose bulk density of sand 1500 kg/m3</div>"
                        + @"<h5><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>a</mi><mi>n</mi><mi>d</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + KGofSands.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>1500</mn></math>"
                        + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi> Sand = " + KGofSandsTon.ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>k</mi><mi>g</mi></h5>";

                    #endregion Third Step Formula

                    #endregion Formula For Meter/Cm
                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
    }
}
