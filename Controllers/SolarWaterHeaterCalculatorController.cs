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
    public class SolarWaterHeaterCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Solar-Water-Heater-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Solar-Water-Heater-Calculator");


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

            return View("SolarWaterHeaterCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(SolarWaterHeaterCalculator solarwaterheater)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Solar-Water-Heater-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (solarwaterheater.Nos != null && Convert.ToInt32(solarwaterheater.Nos) > 0)
            {
                CalculateSolarWaterHeater(solarwaterheater);
                CalculatorLogInsert(solarwaterheater);
            }
            else
            {
               // String ErrorMsg = " - Enter Valid Value In No of Person";
               // ucMessage.ShowError(ErrorMsg);
            }

            return PartialView("_SolarWaterHeaterCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Anti Termite Value For Meter And CM

        protected void CalculateSolarWaterHeater(SolarWaterHeaterCalculator solarwaterheater)
        {
            try
            {
                if (solarwaterheater.Nos != null && solarwaterheater.Nos != 0)
                {
                    #region Calculation

                    Decimal Nos = Convert.ToDecimal(solarwaterheater.Nos);

                    //Decimal Capacity = (Nos * 50) + ((Nos / 4) * 25);
                    Decimal Capacity = (Nos * 50);

                    ViewBag.lblCapacityOfSolarWaterHeater = Capacity.ToString("0") + "<br/> <small>liters</small>";

                    #endregion Calculation


                    #region Formula For Meter/CM

                    if (solarwaterheater.Nos != null)
                    {
                        //ViewBag.lblSolarWaterHeaterFormula = @"Capacity = <math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mrow><mn>(</mn><mo>&#8290;</mo><mi>No.</mi><mo>&#8290;</mo><mi>of</mi><mo>&#8290;</mo><mi>person</mi> <mo>&#xD7;</mo><mi>50</mi><mn>)</mn></mrow> <mo>+</mo> <mn>((</mn><mfrac><mrow><mi>No</mi><mo>&#8290;</mo><mi>of</mi><mo>&#8290;</mo><mi>Person</mi></mrow><mn>4</mn></mfrac></mrow><mn>)</mn><mo>&#xD7;</mo><mo>&#8290;</mo><mn>25</mn><mn>)</mn></math>"
                        //                                    + @"<br /><br />Capacity = <math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mrow><mn>(</mn><mo>&#8290;</mo><mi>" + solarwaterheater.Nos+ "</mi> <mo>&#xD7;</mo><mi>50</mi><mn>)</mn></mrow> <mo>+</mo> <mn>((</mn><mfrac><mrow><mi>"+solarwaterheater.Nos+"</mi></mrow><mn>4</mn></mfrac></mrow><mn>)</mn><mo>&#xD7;</mo><mo>&#8290;</mo><mn>25</mn><mn>)</mn></math>"
                        //                                    + @"<br /><br />Capacity = <math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mo>&#8290;</mo><mi>" + Capacity.ToString("0.00")+" liters</mi></mrow>";
                        ViewBag.lblSolarWaterHeaterFormula = @"Capacity = <math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mrow><mo>&#8290;</mo><mi>No.</mi><mo>&#8290;</mo><mi>of</mi><mo>&#8290;</mo><mi>persons</mi> <mo>&#xD7;</mo><mi>50</mi></mrow></math>"
                                                          + @"<br /><br />Capacity = <math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mrow><mo>&#8290;</mo><mi>" + solarwaterheater.Nos + "</mi> <mo>&#xD7;</mo><mi>50</mi></mrow></math>"
                                                          + @"<br /><br />Capacity = <math xmlns='http://www.w3.org/1998/Math/MathML'><mrow><mo>&#8290;</mo><mi>" + Capacity.ToString("0.00") + " liters</mi></mrow>";
                    }

                    #endregion Formula For Meter/CM
                }
            }
            catch (Exception ex)
            {
                //ucMessage.ShowError(ex.Message);
            }
        }

        #endregion Function Calculate Anti Termite Value For Meter And CM

        #region Insert Log Function
        public void CalculatorLogInsert(SolarWaterHeaterCalculator solarwaterheater)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Solar-Water-Heater-Calculator";

                if (solarwaterheater.Nos != null)
                {
                    entLOG_Calculation.ParamA = Convert.ToString(solarwaterheater.Nos);
                }
                entLOG_Calculation.ParamB = Convert.ToString(ViewBag.lblCapacityOfSolarWaterHeater);
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
