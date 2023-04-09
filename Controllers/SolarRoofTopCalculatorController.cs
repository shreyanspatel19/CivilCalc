using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.Areas.LOG_Calculation.Models;
using CivilCalc.DAL;
using CivilCalc.DAL.LOG.LOG_Calculation;
using CivilCalc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using SelectForSearch_Result = CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result;

namespace CivilCalc.Controllers
{
    public class SolarRoofTopCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Solar-RoofTop-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Solar-RoofTop-Calculator");


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

            return View("SolarRoofTopCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(SolarRoofTopCalculator solarrooftop)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Solar-RoofTop-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            CalculateSolarRooftopValue(solarrooftop);
            CalculatorLogInsert(solarrooftop);

            return PartialView("_SolarRoofTopCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Cement Concrete Value For Meter And CM

        protected void CalculateSolarRooftopValue(SolarRoofTopCalculator solarrooftop)
        {
            try
            {
                if (solarrooftop.Units != null)
                {
                    #region Calculation

                    Decimal ConsumptionType = Convert.ToDecimal(solarrooftop.ConsuptionType);
                    Decimal Monthlyunits = (Convert.ToDecimal(solarrooftop.Units) / ConsumptionType);
                    Decimal DailyUnits = Monthlyunits / 30;
                    ViewBag.lblAnswerUnit = DailyUnits.ToString("0.00") + " units/day";

                    Decimal RooftopCapacity = DailyUnits / 4.5m;
                    ViewBag.lblAnswerKW = RooftopCapacity.ToString("0.00") + " Kw";

                    Decimal NoOfSolarPanel = RooftopCapacity * 3;
                    ViewBag.lblAnswerSolarPanel = NoOfSolarPanel.ToString("0") + " Panels";

                    Decimal AreaInSquareFeet = RooftopCapacity * 95;
                    ViewBag.lblAnswerAreaft = AreaInSquareFeet.ToString("0.00") + "  Sq ft";

                    Decimal AreaInSquareMeter = AreaInSquareFeet / 10.7639m;
                    ViewBag.lblAnswerAreamt = AreaInSquareMeter.ToString("0.00") + "  Sq m";

                    //Decimal AreaInSquareMeter = RooftopCapacity * 95;
                    //ViewBag.lblAnswerAreamt = AreaInSquareMeter.ToString("0.00") + "  Sq m";

                    //Decimal AreaInSquareFeet = AreaInSquareMeter / 10.7639m;
                    //ViewBag.lblAnswerAreaft = AreaInSquareFeet.ToString("0.00") + "  Sq ft";
                    #endregion Calculation


                    #region Formula

                    #region SolarPanel Formula

                    if (solarrooftop.Units != null)
                    {
                        ViewBag.lblDailyUnitConsumption = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo> <mfrac><mn><mi>Monthly Unit Consumption</mi></mn><mrow><mi>30</mi></mrow></mfrac></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo> <mfrac><mn><mi>" + Monthlyunits.ToString("0.0") + "</mi></mn><mrow><mi>30</mi></mrow></mfrac></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + DailyUnits.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi> units/day</mi></math>";

                        ViewBag.lblRooftopCapacity = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo> <mfrac><mn><mi>Daily Unit Consumption</mi></mn><mrow><mi>4.5</mi></mrow></mfrac></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo> <mfrac><mn><mi>" + DailyUnits.ToString("0.00") + "</mi></mn><mrow><mi>4.5</mi></mrow></mfrac></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + RooftopCapacity.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi> Kw</mi></math>";

                        ViewBag.lblNoOfSolarPanel = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn><mi>RoofTop Capacity</mi><mo>&#xD7;</mo><mi>3</mi></mn></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn><mi>" + RooftopCapacity.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>3</mi></mn></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + NoOfSolarPanel.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi> units/day</mi></math>";

                        ViewBag.lblAreaForRoofTop = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo> <mn><mi>RoofTop Capacity</mi><mo>&#xD7;</mo><mi>95</mi></mn></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo> <mn><mi>" + RooftopCapacity.ToString("0.00") + "</mi><mo>&#xD7;</mo><mi>95</mi></mn></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AreaInSquareMeter.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi> Sq meter</mi></math>"
                                                        + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mn>" + AreaInSquareFeet.ToString("0.00") + "</mn><mo>&#xA0;</mo><mi> Sq feet</mi></math>";
                    }

                    #endregion Cement Concrete Volume Formula

                    #endregion Formula                
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Cement Concrete Value For Meter And CM    

        #region Insert Log Function
        public void CalculatorLogInsert(SolarRoofTopCalculator solarrooftop)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Solar-RoofTop-Calculator";

                if (solarrooftop.ConsuptionType != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(solarrooftop.ConsuptionType);

                if (solarrooftop.Units != null)
                {
                    entLOG_Calculation.ParamB = Convert.ToString(solarrooftop.Units);
                }


                entLOG_Calculation.ParamC = ViewBag.lblAnswerUnit + "Units";
                entLOG_Calculation.ParamD = ViewBag.lblAnswerKW + "kw";
                entLOG_Calculation.ParamE = ViewBag.lblAnswerSolarPanel + "Panels";
                entLOG_Calculation.ParamF = ViewBag.lblAnswerAreamt + "Sq m";
                entLOG_Calculation.ParamG = ViewBag.lblAnswerAreaft + "Sq ft";

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
