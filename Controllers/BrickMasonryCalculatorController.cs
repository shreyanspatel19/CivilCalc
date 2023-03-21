using CivilCalc.Models;
using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Controllers
{
    public class BrickMasonryCalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        #region Button Calculate click event
        protected void btnCalculateBrick_Click(BrickMasonryCalculator brickMasonry, EventArgs e)
        {
            //Validation();
            CalculateBrickValue(brickMasonry);
            //CalculatorLogInsert();
        }
        #endregion Button calculate click event
        #region Function CalculateBrickValue

        protected void CalculateBrickValue(BrickMasonryCalculator brickMasonry)
        {
            try
            {
        //        if (brickMasonry.txtWallLengthA != null && brickMasonry.txtWallDepthA != null)
        //        {
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

        //            #region Load DropDown Value


        //            #endregion Load DropDown value

                    BrickLength = Convert.ToDecimal(brickMasonry.txtLengthBrick);
                    BrickWidth = Convert.ToDecimal(brickMasonry.txtWidthBrick);
                    BrickHeight = Convert.ToDecimal(brickMasonry.txtHeightBrick);


        //                #region Length In Feet
        //                Decimal InputLengthInFeet = 0;
        //                Decimal InputLengthInInch = 0;
        //            if (brickMasonry.txtWallLengthA != null && brickMasonry.txtWallLengthA != null)
        //                    InputLengthInFeet = Convert.ToDecimal(brickMasonry.txtWallLengthA);
        //                else
        //                    InputLengthInFeet = 0;


        //                if (brickMasonry.txtWallLengthB != null && brickMasonry.txtWallLengthB != null)
        //                    InputLengthInInch = Convert.ToDecimal(brickMasonry.txtWallLengthB.);
        //                else
        //                    InputLengthInInch = 0;

        //                Decimal WallLenthInFeet = CommonFunctions.FeetAndInchToFeet(InputLengthInFeet, InputLengthInInch);

        //                WallLenthInMeter = CommonFunctions.FeetToMeter(WallLenthInFeet);

        //                #endregion Length In Feet

        //                #region Depth In Feet
        //                Decimal InputDepthInFeet = 0;
        //                Decimal InputDepthInInch = 0;
        //                if (txtWallDepthA.Text.Trim() != null && txtWallDepthA.Text.Trim() != String.Empty)
        //                    InputDepthInFeet = Convert.ToDecimal(txtWallDepthA.Text.Trim());
        //                else
        //                    InputDepthInFeet = 0;


        //                if (txtWallDepthB.Text.Trim() != null && txtWallDepthB.Text.Trim() != String.Empty)
        //                    InputDepthInInch = Convert.ToDecimal(txtWallDepthB.Text.Trim());
        //                else
        //                    InputDepthInInch = 0;

        //                Decimal WallDepthInFeet = CommonFunctions.FeetAndInchToFeet(InputDepthInFeet, InputDepthInInch);

        //                WallDepthInMeter = CommonFunctions.FeetToMeter(WallDepthInFeet);

        //                #endregion Depth In Feet
                    

        //            #region caluclation

        //            #region Calculate the Volume

        //            BrickValueForMeterAndCM = CommonFunctions.Volume(WallLenthInMeter, WallDepthInMeter, WallThicknessInMeter);
        //            BrickValueForFeetAndInch = CommonFunctions.ConvertFeetAndInchForVolume(BrickValueForMeterAndCM);
        //            lblAnswerBrickMeterAndCMValue.Text = BrickValueForMeterAndCM.ToString("0.00") + " m<sup>3</sup>";
        //            lblAnswerBrickFeetAndInchValue.Text = BrickValueForFeetAndInch.ToString("0.00") + " ft<sup>3</sup>";

        //            #endregion Calculate the Volume

        //            #region Convert Brick size in meter
        //            BrickLengthmeter = BrickLength / 100m;
        //            BrickWidthmeter = BrickWidth / 100m;
        //            BrickHeightmeter = BrickHeight / 100m;
        //            VolumeBrickWithoutMortar = (BrickLengthmeter * BrickWidthmeter * BrickHeightmeter);
        //            #endregion Convert Brick size in meter

        //            #region Volume of brick with mortar
        //            BrickLengthWithMortar = BrickLengthmeter + 0.01m;
        //            BrickWidthWithMortar = BrickWidthmeter + 0.01m;
        //            BrickHeightWithMortar = BrickHeightmeter + 0.01m;
        //            #endregion Volume of brick with mortar

        //            #region No of Bricks
        //            NoofBricks = BrickValueForMeterAndCM / (BrickLengthWithMortar * BrickWidthWithMortar * BrickHeightWithMortar);
        //            lblAmountBricks.Text = NoofBricks.ToString("0");
        //            lblAnswerTotalBricksValue.Text = NoofBricks.ToString("0");
        //            #endregion No of Bricks

        //            #region Volume of brick without mortar
        //            ActualVolumeBrickWithoutMortar = NoofBricks * VolumeBrickWithoutMortar;
        //            #endregion Volume of brick without mortar

        //            #region Quantity of Mortar
        //            QuantityMortar = (BrickValueForMeterAndCM) - ActualVolumeBrickWithoutMortar;

        //            #region Add 15% Mortar For Wastage
        //            Quantity15Wastage = QuantityMortar * 1.15m;
        //            #endregion Add 15% Mortar For Wastage

        //            #region Add 25%  For Dry Volume
        //            Quantity25Wastage = Quantity15Wastage * 1.25m;
        //            #endregion Add 25%  For Dry Volume

        //            #endregion Quantity of Mortar

        //            #region Amount of cement Require
        //            AmountCement = (RatioCement / (RatioSand + RatioCement)) * Quantity25Wastage;
        //            KGofCement = AmountCement / 0.035m;
        //            NoofCementBags = Math.Ceiling(KGofCement);
        //            lblAmountCement.Text = NoofCementBags == 1 ? NoofCementBags.ToString("0") + " Bag" : NoofCementBags.ToString("0") + " Bags";
        //            #endregion Amount of cement Require

        //            #region Amount of sand Require
        //            KGofSands = (RatioSand / (RatioCement + RatioSand)) * Quantity25Wastage;
        //            KGofSandsTon = KGofSands * Convert.ToDecimal(1500);
        //            AmountsandTon = (KGofSandsTon / 1000m);
        //            lblAmountSand.Text = AmountsandTon.ToString("0.00") + " ton";
        //            #endregion Amount of sand Require

        //            #endregion end caluclation

        //            #region Load Chart
        //            ChartCement = (KGofCement * 50);
        //            ChartSand = KGofSandsTon;
        //            ChartBrick = NoofBricks;
        //            ChartShow();
        //            #endregion Load Chart

        //            #region Formula For Meter/Cm

        //            #region First Step Formula

        //            lblStepI.Text = @"<h4><b>Step 1 :</h4></b>"
        //           + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Volume</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mi>L</mi><mi>e</mi><mi>n</mi><mi>g</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mi>D</mi><mi>e</mi><mi>p</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mi>W</mi><mi>a</mi><mi>l</mi><mi>l</mi><mo>&#xA0;</mo><mi>T</mi><mi>h</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>n</mi><mi>e</mi><mi>s</mi><mi>s</mi><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo></math>"
        //           + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + WallLenthInMeter.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + WallDepthInMeter.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + WallThicknessInMeter + "</mn></math>"
        //           + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickValueForMeterAndCM.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>";
        //            //+ @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickValueForFeetAndInch.ToString("0.00") + "</mn><mo>&#xA0;</mo><msup><mi>ft</mi><mn>3</mn></msup></math>";

        //            lblStepI.Text += @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickLength + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>cm</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidth + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>cm</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeight + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>cm</mi><mo>)</mo></math>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickLength + "</mn><mo>&#xA0;</mo><mo>/</mo><mo>&#xA0;</mo><mn></mn><mo></mo><mn>100</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickWidth + "</mn><mo>&#xA0;</mo><mo>/</mo><mo>&#xA0;</mo><mn></mn><mo></mo><mn>100</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickHeight + "</mn><mo>&#xA0;</mo><mo>/</mo><mo>&#xA0;</mo><mn></mn><mo></mo><mn>100</mn><mo>&#xA0;</mo><mo>)</mo></math>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickLengthmeter + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidthmeter + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeightmeter + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo></math>"
        //            + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>w</mi><mi>i</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickLengthmeter + "</mn><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>01</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickWidthmeter + "</mn><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>01</mn><mo>&#xA0;</mo><mo>)</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickHeightmeter + "</mn><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mn>0</mn><mo>.</mo><mn>01</mn><mo>&#xA0;</mo><mo>)</mo></math>"
        //            + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>i</mi><mi>z</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>w</mi><mi>i</mi><mi>t</mi><mi>h</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickLengthWithMortar + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidthWithMortar + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeightWithMortar + "</mn><mo>&#xA0;</mo><mo>(</mo><mi>m</mi><mo>)</mo></math>"
        //            + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mfrac><mrow><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>n</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>a</mi><mi>r</mi><mi>y</mi></mrow><mrow><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>o</mi><mi>n</mi><mi>e</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi></mrow></mfrac></math>"
        //            + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mfrac><mrow><mn>" + BrickValueForMeterAndCM.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></mrow><mrow><mn>" + BrickLengthWithMortar + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidthWithMortar + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeightWithMortar + "</mn></mrow></mfrac></math>"
        //            + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + NoofBricks.ToString("0") + "</mn><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi></math>"
        //            + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>W</mi><mi>i</mi><mi>t</mi><mi>h</mi><mi>o</mi><mi>u</mi><mi>t</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi></math>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + NoofBricks.ToString("0") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>(</mo><mo>&#xA0;</mo><mn>" + BrickLengthmeter.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickWidthmeter.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + BrickHeightmeter.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>)</mo></math>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + NoofBricks.ToString("0") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + VolumeBrickWithoutMortar.ToString("0.0000") + "</mn></math>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + ActualVolumeBrickWithoutMortar.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
        //            + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>&#xA0;</mo><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mo>&#xA0;</mo><mi>M</mi><mi>a</mi><mi>n</mi><mi>s</mi><mi>o</mi><mi>n</mi><mi>a</mi><mi>r</mi><mi>y</mi><mo>&#xA0;</mo><mo>-</mo><mo>&#xA0;</mo><mi>A</mi><mi>c</mi><mi>t</mi><mi>u</mi><mi>a</mi><mi>l</mi><mo>&#xA0;</mo><mi>V</mi><mi>o</mi><mi>l</mi><mi>u</mi><mi>m</mi><mi>e</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>B</mi><mi>r</mi><mi>i</mi><mi>c</mi><mi>k</mi><mi>s</mi><mo>&#xA0;</mo><mi>w</mi><mi>i</mi><mi>t</mi><mi>h</mi><mi>o</mi><mi>u</mi><mi>t</mi><mo>&#xA0;</mo><mi>m</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi></math>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>&#xA0;</mo><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + BrickValueForMeterAndCM.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>-</mo><mo>&#xA0;</mo><mn>" + ActualVolumeBrickWithoutMortar.ToString("0.0000") + "</mn></math>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>&#xA0;</mo><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + QuantityMortar.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
        //            + @"<div class=""alert alert-block alert-success text-center"" style=""margin: 10px;"">Add 15% more for wastage, Non - uniform thickness of mortar joins</div>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + QuantityMortar.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mfenced><mrow><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + QuantityMortar.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mfrac><mn>15</mn><mn>100</mn></mfrac><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo></mrow></mfenced></math>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + Quantity15Wastage.ToString("0.0000") + "</mn></math>"
        //            + @"<div class=""alert alert-block alert-success text-center"" style=""margin: 10px;"">Add 25% more for Dry Volume</div>"
        //            + @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + Quantity15Wastage.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>+</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mfenced><mrow><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + Quantity15Wastage.ToString("0.0000") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mfrac><mn>25</mn><mn>100</mn></mfrac><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo></mrow></mfenced></math>"
        //            + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Q</mi><mi>u</mi><mi>a</mi><mi>n</mi><mi>t</mi><mi>i</mi><mi>t</mi><mi>y</mi><mo>&#xA0;</mo><mi>o</mi><mi>f</mi><mo>&#xA0;</mo><mi>M</mi><mi>o</mi><mi>r</mi><mi>t</mi><mi>a</mi><mi>r</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + Quantity25Wastage.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>";

        //            #endregion First Step Formula

        //            #region Second Step Formula

        //            lblStepII.Text = @"<h4><b>Step 2 :</h4></b>"

        //                + @"<h4><b>Amount of Cement</h4></b><h5><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo></mo><mfrac><mi>Cement</mi><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Qauntity Of Mortar</mi></math>"
        //                + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mo>=</mo><mfrac><mn>1</mn><mrow><mn>" + (RatioSand + RatioCement) + "</mn><mo>&#xA0;</mo></mrow></mfrac><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mo>&#xA0;</mo><mn>" + Quantity25Wastage.ToString("0.0000") + "</mn></math>"
        //                + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mo>=</mo><mn>" + AmountCement.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
        //                + @"</h5><div class=""alert alert-block alert-success"" style=""margin: 10px;"">1 Bag of Cement  = 0.035 m<sup>3</sup></div>"
        //                + @"<h5><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>f</mi><mi>o</mi><mo>&#xA0;</mo><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mi>B</mi><mi>a</mi><mi>g</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mfrac><mn>" + AmountCement.ToString("0.0000") + "</mn><mn>0.035</mn></mfrac></math>"
        //                + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>N</mi><mi>o</mi><mo>&#xA0;</mo><mi>0</mi><mi>f</mi><mo>&#xA0;</mo><mi>C</mi><mi>e</mi><mi>m</mi><mi>e</mi><mi>n</mi><mi>t</mi><mo>&#xA0;</mo><mi>B</mi><mi>a</mi><mi>g</mi><mi>s</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + Convert.ToDecimal(KGofCement * 50).ToString("0") + "</mn><mo>&#xA0;</mo><mi>k</mi><mi>g</mi></math></h5>";

        //            #endregion Second Step Formula

        //            #region Third Step Formula

        //            lblStepIII.Text = @"<h4><b>Step 3 :</h4></b>"
        //                + @"<h4><b>Amount of Sand Required</h4></b><h5><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo></mo><mfrac><mi>Sand</mi><mrow><mi>Sum of Ratio</mi></mrow></mfrac><mo>&#xD7;</mo><mi>Qauntity Of Mortar</mi></math>"
        //                + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>a</mi><mi>n</mi><mi>d</mi><mo>&#xA0;</mo><mo>=</mo><mfrac><mn>" + RatioSand + "</mn><mn>" + (RatioCement + RatioSand) + "</mn></mfrac><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>" + Quantity25Wastage.ToString("0.0000") + "</mn></math>"
        //                + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>a</mi><mi>n</mi><mi>d</mi><mo>&#xA0;</mo><mo>=</mo><mn>" + KGofSands.ToString("0.0000") + "</mn><mo>&#xA0;</mo><msup><mi>m</mi><mn>3</mn></msup></math>"
        //                + @"</h5><div class=""alert alert-block alert-success text-center"" style=""margin: 10px;"">By Considering dry loose bulk density of sand 1500 kg/m3</div>"
        //                + @"<h5><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>S</mi><mi>a</mi><mi>n</mi><mi>d</mi><mo>&#xA0;</mo><mo>=</mo><mo>&#xA0;</mo><mn>" + KGofSands.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mn>1500</mn></math>"
        //                + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi> Sand = " + KGofSandsTon.ToString("0.00") + "</mi><mo>&#xA0;</mo><mi>k</mi><mi>g</mi></h5>";

        //            #endregion Third Step Formula

        //            #endregion Formula For Meter/Cm
        //        }
            }
            catch (Exception e)
            {
                
            }

        }
        #endregion Function CalculateBrickValue


    }
}
