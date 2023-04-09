using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.DAL;
using CivilCalc.Models;
using Microsoft.AspNetCore.Mvc;
using SelectForSearch_Result = CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result;

namespace CivilCalc.Controllers
{
    public class CarpetAreaCalculatorController : Controller
    {

        #region Index
        [Route("Quantity-Estimator/Carpet-Built-Up-Super-Built-Up-Area-Calculator")]
        public IActionResult Index()
        {
            List<CivilCalc.DAL.CAL.CAL_Calculator.SelectForSearch_Result> lstCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Carpet-Built-Up-Super-Built-Up-Area-Calculator");


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

            return View("CarpetAreaCalculator");
        }
        #endregion

        #region _Calculation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Calculation(CarpetAreaCalculator carpetarea)
        {
            var vCalculator = DBConfig.dbCALCalculator.SelectByURLName("/Quantity-Estimator/Carpet-Built-Up-Super-Built-Up-Area-Calculator").SingleOrDefault();
            Mapper.Initialize(config => config.CreateMap<SelectForSearch_Result, CAL_CalculatorModel>());
            var vModel = AutoMapper.Mapper.Map<SelectForSearch_Result, CAL_CalculatorModel>(vCalculator);

            ViewBag.Page = DBConfig.dbCALCalculatorContent.SelectByCalculator(vModel.CalculatorID).ToList();


            AddNewRecordRowToGrid(carpetarea);
            CarpetArea(carpetarea);
            BuiltupArea(carpetarea);
            ClearControl(carpetarea);

            return PartialView("_CarpetAreaCalculatorResult", vModel);
        }
        #endregion

        #region Add new Record in Datatable

        private void AddNewRecordRowToGrid(CarpetAreaCalculator carpetarea)
        {
            Double lengthFeet, lengthInch, breadthFeet, breadthInch, length, breadth, Area;

            if (carpetarea.LengthFeet != null)
                lengthFeet = Convert.ToDouble(carpetarea.LengthFeet);
            else
                lengthFeet = Convert.ToDouble(0.0);

            if (carpetarea.LengthInche != null)
                lengthInch = Convert.ToDouble(carpetarea.LengthInche);
            else
                lengthInch = Convert.ToDouble(0.0);

            if (carpetarea.BreadthFeet != null)
                breadthFeet = Convert.ToDouble(carpetarea.BreadthFeet);
            else
                breadthFeet = Convert.ToDouble(0.0);

            if (carpetarea.BreadthInches != null)
                breadthInch = Convert.ToDouble(carpetarea.BreadthInches);
            else
                breadthInch = Convert.ToDouble(0.0);

            if (lengthInch > 0)
                lengthInch = lengthInch * 0.0833333;

            length = lengthFeet + lengthInch;
            length = Math.Round(length, 3);

            if (breadthInch > 0)
                breadthInch = breadthInch * 0.0833333;

            breadth = breadthFeet + breadthInch;
            breadth = Math.Round(breadth, 3);

            Area = length * breadth;
            Area = Math.Round(Area, 3);

            //if (Session["CarpetArea"] != null)
            //{
            //    DataTable dtCurrentTable = (DataTable)Session["CarpetArea"];
            //    DataRow drCurrentRow = null;

            //    if (dtCurrentTable.Rows.Count > 0)
            //    {
            //        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
            //        {
            //            drCurrentRow = dtCurrentTable.NewRow();
            //            drCurrentRow["Type"] = carpetarea.lType;
            //            drCurrentRow["Length"] = length;
            //            drCurrentRow["Breadth"] = breadth;
            //            drCurrentRow["Area"] = Area;
            //        }
            //        if (dtCurrentTable.Rows[0][0].ToString() == "")
            //        {
            //            dtCurrentTable.Rows[0].Delete();
            //            dtCurrentTable.AcceptChanges();
            //        }

            //        dtCurrentTable.Rows.Add(drCurrentRow);
            //        Session["CarpetArea"] = dtCurrentTable;

            //        rptCarpetArea.DataSource = dtCurrentTable;
            //        rptCarpetArea.DataBind();
            //    }
            //}
        }

        #endregion Add new Record in Datatable

        #region Carpet Area

        private void CarpetArea(CarpetAreaCalculator carpetarea)
        {
            //if (Session["CarpetArea"] != null)
            //{
            //    DataTable dt = (DataTable)Session["CarpetArea"];
            //    Double sum = 0;

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (Convert.ToString(dr["Type"]) != "Duct" && Convert.ToString(dr["Type"]) != "Staircase" && Convert.ToString(dr["Type"]) != "Garden" && Convert.ToString(dr["Type"]) != "Passage" && Convert.ToString(dr["Type"]) != "Lift" && Convert.ToString(dr["Type"]) != "Lobby" && Convert.ToString(dr["Type"]) != "Gym" && Convert.ToString(dr["Type"]) != "Terrace" && Convert.ToString(dr["Type"]) != "Swiming Pool")
            //        {
            //            sum += Convert.ToDouble(dr["Area"]);
            //            sum = Math.Round(sum, 2);
            //        }
            //    }
            //    ViewBag.lblCarpetArea = sum.ToString();
            //}
        }

        #endregion Carpet Area

        #region Builtup Area

        private void BuiltupArea(CarpetAreaCalculator carpetarea)
        {
            double sum;
            ViewBag.lblBuiltupArea = Convert.ToString(Convert.ToDouble(ViewBag.lblCarpetArea) * 0.20);
            ViewBag.lblBuiltupArea = Convert.ToString(Convert.ToDouble(ViewBag.lblCarpetArea) + Convert.ToDouble(ViewBag.lblBuiltupArea));
            sum = Convert.ToDouble(ViewBag.lblBuiltupArea);
            sum = Math.Round(sum, 2);
            SuperBuiltupArea(sum);
        }

        #endregion Builtup Area

        #region ClearControl

        private void ClearControl(CarpetAreaCalculator carpetarea)
        {
            //carpetarea.LengthFeet = "";
            //carpetarea.LengthInche = "";
            //carpetarea.BreadthFeet = "";
            //carpetarea.BreadthInches = "";
            //carpetarea.lType.SelectedValue = null;
            //carpetarea.lType.Focus();
        }

        #endregion ClearControl

        #region Super Builtup Area

        private void SuperBuiltupArea(Double area)
        {
            //if (Session["CarpetArea"] != null)
            //{
            //    DataTable dt = (DataTable)Session["CarpetArea"];
            //    Double sum = 0;

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (Convert.ToString(dr["Type"]) != "Bedroom" && Convert.ToString(dr["Type"]) != "Living" && Convert.ToString(dr["Type"]) != "Dining" && Convert.ToString(dr["Type"]) != "Kitchen" && Convert.ToString(dr["Type"]) != "Bathroom")
            //        {
            //            sum += Convert.ToDouble(dr["Area"]);
            //            sum = Math.Round(sum, 2);
            //        }
            //    }
            //    Double superArea = sum + area;
            //   ViewBag.lblSuperBuiltupArea = superArea.ToString();
            //}
        }

        #endregion Super Builtup Area




    }
}
