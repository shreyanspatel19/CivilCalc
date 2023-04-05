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
    public class RoofPitchCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Roof-Pitch-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Roof-Pitch-Calculator");


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

            return View("RoofPitchCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(RoofPitchCalculator roofpitch)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Roof-Pitch-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            Calculate(roofpitch);
            CalculatorLogInsert(roofpitch);

            return PartialView("_RoofPitchCalculatorResult", vModel);
        }
        #endregion

        #region Function: Calculate

        protected void Calculate(RoofPitchCalculator roofpitch)
        {
            try
            {
                if (roofpitch.RiseA != null && roofpitch.RunA != null)
                {
                    #region Variables

                    Decimal RoofPitch, Slop, Angle;

                    #endregion Variables

                    #region Calculation

                    if (roofpitch.RiseB == null || roofpitch.RunB == null)
                    {
                        RoofPitch = Convert.ToDecimal(roofpitch.RiseA) / (Convert.ToDecimal(roofpitch.RunA) / 12);
                        ViewBag.lblAnswerRoofPitch = RoofPitch.ToString("0") + "/12";

                        Slop = (Convert.ToDecimal(roofpitch.RiseA) / Convert.ToDecimal(roofpitch.RunA)) * 100;
                        ViewBag.lblAnswerSlop = Slop.ToString("0.000") + " %";

                        Angle = Convert.ToDecimal(Math.Atan(Convert.ToDouble(roofpitch.RiseA) / Convert.ToDouble((roofpitch.RunA))) * 180 / Math.PI);
                        ViewBag.lblAnswerAngle = Angle.ToString("0.000") + " Degree";
                    }
                    else
                    {
                        RoofPitch = Convert.ToDecimal(roofpitch.RiseA + "." + roofpitch.RiseB) / (Convert.ToDecimal(roofpitch.RunA + "." + roofpitch.RunB) / 12);
                        ViewBag.lblAnswerRoofPitch = RoofPitch.ToString("0") + "/12";

                        Slop = (Convert.ToDecimal(roofpitch.RiseA + "." + roofpitch.RiseB) / Convert.ToDecimal(roofpitch.RunA + "." + roofpitch.RunB)) * 100;
                        ViewBag.lblAnswerSlop = Slop.ToString("0.000") + "%";

                        Angle = Convert.ToDecimal(Math.Atan(Convert.ToDouble(roofpitch.RiseA + "." + roofpitch.RiseB) / Convert.ToDouble((roofpitch.RunA + "." + roofpitch.RunB))) * 180 / Math.PI);
                        ViewBag.lblAnswerAngle = Angle.ToString("0.000");
                    }

                    #endregion Calculation

                    #region Formula

                    if (roofpitch.RiseB != null || roofpitch.RunB != null)
                    {
                        ViewBag.lblRoofPitchFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>S</mi><mfenced><mstyle displaystyle=""true""><mfrac><mi>N</mi><mn>12</mn></mfrac></mstyle></mfenced></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>" + roofpitch.RiseA + "." + roofpitch.RiseB + "</mi><mfenced><mstyle displaystyle='true'><mfrac><mi>" + roofpitch.RunA + "." + roofpitch.RunB + "</mi><mn>12</mn></mfrac></mstyle></mfenced></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoofPitch.ToString("0") + "</mi></math>";

                        ViewBag.lblSlopFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>S</mi><mi>N</mi></mfrac><mo>&#xD7;</mo><mn>100</mn></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>" + roofpitch.RiseA + "." + roofpitch.RiseB + "</mi><mi>" + roofpitch.RunA + "." + roofpitch.RunB + "</mi></mfrac><mo>&#xD7;</mo><mn>100</mn></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Slop.ToString("0.000") + "</mi></math>";

                        ViewBag.lblAngleFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><msup><mi>tan</mi><mrow><mo>-</mo><mn>1</mn></mrow></msup><mo>&#xA0;</mo><mfenced><mfrac><mi>S</mi><mi>N</mi></mfrac></mfenced></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><msup><mi>tan</mi><mrow><mo>-</mo><mn>1</mn></mrow></msup><mo>&#xA0;</mo><mfenced><mfrac><mi>" + roofpitch.RiseA + "." + roofpitch.RiseB + "</mi><mi>" + roofpitch.RunA + "." + roofpitch.RunB + "</mi></mfrac></mfenced></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Angle.ToString("0.000") + "</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblRoofPitchFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>S</mi><mfenced><mstyle displaystyle=""true""><mfrac><mi>N</mi><mn>12</mn></mfrac></mstyle></mfenced></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>" + roofpitch.RiseA + "</mi><mfenced><mstyle displaystyle='true'><mfrac><mi>" + roofpitch.RunA + "</mi><mn>12</mn></mfrac></mstyle></mfenced></mfrac></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoofPitch.ToString("0") + "</mi></math>";

                        ViewBag.lblSlopFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>S</mi><mi>N</mi></mfrac><mo>&#xD7;</mo><mn>100</mn></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>" + roofpitch.RiseA + "</mi><mi>" + roofpitch.RunA + "</mi></mfrac><mo>&#xD7;</mo><mn>100</mn></math>"
                                            + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Slop.ToString("0.000") + "</mi></math>";

                        ViewBag.lblAngleFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><msup><mi>tan</mi><mrow><mo>-</mo><mn>1</mn></mrow></msup><mo>&#xA0;</mo><mfenced><mfrac><mi>S</mi><mi>N</mi></mfrac></mfenced></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><msup><mi>tan</mi><mrow><mo>-</mo><mn>1</mn></mrow></msup><mo>&#xA0;</mo><mfenced><mfrac><mi>" + roofpitch.RiseA + "</mi><mi>" + roofpitch.RunA + "</mi></mfrac></mfenced></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + Angle.ToString("0.000") + "</mi></math>";
                    }

                    #endregion Formula
                }
            }
            catch (Exception e)
            {
              //  ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function: Calculate

        #region Insert Log Function

        public void CalculatorLogInsert(RoofPitchCalculator roofpitch)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();
                #region Gather Data

                entLOG_Calculation.ScreenName = "Roof-Pitch-Calculator";

                if (roofpitch.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(roofpitch.UnitID);

                if (roofpitch.RiseA != null)
                    if (roofpitch.RiseB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(roofpitch.RiseA + "." + roofpitch.RiseB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(roofpitch.RiseA);

                if (roofpitch.RunA != null)
                    if (roofpitch.RunB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(roofpitch.RunA + "." + roofpitch.RunB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(roofpitch.RunA);

                entLOG_Calculation.ParamD = ViewBag.lblAnswerRoofPitch + " RoofPitch";
                entLOG_Calculation.ParamE = ViewBag.lblAnswerSlop + " Slop";
                entLOG_Calculation.ParamF = ViewBag.lblAnswerAngle + " Angle";
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
