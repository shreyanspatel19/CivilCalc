using CivilCalc.Areas.LOG_Calculation.Models;
using CivilCalc.DAL;
using CivilCalc.DAL.LOG.LOG_Calculation;
using CivilCalc.Models;
using CivilEngineeringCalculators;
using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Controllers
{
    public class AirConditionerSizeCalculatorController : Controller
    {
        [Route("Quantity-Estimator/Air-Conditioner-Size-Calculator")]

        #region Index
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> Calculator = DBConfig.dbCALCalculator.SelectURLName("/Quantity-Estimator/Air-Conditioner-Size-Calculator");

            ViewBag.Title = Calculator[0].MetaTitle;
            ViewBag.HeaderName = Calculator[0].HeaderName;
            ViewBag.SubHeaderName = Calculator[0].SubHeaderName;
            ViewBag.CalculatorName = Calculator[0].CalculatorName;
            ViewBag.CategoryName = Calculator[0].CategoryName;
            ViewBag.MetaKeyword = Calculator[0].MetaKeyword;
            ViewBag.MetaDescription = Calculator[0].MetaDescription;
            ViewBag.CalculatorIcon = Calculator[0].CalculatorIcon;
            return View();
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(AirConditionerSizeCalculator aircalculator)
        {
            CalculateAircondtionerSize(aircalculator);
            CalculatorLogInsert(aircalculator);
            return PartialView("_CalculationDetails");
        }
        #endregion

        #region Insert Log Function
        public void CalculatorLogInsert(AirConditionerSizeCalculator aircalculator)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Air-Conditioner-Size-Calculator";

                if (aircalculator.txtLengthA != null)
                {
                    if (aircalculator.txtLengthB != null)
                        entLOG_Calculation.ParamA = Convert.ToString(aircalculator.txtLengthA + "." + aircalculator.txtLengthB);
                    else
                        entLOG_Calculation.ParamA = Convert.ToString(aircalculator.txtLengthA);
                }

                if (aircalculator.txtBreadthA != null)
                {
                    if (aircalculator.txtBreadthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(aircalculator.txtBreadthA + "." + aircalculator.txtBreadthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(aircalculator.txtBreadthA);
                }

                if (aircalculator.txtHeightA != null)
                {
                    if (aircalculator.txtHeightB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(aircalculator.txtHeightA + "." + aircalculator.txtHeightB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(aircalculator.txtHeightA);
                }

                if (aircalculator.txtNoofPerson != null)
                {
                    entLOG_Calculation.ParamD = Convert.ToString(aircalculator.txtNoofPerson);
                }
                else
                {
                    entLOG_Calculation.ParamD = Convert.ToString("0");
                }

                if (aircalculator.txttemperature != null)
                {
                    entLOG_Calculation.ParamE = Convert.ToString(aircalculator.txttemperature);
                }
                else
                {
                    entLOG_Calculation.ParamE = Convert.ToString("0");
                }

                entLOG_Calculation.ParamF = ViewBag.lblSizeOfAc;

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

        #region Function Calculate Ac Size Value 
        public void CalculateAircondtionerSize(AirConditionerSizeCalculator aircalculator)
        {
            try
            {
                #region Calculation

                if (aircalculator.txtNoofPerson == null && aircalculator.txttemperature == null && aircalculator.txtHeightA == null)
                {
                   // pnlArea.Visible = true;
                  //  pnlVolume.Visible = false;
                  // pnlTotal.Visible = false;
                    CalculateAcSize(Convert.ToDecimal(aircalculator.txtLengthA + "." + ConvertInchtoFeet(aircalculator.txtLengthB)), Convert.ToDecimal(aircalculator.txtBreadthA + "." + ConvertInchtoFeet(aircalculator.txtBreadthB)));
                }
                else if (aircalculator.txtNoofPerson == null && aircalculator.txttemperature == null)
                {
                    //pnlArea.Visible = false;
                    //pnlVolume.Visible = true;
                    //pnlTotal.Visible = false;
                    CalculateAcSize(Convert.ToDecimal(aircalculator.txtLengthA + "." + ConvertInchtoFeet(aircalculator.txtLengthB)), Convert.ToDecimal(aircalculator.txtBreadthA + "." + ConvertInchtoFeet(aircalculator.txtBreadthB)), Convert.ToDecimal(aircalculator.txtHeightA + "." + ConvertInchtoFeet(aircalculator.txtHeightB)));
                }
                else
                {
                    //pnlArea.Visible = false;
                    //pnlVolume.Visible = false;
                    //pnlTotal.Visible = true;
                    decimal NoofPerson = (aircalculator.txtNoofPerson == null) ? 0 : Convert.ToDecimal(aircalculator.txtNoofPerson);
                    decimal Temperature = (aircalculator.txttemperature == null) ? 0 : Convert.ToDecimal(aircalculator.txttemperature);
                    CalculateAcSize(Convert.ToDecimal(aircalculator.txtLengthA + "." + ConvertInchtoFeet(aircalculator.txtLengthB)), Convert.ToDecimal(aircalculator.txtBreadthA + "." + ConvertInchtoFeet(aircalculator.txtBreadthB)), Convert.ToDecimal(aircalculator.txtHeightA + "." + ConvertInchtoFeet(aircalculator.txtHeightB)), NoofPerson, Temperature);
                }

                #endregion Calculation


            }
            catch (Exception e)
            {

            }
        }
        #endregion Function Calculate Ac Size Value

        #region Function Calculate Ac Size Area 
        public void CalculateAcSize(decimal length, decimal breadth)
        {

            decimal Area = CommonFunctions.Area(length, breadth);
            decimal AreaAnswer = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(Area))) / 10;
            ViewBag.lblSizeOfAc = AreaAnswer.ToString("0.00");
            #region Formula For Meter/CM

            ViewBag.lblFormulaAcSize = @"Air Conditioner Tons = <math xmlns='http://www.w3.org/1998/Math/MathML'><mfrac><mrow><msqrt><msup><mi>Length</mi></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>Breadth</mi><mo></mo></mrow></msup></msqrt></mrow><mi>10</mi></mfrac></math>"
                                      + @"<br /><br />Air Conditioner Tons =  <math xmlns='http://www.w3.org/1998/Math/MathML'><mfrac><mrow><msqrt><msup><mi>" + length.ToString("0.00") + "</mi></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>" + breadth.ToString("0.00") + "</mi><mo></mo></mrow></msup></msqrt></mrow><mi>10</mi></mfrac></math>"
                                      + @"<br /><br />Air Conditioner Tons =  <math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mi>" + AreaAnswer.ToString("0.00") + " Tons</mi></math>";


            #endregion Formula For Meter/CM
        }
        #endregion Function Calculate Ac Size Area

        #region Function Calculate Ac Size Volume 
        public void CalculateAcSize(decimal length, decimal breadth, decimal height)
        {
            decimal Volume = CommonFunctions.Volume(length, breadth , height);
            //decimal VolumeAnswer = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(Volume))) / 10;
            decimal VolumeAnswer = Convert.ToDecimal(Convert.ToDouble(Volume)) / 1000;
           ViewBag.lblSizeOfAc = VolumeAnswer.ToString("0.00");
            #region Formula For Meter/CM
            //lblFormulaAcSize.Text = @"Air Conditioner Tons = <math xmlns='http://www.w3.org/1998/Math/MathML'><mfrac><mrow><msqrt><msup><mi>Length</mi></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>Breadth</mi><mo></mo></mrow></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>Height</mi><mo></mo></mrow></msup></msqrt></mrow><mi>10</mi></mfrac></math>"
           ViewBag.lblFormulaAcSize = @"Air Conditioner Tons = <math xmlns='http://www.w3.org/1998/Math/MathML'><mfrac><mrow><msup><mi>Length</mi></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>Breadth</mi><mo></mo></mrow></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>Height</mi><mo></mo></mrow></msup></mrow><mi>1000</mi></mfrac></math>"
                                      + @"<br /><br />Air Conditioner Tons =  <math xmlns='http://www.w3.org/1998/Math/MathML'><mfrac><mrow><msup><mi>" + length.ToString("0.00") + "</mi></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>" + breadth.ToString("0.00") + "</mi><mo></mo></mrow></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>" + height.ToString("0.00") + "</mi><mo></mo></mrow></msup></mrow><mi>1000</mi></mfrac></math>"
                                      + @"<br /><br />Air Conditioner Tons =  <math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mi>" + VolumeAnswer.ToString("0.00") + " Tons</mi></math>";
            #endregion Formula For Meter/CM
        }
        #endregion Function Calculate Ac Size Volume

        #region Function Calculate Ac Size 
        protected void CalculateAcSize(decimal length, decimal breadth, decimal height, decimal person, decimal temperature)
        {
            decimal Area = CommonFunctions.Area(length, breadth);
            decimal AreaAnswer = (Area * 20) / 12000;
            decimal Extraperson = (person > 3) ? (person - 3) : 0;
            decimal NoofPerson = 0.3m + (0.07m * Extraperson);
            decimal roomTemperature = 0.2m;
            if (temperature > 45)
            {
                roomTemperature = 0.5m;
            }
            if (temperature > 40 && temperature <= 45)
            {
                roomTemperature = 0.4m;
            }
            if (temperature > 35 && temperature <= 40)
            {
                roomTemperature = 0.3m;
            }
            decimal roomHeight = (height <= 8) ? 0 : (height - 8) * 0.1m;
            decimal AcTonsAnswer = AreaAnswer + NoofPerson + roomTemperature + roomHeight;
           ViewBag.lblSizeOfAc = AcTonsAnswer.ToString("0.00");

            #region Formula For Meter/CM
           ViewBag.lblFormulaAcSize = @"AC Tons = <math xmlns='http://www.w3.org/1998/Math/MathML'><mfenced><mfrac><mrow><msup><mi>Length</mi></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>Breadth</mi><mo></mo></mrow></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>20</mi><mo></mo></mrow></msup></mrow><mi>12000</mi></mfrac></mfenced><mo>+</mo><mrow><mi>No of Person</mi><mo>+</mo><mi>Temperature</mi><mo>+</mo><mi>Height</mi></mrow></math>"
                                      + @"<br /><br />AC Tons =  <math xmlns='http://www.w3.org/1998/Math/MathML'><mfenced><mfrac><mrow><msup><mi>" + length.ToString("0.00") + "</mi></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>" + breadth.ToString("0.00") + "</mi><mo></mo></mrow></msup><mo>&#xD7;</mo><msup><mrow><mo></mo><mi>20</mi><mo></mo></mrow></msup></mrow><mi>12000</mi></mfrac></mfenced><mo>+</mo><mrow><mi>" + NoofPerson.ToString("0.00") + "</mi><mo>+</mo><mi>" + roomTemperature.ToString("0.00") + "</mi><mo>+</mo><mi>" + roomHeight.ToString("0.00") + "</mi></mrow></math>"
                                      + @"<br /><br />AC Tons =  <math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mi>" + AcTonsAnswer.ToString("0.00") + " Tons</mi></math>";
            #endregion Formula For Meter/CM
        }
        #endregion Function Calculate Ac Size


        #region convert inch to feet
        public Decimal ConvertInchtoFeet(int? inch)
        {
            if (inch != 0)
            {
                Decimal feet = (Convert.ToDecimal(inch) * 100) / 12;
                return feet;
            }
            return 0;
        }
        #endregion

    }
}
