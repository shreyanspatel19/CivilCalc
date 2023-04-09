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
    public class PaintWorkCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Paint-Work-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Paint-Work-Calculator");


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

            return View("PaintWorkCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(PaintWorkCalculator paintwork)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Paint-Work-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            CalculatePaint_WorkValue(paintwork);
            CalculatorLogInsert(paintwork);

            return PartialView("_PaintWorkCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Paint Work Value

        protected void CalculatePaint_WorkValue(PaintWorkCalculator paintwork)
        {
            try
            {
                if (paintwork.Carpet_Area != null)
                {
                    #region Variables

                    decimal CarpetArea = (paintwork.Carpet_Area == null) ? 0 : Convert.ToDecimal(paintwork.Carpet_Area);
                    decimal Doorheight = (paintwork.DoorHeight == null) ? 0 : Convert.ToDecimal(paintwork.DoorHeight);
                    decimal Doorwidth = (paintwork.DoorWidth == null) ? 0 : Convert.ToDecimal(paintwork.DoorWidth);
                    decimal Windowheight = (paintwork.WindowHeight == null) ? 0 : Convert.ToDecimal(paintwork.WindowHeight);
                    decimal Windowwidth = (paintwork.WindowWidth == null) ? 0 : Convert.ToDecimal(paintwork.WindowWidth);
                    decimal NoofDoors = (paintwork.NoofDoors == null) ? 0 : Convert.ToDecimal(paintwork.NoofDoors);
                    decimal NoofWindows = (paintwork.NoofWindows == null) ? 0 : Convert.ToDecimal(paintwork.NoofWindows);
                    decimal ApproxPaintArea = 0, DoorArea = 0, WindowArea = 0, ActualPaintAreaft = 0, ActualPaintAream = 0, paint = 0, primer = 0, putty = 0;
                    string unit = (paintwork.UnitID == 1) ? "m" : "ft";

                    #endregion Variables

                    #region Approx Paint Area

                    ApproxPaintArea = CarpetArea * 3.5m;

                    #endregion Approx Paint Area

                    #region Door Area

                    DoorArea = Doorheight * Doorwidth * NoofDoors;

                    #endregion Door Area

                    #region Window Area

                    WindowArea = Windowheight * Windowwidth * NoofWindows;

                    #endregion Window Area

                    #region Actual Paint Area

                    if (paintwork.UnitID == 1)
                    {
                        ActualPaintAream = ApproxPaintArea - DoorArea - WindowArea;
                        ActualPaintAreaft = CommonFunctions.ConvertFeetAndInchForArea(ActualPaintAream);
                    }
                    else
                    {
                        ActualPaintAreaft = ApproxPaintArea - DoorArea - WindowArea;
                        ActualPaintAream = CommonFunctions.ConvertMeterAndCMForArea(ActualPaintAreaft);
                    }

                    #endregion Actual Paint Area

                    #region Require Material

                    paint = ActualPaintAreaft / 100;
                    primer = ActualPaintAreaft / 100;
                    putty = (ActualPaintAreaft / 100) * 2.5m;

                    #endregion Require Material

                    #region Set Answer in Label

                    ViewBag.lblAnswerPaint_SquareFeetAndInchValue = ActualPaintAreaft.ToString("0.00") + " ft<sup>2</sup>";
                    ViewBag.lblAnswerPaint_SquareMeterAndCMValue = ActualPaintAream.ToString("0.00") + " m<sup>2</sup>";
                    ViewBag.lblAnswerPaint = paint.ToString("0.00") + " liters";
                    ViewBag.lblAnswerPrimer = primer.ToString("0.00") + " liters";
                    ViewBag.lblAnswerPutty = putty.ToString("0.00") + " kgs";

                    #endregion Set Answer in Label

                    #region Formula

                    ViewBag.lblPaintAreaFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Paint Area = </mi><mrow><msub><mi>Carpet Area</mi></msub><mo>&#xD7;</mo><msub><mi>3.5</mi></msub></mrow>"
                                              + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Paint Area = </mi><mrow><msub><mi>" + CarpetArea.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>3.5</mi></msub></mrow>"
                                              + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Paint Area = </mi><mrow><msub><mi>" + ApproxPaintArea.ToString("0.00") + "</mi></msup></mrow><msup><mi>" + unit + "</mi><mn>2</mn></msub>";

                    ViewBag.lblDoorAreaFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Door</mi><mi>Area</mi><mo>=</mo><mi>Width</mi><mo>&#xD7;</mo><mi>Height</mi><mo>&#xD7;</mo><mo>Doors</mo></math>"
                                              + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Door Area = </mi><mrow><msub><mi>" + Doorwidth.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + Doorheight.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + NoofDoors.ToString("0") + "</mi></msub></mrow>"
                                              + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Door Area = </mi><mrow><msub><mi>" + DoorArea.ToString("0.00") + "</mi></msub><msup><mi>" + unit + "</mi><mn>2</mn></msub></mrow>"
                                              + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Window</mi><mi>Area</mi><mo>=</mo><mi>Width</mi><mo>&#xD7;</mo><mi>Height</mi><mo>&#xD7;</mo><mi>Windows</mi></math>"
                                              + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Window Area = </mi><mrow><msub><mi>" + Windowwidth.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + Windowheight.ToString("0.00") + "</mi></msub><mo>&#xD7;</mo><msub><mi>" + NoofWindows.ToString("0") + "</mi></msub></mrow>"
                                              + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Window Area = </mi><mrow><msub><mi>" + WindowArea.ToString("0.00") + "</mi></msub><msup><mi>" + unit + "</mi><mn>2</mn></msup></mrow>";

                    ViewBag.lblActualPaintFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>=</mi><mrow><mi>Carpet Area</mi><mo>-</mo><mi>Door Area</mi><mo>-</mo><mi>Window Area</mi></mrow>"
                                                + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>=</mi><mrow><mi>" + ApproxPaintArea.ToString("0.00") + "</mi><mo>-</mo><mi>" + DoorArea.ToString("0.00") + "</mi><mo>-</mo><mi>" + WindowArea.ToString("0.00") + "</mi></mrow>"
                                                + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Actual Paint Area = </mi><mrow><msub><mi>" + ActualPaintAream.ToString("0.00") + "</mi></msub><msup><mi>m</mi><mn>2</mn></msup></mrow></math>"
                                                + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Actual Paint Area = </mi><mrow><msub><mi>" + ActualPaintAreaft.ToString("0.00") + "</mi></msub><msup><mi>ft</mi><mn>2</mn></msup></mrow></math></br></br></br></br></br>";

                    ViewBag.lblPaintFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Paint =</mo><mfrac><mrow><mi>Actual Paint Area</mi></mrow><mn>100</mn></mfrac></math>"
                                            + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Paint =</mo><mfrac><mrow><mi>" + ActualPaintAreaft.ToString("0.00") + "</mi><msup><mi>ft</mi><mn>2</mn></msup></mrow><mn>100</mn></mfrac></math>"
                                            + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Paint =</mo><mrow><mi>" + paint.ToString("0.00") + " liter</mi></mrow></math>";

                    ViewBag.lblPrimerFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Primer =</mo><mfrac><mrow><mi>Actual Paint Area</mi></mrow><mn>100</mn></mfrac></math>"
                                            + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Primer =</mo><mfrac><mrow><mi>" + ActualPaintAreaft.ToString("0.00") + "</mi><msup><mi>ft</mi><mn>2</mn></msup></mrow><mn>100</mn></mfrac></math>"
                                            + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Primer =</mo><mrow><mi>" + primer.ToString("0.00") + " liter</mi></mrow></math>";

                    ViewBag.lblPuttyFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Putty =</mo><mfrac><mrow><mi>Actual Paint Area</mi></mrow><mn>40</mn></mfrac></math>"
                                            + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Putty =</mo><mfrac><mrow><mi>" + ActualPaintAreaft.ToString("0.00") + "</mi><msup><mi>ft</mi><mn>2</mn></msup></mrow><mn>40</mn></mfrac></math>"
                                            + @"</br></br><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Putty =</mo><mrow><mi>" + putty.ToString("0.00") + " kgs</mi></mrow></math>";

                    #endregion Formula
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Paint Work Value

        #region Insert Log Function

        public void CalculatorLogInsert(PaintWorkCalculator paintwork)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Paint-Work-CMalculator";

                if (paintwork.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(paintwork.UnitID);

                if (paintwork.Carpet_Area != null)
                    entLOG_Calculation.ParamB = Convert.ToString(paintwork.Carpet_Area);

                if (paintwork.DoorWidth != null)
                    entLOG_Calculation.ParamC = Convert.ToString(paintwork.DoorWidth);

                if (paintwork.DoorHeight != null)
                    entLOG_Calculation.ParamD = Convert.ToString(paintwork.DoorHeight);

                if (paintwork.NoofDoors != null)
                    entLOG_Calculation.ParamE = Convert.ToString(paintwork.NoofDoors);

                if (paintwork.WindowWidth != null)
                    entLOG_Calculation.ParamF = Convert.ToString(paintwork.WindowWidth);

                if (paintwork.WindowHeight != null)
                    entLOG_Calculation.ParamG = Convert.ToString(paintwork.WindowHeight);

                if (paintwork.NoofWindows != null)
                    entLOG_Calculation.ParamH = Convert.ToString(paintwork.NoofWindows);

                if (paintwork.UnitID == 1)
                {
                    if (ViewBag.lblAnswerPaint_SquareMeterAndCMValue != null)
                    {
                        entLOG_Calculation.ParamI = ViewBag.lblAnswerPaint_SquareMeterAndCMValue;
                        entLOG_Calculation.ParamJ = ViewBag.lblAnswerPaint_SquareFeetAndInchValue;
                    }
                }
                else if (paintwork.UnitID == 2)
                {
                    if (ViewBag.lblAnswerPaint_SquareFeetAndInchValue != null)
                    {
                        entLOG_Calculation.ParamI = ViewBag.lblAnswerPaint_SquareFeetAndInchValue;
                        entLOG_Calculation.ParamJ = ViewBag.lblAnswerPaint_SquareMeterAndCMValue;
                    }
                }

                entLOG_Calculation.ParamK = ViewBag.lblAnswerPaint;
                entLOG_Calculation.ParamL = ViewBag.lblAnswerPrimer;
                entLOG_Calculation.ParamM = ViewBag.lblAnswerPutty;
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
