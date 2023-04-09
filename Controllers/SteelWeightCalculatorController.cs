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
    public class SteelWeightCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Steel-Weight-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Steel-Weight-Calculator");


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

            return View("SteelWeightCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(SteelWeightCalculator steelweight)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Steel-Weight-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (steelweight.UnitID == 1)
                CalculateValueForMeterAndCM(steelweight);
            else
                CalculateValueForFeetAndInch(steelweight);

            CalculatorLogInsert(steelweight);

            return PartialView("_SteelWeightCalculatorResult", vModel);
        }
        #endregion

        #region Function: Calculate Value For Meter And CM

        protected void CalculateValueForMeterAndCM(SteelWeightCalculator steelweight)
        {
            try
            {
                if (steelweight.Diameter != null && steelweight.LengthA != null && steelweight.Quantity != null)
                {
                    #region Variables

                    Decimal Temp = 0m, SteelWeightInKg = 0m, SteelWeightInTon = 0m;

                    #endregion Variables

                    #region Calculation

                    Temp = (Convert.ToDecimal(steelweight.Diameter) * Convert.ToDecimal(steelweight.Diameter)) / Convert.ToDecimal(162.28);

                    if (steelweight.LengthB != null)
                        SteelWeightInKg = Temp * Convert.ToDecimal(steelweight.LengthA + "." + steelweight.LengthB) * Convert.ToDecimal(steelweight.Quantity);
                    else
                        SteelWeightInKg = Temp * Convert.ToDecimal(steelweight.LengthA) * Convert.ToDecimal(steelweight.Quantity);

                    SteelWeightInTon = SteelWeightInKg / 1000;
                    ViewBag.lblKgAnswer = SteelWeightInKg.ToString("0.0000") + " kg.";
                    ViewBag.lblTonAnswer = SteelWeightInTon.ToString("0.0000") + " Ton";

                    #endregion Calculation

                    #region Formula

                    if (steelweight.LengthB != null)
                    {
                        ViewBag.lblSteelWeightFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><msup><mi>D</mi><mn>2</mn></msup><mn>162.28</mn></mfrac><mo>&#xD7;</mo><mi>Length</mi><mo>&#xD7;</mo><mi>Quantity</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><msup><mi>" + steelweight.Diameter + "</mi><mn>2</mn></msup><mn>162.28</mn></mfrac><mo>&#xD7;</mo><mi>" + steelweight.LengthA + "." + steelweight.LengthB + "</mi><mo>&#xD7;</mo><mi>" + steelweight.Quantity + "</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SteelWeightInKg.ToString("0.0000") + " kg.</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>" + SteelWeightInKg.ToString("0.0000") + "</mi><mn>1000</mn></mfrac></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SteelWeightInTon.ToString("0.0000") + " Ton</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblSteelWeightFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><msup><mi>D</mi><mn>2</mn></msup><mn>162.28</mn></mfrac><mo>&#xD7;</mo><mi>Length</mi><mo>&#xD7;</mo><mi>Quantity</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><msup><mi>" + steelweight.Diameter + "</mi><mn>2</mn></msup><mn>162.28</mn></mfrac><mo>&#xD7;</mo><mi>" + steelweight.LengthA + "</mi><mo>&#xD7;</mo><mi>" + steelweight.Quantity + "</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SteelWeightInKg.ToString("0.0000") + " kg.</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>" + SteelWeightInKg.ToString("0.0000") + "</mi><mn>1000</mn></mfrac></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SteelWeightInTon.ToString("0.0000") + " Ton</mi></math>";
                    }

                    #endregion Formula
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function: Calculate Value For Meter And CM

        #region Function: Calculate Value For Feet And Inch

        protected void CalculateValueForFeetAndInch(SteelWeightCalculator steelweight)
        {
            try
            {
                if (steelweight.Diameter != null && steelweight.LengthA != null && steelweight.Quantity != null)
                {
                    #region Variables

                    Decimal Temp = 0m, SteelWeightInKg = 0m, SteelWeightInTon = 0m, LengthInMeter = 0m;

                    #endregion Variables

                    #region Calculation

                    Temp = (Convert.ToDecimal(steelweight.Diameter) * Convert.ToDecimal(steelweight.Diameter)) / Convert.ToDecimal(162.28);

                    if (steelweight.LengthB != null)
                        LengthInMeter = Convert.ToDecimal(steelweight.LengthA + "." + steelweight.LengthB) / Convert.ToDecimal(3.28084);
                    else
                        LengthInMeter = Convert.ToDecimal(steelweight.LengthA) / Convert.ToDecimal(3.28084);

                    SteelWeightInKg = Temp * LengthInMeter * Convert.ToDecimal(steelweight.Quantity);

                    SteelWeightInTon = SteelWeightInKg / 1000;
                    ViewBag.lblKgAnswer = SteelWeightInKg.ToString("0.0000") + " kg.";
                    ViewBag.lblTonAnswer = SteelWeightInTon.ToString("0.0000") + " Ton";

                    #endregion Calculation

                    #region Formula

                    ViewBag.lblSteelWeightFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><msup><mi>D</mi><mn>2</mn></msup><mn>162.28</mn></mfrac><mo>&#xD7;</mo><mi>Length (in meter)</mi><mo>&#xD7;</mo><mi>Quantity</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><msup><mi>" + steelweight.Diameter + "</mi><mn>2</mn></msup><mn>162.28</mn></mfrac><mo>&#xD7;</mo><mi>" + LengthInMeter.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>" + steelweight.Quantity + "</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SteelWeightInKg.ToString("0.0000") + " kg.</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>" + SteelWeightInKg.ToString("0.0000") + "</mi><mn>1000</mn></mfrac></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + SteelWeightInTon.ToString("0.0000") + " Ton</mi></math>";
                    #endregion Formula
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function: Calculate Value For Feet And Inch

        #region Insert Log Function

        public void CalculatorLogInsert(SteelWeightCalculator steelweight)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Steel-Weight-Calculator";

                if (steelweight.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(steelweight.UnitID);

                if (steelweight.Diameter != null)
                    entLOG_Calculation.ParamB = Convert.ToString(steelweight.Diameter);

                if (steelweight.LengthA != null)
                {
                    if (steelweight.LengthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(steelweight.LengthA + "." + steelweight.LengthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(steelweight.LengthA);
                }

                if (steelweight.Quantity != null)
                    entLOG_Calculation.ParamD = Convert.ToString(steelweight.Quantity);

                if (ViewBag.lblKgAnswer != null)
                    entLOG_Calculation.ParamE = ViewBag.lblKgAnswer;

                if (ViewBag.lblTonAnswer != null)
                    entLOG_Calculation.ParamF = ViewBag.lblTonAnswer;

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
