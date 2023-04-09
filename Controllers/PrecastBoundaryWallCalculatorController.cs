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
    public class PrecastBoundaryWallCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Precast-Readymade-Compound-Wall-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Precast-Readymade-Compound-Wall-Calculator");


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

            return View("PrecastBoundaryWallCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(PrecastBoundaryWallCalculator precastboundarywall)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Precast-Readymade-Compound-Wall-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            CalculatePreCastWallValue(precastboundarywall);
            CalculatorLogInsert(precastboundarywall);

            return PartialView("_PrecastBoundaryWallCalculatorResult", vModel);
        }
        #endregion

        #region Function Calculate Pre Cast Wall Value

        protected void CalculatePreCastWallValue(PrecastBoundaryWallCalculator precastboundarywall)
        {
            try
            {
                if (precastboundarywall.LengthA != null && precastboundarywall.HeightA != null && precastboundarywall.LengthofBarA != null && precastboundarywall.HeightofBarA != null)
                {
                    #region Variables

                    decimal AreaLength;
                    decimal AreaHeight;
                    decimal LengthofBar;
                    decimal HeightofBar;
                    decimal ActualArea;
                    decimal NoofHorizontalBarWithoutSpace = 0;
                    decimal NoofHorizontalBarWithSpace = 0;
                    decimal NoofVerticalPost = 0;
                    string unit = (precastboundarywall.UnitID == 1) ? "m" : "ft";
                    decimal Space = (precastboundarywall.UnitID == 1) ? 0.1524m : 0.5m;

                    #endregion Variables

                    #region Load Textbox value

                    AreaLength = Convert.ToDecimal(precastboundarywall.LengthA + "." + precastboundarywall.LengthB);
                    AreaHeight = Convert.ToDecimal(precastboundarywall.HeightA + "." + precastboundarywall.HeightB);
                    LengthofBar = Convert.ToDecimal(precastboundarywall.LengthofBarA + "." + precastboundarywall.LengthofBarB);
                    HeightofBar = Convert.ToDecimal(precastboundarywall.HeightofBarA + "." + precastboundarywall.HeightofBarB);

                    #endregion Load Textbox value

                    #region Horizontal Bar Without Space

                    NoofHorizontalBarWithoutSpace = Math.Ceiling((AreaLength / LengthofBar) * (AreaHeight / HeightofBar));

                    #endregion Horizontal Bar Without Space

                    #region Vertical Post Without Space

                    NoofVerticalPost = Math.Ceiling(AreaLength / LengthofBar) + 1;

                    #endregion Vertical Post Without Space

                    #region Horizontal Bar With Space

                    ActualArea = Math.Abs(AreaLength - ((AreaLength / LengthofBar) * Space));
                    NoofHorizontalBarWithSpace = Math.Ceiling(((ActualArea) / LengthofBar) * (AreaHeight / HeightofBar));

                    #endregion Horizontal Bar With Space

                    #region Answer Panel

                    ViewBag.lblAnserHorizonatlBarA = NoofHorizontalBarWithoutSpace.ToString("0") + " Nos";
                    ViewBag.lblAnserVerticalPostA = NoofVerticalPost.ToString("0") + " Nos";
                    ViewBag.lblAnswerHorizontalBarB = NoofHorizontalBarWithSpace.ToString("0") + " Nos";
                    ViewBag.lblAnswerVerticalPostB = NoofVerticalPost.ToString("0") + " Nos";

                    #endregion Answer Panel

                    #region Formula

                    ViewBag.lblFormulaWithoutSpace = @"<h4><b>Horizontal Bar Require With Given Area</h4></b>"
                                                  + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Horizontal Bar</mi>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfenced><mfrac><mrow><mn>Length of Area</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Height of Area</mi></mrow><mrow><mi>Horizontal Bar Length</mi><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Horizontal Bar Height</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Horizontal Bar</mi><mo>=</mo><mfenced><mfrac><mrow><mn>" + AreaLength.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>" + AreaHeight.ToString("0.00") + "</mi></mrow><mrow><mi>" + LengthofBar.ToString("0.00") + "</mi><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>" + HeightofBar.ToString("0.00") + "</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Horizontal Bar</mi><mo>=</mo><mi>" + NoofHorizontalBarWithoutSpace.ToString("0") + "</mi></math>"
                                                  + @"<div class='alert alert-block alert-success text-center' style='margin: 5px;'><strong>Note: </strong> No Space is Kept Between <br />horizontal bar </div><hr />"
                                                  + @"<h4><b>No of Vertical Post Require</h4></b>"
                                                  + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Vertical Post</mi>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfenced><mfrac><mrow><mn>Length of Area</mn></mrow><mrow><mi>Horizontal Bar Length</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo><mo>+</mo><mi>1</mi></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Vertical Post</mi><mo>=</mo><mfenced><mfrac><mrow><mn>" + AreaLength.ToString("0.00") + "</mn></mrow><mrow><mi>" + LengthofBar.ToString("0.00") + "</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo><mo>+</mo><mi>1</mi></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Vertical Post</mi><mo>=</mo><mi>" + NoofVerticalPost.ToString("0") + "</mi></math>";

                    ViewBag.lblFormulaWithSpace = @"<h4><b>Horizontal Bar Require With Actual Area</h4></b>"
                                                  + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Actual Area To be Covered By Horizontal Bar=&nbsp;</mi>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>= Length of Area</mi><mo>-</mo> <mfenced><mfrac><mrow><mn>Length of Area</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>" + Space.ToString("0.00") + "</mi></mrow><mrow><mi>Horizontal Bar Length</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>= " + AreaLength.ToString("0.00") + "</mi><mo>-</mo> <mfenced><mfrac><mrow><mn>" + AreaLength.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>" + Space.ToString("0.00") + "</mi></mrow><mrow><mi>" + LengthofBar.ToString("0.00") + "</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Actual Area To be Covered By Horizontal Bar=&nbsp;</mi><mi>" + ActualArea.ToString("0.00") + "</mi>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>Horizontal Bar=</mo><mfenced><mfrac><mrow><mn>Actual Area of Horizontal Bar</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Height of Area</mi></mrow><mrow><mi>Horizontal Bar Length</mi><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>Horizontal Bar Height</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Horizontal Bar</mi><mo>=</mo><mfenced><mfrac><mrow><mn>" + ActualArea.ToString("0.00") + "</mn><mo>&#xA0;</mo><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>" + AreaHeight.ToString("0.00") + "</mi></mrow><mrow><mi>" + LengthofBar.ToString("0.00") + "</mi><mo>&#xA0;</mo><mo>&#xD7;</mo><mo>&#xA0;</mo><mi>" + HeightofBar.ToString("0.00") + "</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Horizontal Bar</mi><mo>=</mo><mi>" + NoofHorizontalBarWithSpace.ToString("0") + "</mi></math>"
                                                  + @"<div class='alert alert-block alert-success text-center' style='margin: 5px;'><strong>Note: </strong> Assuming " + Space + " " + unit + " <br />space kept between horizontal bar </div>"
                                                  + @"<hr /><h4><b>No of Vertical Post Require</h4></b>"
                                                  + @"<br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mo>=</mo><mfenced><mfrac><mrow><mn>Length of Area</mn></mrow><mrow><mi>Horizontal Bar Length</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo><mo>+</mo><mi>1</mi></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Vertical Post</mi><mo>=</mo><mfenced><mfrac><mrow><mn>" + AreaLength.ToString("0.00") + "</mn></mrow><mrow><mi>" + LengthofBar.ToString("0.00") + "</mi></mrow></mfrac></mfenced><mo>&#xA0;</mo><mo>+</mo><mi>1</mi></math>"
                                                  + @"<br /><br /><math xmlns=""http://www.w3.org/1998/Math/MathML""><mi>Total Require Vertical Post</mi><mo>=</mo><mi>" + NoofVerticalPost.ToString("0") + "</mi></math>";

                    #endregion Formula
                }
            }
            catch (Exception e)
            {
               // ucMessage.ShowError(e.Message);
            }
        }

        #endregion Function Calculate Pre Cast Wall Value

        #region Insert Log Function

        public void CalculatorLogInsert(PrecastBoundaryWallCalculator precastboundarywall)
        {
            try
            {
                LOG_CalculationModel entLOG_Calculation = new LOG_CalculationModel();

                #region Gather Data

                entLOG_Calculation.ScreenName = "Precast_Wall-Calculator";

                if (precastboundarywall.UnitID != -1)
                    entLOG_Calculation.ParamA = Convert.ToString(precastboundarywall.UnitID);

                if (precastboundarywall.LengthA != null)
                {
                    if (precastboundarywall.LengthB != null)
                        entLOG_Calculation.ParamB = Convert.ToString(precastboundarywall.LengthA + "." + precastboundarywall.LengthB);
                    else
                        entLOG_Calculation.ParamB = Convert.ToString(precastboundarywall.LengthA);
                }

                if (precastboundarywall.HeightA != null)
                {
                    if (precastboundarywall.HeightB != null)
                        entLOG_Calculation.ParamC = Convert.ToString(precastboundarywall.HeightA + "." + precastboundarywall.HeightB);
                    else
                        entLOG_Calculation.ParamC = Convert.ToString(precastboundarywall.HeightA);
                }

                if (precastboundarywall.LengthofBarA != null)
                {
                    if (precastboundarywall.LengthofBarB != null)
                        entLOG_Calculation.ParamD = Convert.ToString(precastboundarywall.LengthofBarA + "." + precastboundarywall.LengthofBarB);
                    else
                        entLOG_Calculation.ParamD = Convert.ToString(precastboundarywall.LengthofBarA);
                }

                if (precastboundarywall.HeightofBarA != null)
                {
                    if (precastboundarywall.HeightofBarB != null)
                        entLOG_Calculation.ParamE = Convert.ToString(precastboundarywall.HeightofBarA + "." + precastboundarywall.HeightofBarB);
                    else
                        entLOG_Calculation.ParamE = Convert.ToString(precastboundarywall.HeightofBarA);
                }

                entLOG_Calculation.ParamF = ViewBag.lblAnserHorizonatlBarA;
                entLOG_Calculation.ParamG = ViewBag.lblAnserVerticalPostA;
                entLOG_Calculation.ParamH = ViewBag.lblAnswerHorizontalBarB;
                entLOG_Calculation.ParamI = ViewBag.lblAnswerVerticalPostB;
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
