using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.DAL;
using CivilCalc.DAL.LOG.LOG_Calculation;
using CivilCalc.Models;
using Microsoft.AspNetCore.Mvc;
using CivilEngineeringCalculators;
using SelectForSearch_Result = CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result;
using CivilCalc.Areas.LOG_Calculation.Models;

namespace CivilCalc.Controllers
{
    public class PlywoodSheetsCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Plywood-Sheets-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Plywood-Sheets-Calculator");


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

            return View("PlywoodSheetsCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(PlywoodSheetsCalculator plywoodsheets)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Plywood-Sheets-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            if (plywoodsheets.UnitID == 1)
                CalculateValueForMeterAndCM(plywoodsheets);
            else
                CalculateValueForFeetAndInch(plywoodsheets);

            CalculatorLogInsert(plywoodsheets);

            return PartialView("_PlywoodSheetsCalculatorResult", vModel);
        }
        #endregion

        #region Function: Calculate Value For Meter And CM

        protected void CalculateValueForMeterAndCM(PlywoodSheetsCalculator plywoodsheets)
        {
            try
            {
                if (plywoodsheets.RoomLengthA != null && plywoodsheets.RoomWidthA != null && plywoodsheets.PlywoodLengthA != null && plywoodsheets.PlywoodWidthA != null)
                {
                    #region Variables

                    Decimal RoomAreaMeterAndCM, RoomAreaFeetAndInch, PlywoodCoverMeterAndCM, PlywoodCoverFeetAndInch, PlywoodSheets;

                    #endregion Variables

                    #region Calculation

                    RoomAreaMeterAndCM = CommonFunctions.Area(Convert.ToDecimal(plywoodsheets.RoomLengthA + "." + plywoodsheets.RoomLengthB), Convert.ToDecimal(plywoodsheets.RoomWidthA + "." + plywoodsheets.RoomWidthB));
                    ViewBag.lblAnswerRoomAreaMeterAndCMValue = RoomAreaMeterAndCM.ToString("0.00") + " m<sup>2</sup>";

                    RoomAreaFeetAndInch = CommonFunctions.ConvertFeetAndInchForArea(RoomAreaMeterAndCM);
                    ViewBag.lblAnswerRoomAreaFeetAndInchValue = RoomAreaFeetAndInch.ToString("0.00") + " ft<sup>2</sup>";

                    PlywoodCoverMeterAndCM = CommonFunctions.Area(Convert.ToDecimal(plywoodsheets.PlywoodLengthA + "." + plywoodsheets.PlywoodLengthB), Convert.ToDecimal(plywoodsheets.PlywoodWidthA + "." + plywoodsheets.PlywoodWidthB));
                    ViewBag.lblAnswerPlywoodCoverMeterAndCMValue = PlywoodCoverMeterAndCM.ToString("0.00") + " m<sup>2</sup>";

                    PlywoodCoverFeetAndInch = CommonFunctions.ConvertFeetAndInchForArea(PlywoodCoverMeterAndCM);
                    ViewBag.lblAnswerPlywoodCoverFeetAndInchValue = PlywoodCoverFeetAndInch.ToString("0.00") + " ft<sup>2</sup>";

                    RoomAreaFeetAndInch = CommonFunctions.ConvertFeetAndInchForArea(RoomAreaMeterAndCM);
                    PlywoodSheets = RoomAreaMeterAndCM / PlywoodCoverMeterAndCM;
                    ViewBag.lblAnswerSheetsNo = PlywoodSheets.ToString("0.00");

                    #endregion Calculation

                    #region Formula

                    if (plywoodsheets.RoomLengthB != null || plywoodsheets.RoomWidthB != null)
                    {
                        ViewBag.lblRoomAreaFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Room Length</mi><mo>&#xD7;</mo><mi>Room Width</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + plywoodsheets.RoomLengthA + "." + plywoodsheets.RoomLengthB + "</mi><mo>&#xD7;</mo><mi>" + plywoodsheets.RoomWidthA + "." + plywoodsheets.RoomWidthB + "</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoomAreaMeterAndCM.ToString("0.00") + " sq. mt.</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoomAreaFeetAndInch.ToString("0.00") + " sq. ft.</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblRoomAreaFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Room Length</mi><mo>&#xD7;</mo><mi>Room Width</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + plywoodsheets.RoomLengthA + "</mi><mo>&#xD7;</mo><mi>" + plywoodsheets.RoomWidthA + "</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoomAreaMeterAndCM.ToString("0.00") + " sq. mt.</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoomAreaFeetAndInch.ToString("0.00") + " sq. ft.</mi></math>";
                    }

                    if (plywoodsheets.PlywoodLengthB != null || plywoodsheets.PlywoodWidthB != null)
                    {
                        ViewBag.lblPlywoodCoverFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Plywood Length</mi><mo>&#xD7;</mo><mi>Plywood Width</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + plywoodsheets.PlywoodLengthA + "." + plywoodsheets.PlywoodLengthB + "</mi><mo>&#xD7;</mo><mi>" + plywoodsheets.PlywoodWidthA + "." + plywoodsheets.PlywoodWidthB + "</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodCoverMeterAndCM.ToString("0.00") + " sq. mt.</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodCoverFeetAndInch.ToString("0.00") + " sq. ft.</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblPlywoodCoverFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Plywood Length</mi><mo>&#xD7;</mo><mi>Plywood Width</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + plywoodsheets.PlywoodLengthA + "</mi><mo>&#xD7;</mo><mi>" + plywoodsheets.PlywoodWidthA + "</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodCoverMeterAndCM.ToString("0.00") + " sq. mt.</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodCoverFeetAndInch.ToString("0.00") + " sq. ft.</mi></math>";
                    }

                    ViewBag.lblPlywoodSheetsFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>Room Area (In sq. mt.)</mi><mi>Plywood Cover (In sq. mt.)</mi></mfrac></math>"
                                                 + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>" + RoomAreaMeterAndCM.ToString("0.00") + " sq. mt.</mi><mi>" + PlywoodCoverMeterAndCM.ToString("0.00") + " sq. mt.</mi></mfrac></math>"
                                                 + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodSheets.ToString("0.00") + " Sheets</mi></math>";

                    #endregion Formula
                }
            }
            catch (Exception e)
            {
                //ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function: Calculate Value For Meter And CM

        #region Function: Calculate Value For Feet And Inch

        protected void CalculateValueForFeetAndInch(PlywoodSheetsCalculator plywoodsheets)
        {
            try
            {
                if (plywoodsheets.RoomLengthA != null && plywoodsheets.RoomWidthA != null && plywoodsheets.PlywoodLengthA != null && plywoodsheets.PlywoodWidthA != null)
                {
                    #region Variables

                    Decimal RoomAreaMeterAndCM, RoomAreaFeetAndInch, PlywoodCoverMeterAndCM, PlywoodCoverFeetAndInch, PlywoodSheets;

                    #endregion Variables

                    #region Calculation

                    RoomAreaFeetAndInch = CommonFunctions.Area(Convert.ToDecimal(plywoodsheets.RoomLengthA + "." + plywoodsheets.RoomLengthB), Convert.ToDecimal(plywoodsheets.RoomWidthA + "." + plywoodsheets.RoomWidthB));
                    ViewBag.lblAnswerRoomAreaFeetAndInchValue = RoomAreaFeetAndInch.ToString("0.00") + " ft<sup>2</sup>";

                    RoomAreaMeterAndCM = CommonFunctions.ConvertMeterAndCMForArea(RoomAreaFeetAndInch);
                    ViewBag.lblAnswerRoomAreaMeterAndCMValue = RoomAreaMeterAndCM.ToString("0.00") + " m<sup>2</sup>";

                    PlywoodCoverFeetAndInch = CommonFunctions.Area(Convert.ToDecimal(plywoodsheets.PlywoodLengthA + "." + plywoodsheets.PlywoodLengthB), Convert.ToDecimal(plywoodsheets.PlywoodWidthA + "." + plywoodsheets.PlywoodWidthB));
                    ViewBag.lblAnswerPlywoodCoverFeetAndInchValue = PlywoodCoverFeetAndInch.ToString("0.00") + " ft<sup>2</sup>";

                    PlywoodCoverMeterAndCM = CommonFunctions.ConvertMeterAndCMForArea(PlywoodCoverFeetAndInch);
                    ViewBag.lblAnswerPlywoodCoverMeterAndCMValue = PlywoodCoverMeterAndCM.ToString("0.00") + " m<sup>2</sup>";

                    PlywoodSheets = RoomAreaFeetAndInch / PlywoodCoverFeetAndInch;
                    ViewBag.lblAnswerSheetsNo = PlywoodSheets.ToString("0.00");

                    #endregion Calculation

                    #region Formula

                    if (plywoodsheets.RoomLengthB != null || plywoodsheets.RoomWidthB != null)
                    {
                        ViewBag.lblRoomAreaFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Room Length</mi><mo>&#xD7;</mo><mi>Room Width</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + plywoodsheets.RoomLengthA + "." + plywoodsheets.RoomLengthB + "</mi><mo>&#xD7;</mo><mi>" + plywoodsheets.RoomWidthA + "." + plywoodsheets.RoomWidthB + "</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoomAreaFeetAndInch.ToString("0.00") + " sq. ft.</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoomAreaMeterAndCM.ToString("0.00") + " sq. mt.</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblRoomAreaFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Room Length</mi><mo>&#xD7;</mo><mi>Room Width</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + plywoodsheets.RoomLengthA + "</mi><mo>&#xD7;</mo><mi>" + plywoodsheets.RoomWidthA + "</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoomAreaFeetAndInch.ToString("0.00") + " sq. ft.</mi></math>"
                                                + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + RoomAreaMeterAndCM.ToString("0.00") + " sq. mt.</mi></math>";
                    }

                    if (plywoodsheets.PlywoodLengthB != null || plywoodsheets.PlywoodWidthB != null)
                    {
                        ViewBag.lblPlywoodCoverFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Plywood Length</mi><mo>&#xD7;</mo><mi>Plywood Width</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + plywoodsheets.PlywoodLengthA + "." + plywoodsheets.PlywoodLengthB + "</mi><mo>&#xD7;</mo><mi>" + plywoodsheets.PlywoodWidthA + "." + plywoodsheets.PlywoodWidthB + "</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodCoverFeetAndInch.ToString("0.00") + " sq. ft.</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodCoverMeterAndCM.ToString("0.00") + " sq. mt.</mi></math>";
                    }
                    else
                    {
                        ViewBag.lblPlywoodCoverFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>Plywood Length</mi><mo>&#xD7;</mo><mi>Plywood Width</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + plywoodsheets.PlywoodLengthA + "</mi><mo>&#xD7;</mo><mi>" + plywoodsheets.PlywoodWidthA + "</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodCoverFeetAndInch.ToString("0.00") + " sq. ft.</mi></math>"
                                                    + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodCoverMeterAndCM.ToString("0.00") + " sq. mt.</mi></math>";
                    }

                    ViewBag.lblPlywoodSheetsFormula = @"<math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>Room Area (In sq. ft.)</mi><mi>Plywood Cover (In sq. ft.)</mi></mfrac></math>"
                                                 + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfrac><mi>" + RoomAreaFeetAndInch.ToString("0.00") + " sq. ft.</mi><mi>" + PlywoodCoverFeetAndInch.ToString("0.00") + " sq. ft.</mi></mfrac></math>"
                                                 + @"<br/><br/><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mi>" + PlywoodSheets.ToString("0.00") + " Sheets</mi></math>";

                    #endregion Formula
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function: Calculate Value For Feet And Inch

        #region Insert Log Function

        public void CalculatorLogInsert(PlywoodSheetsCalculator plywoodsheets)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Plywood-Sheets-Calculator";

                if (plywoodsheets.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(plywoodsheets.UnitID);

                if (plywoodsheets.RoomLengthA != null)
                    if (plywoodsheets.RoomLengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(plywoodsheets.RoomLengthA + "." + plywoodsheets.RoomLengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(plywoodsheets.RoomLengthA);

                if (plywoodsheets.RoomWidthA != null)
                    if (plywoodsheets.RoomWidthB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(plywoodsheets.RoomWidthA + "." + plywoodsheets.RoomWidthB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(plywoodsheets.RoomWidthA);

                if (plywoodsheets.PlywoodLengthA != null)
                    if (plywoodsheets.PlywoodLengthB != null)
                        entLOG_Calculation.ParamD = Convert.ToString(plywoodsheets.PlywoodLengthA + "." + plywoodsheets.PlywoodLengthB);
                    else
                        entLOG_Calculation.ParamD = Convert.ToString(plywoodsheets.PlywoodLengthA);

                if (plywoodsheets.PlywoodWidthA != null)
                    if (plywoodsheets.PlywoodWidthB != null)
                        entLOG_Calculation.ParamE = Convert.ToString(plywoodsheets.PlywoodWidthA + "." + plywoodsheets.PlywoodWidthA);
                    else
                        entLOG_Calculation.ParamE = Convert.ToString(plywoodsheets.PlywoodWidthA);

                entLOG_Calculation.ParamE = ViewBag.lblAnswerRoomAreaMeterAndCMValue + " Room Area";
                entLOG_Calculation.ParamF = ViewBag.lblAnswerRoomAreaFeetAndInchValue + " Room Area";
                entLOG_Calculation.ParamG = ViewBag.lblAnswerPlywoodCoverMeterAndCMValue + " Plywood Covers";
                entLOG_Calculation.ParamH = ViewBag.lblAnswerPlywoodCoverFeetAndInchValue + " Plywood Covers";
                entLOG_Calculation.ParamI = ViewBag.lblAnswerSheetsNo + " Sheets";
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
