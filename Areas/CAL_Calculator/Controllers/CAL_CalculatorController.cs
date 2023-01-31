using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Drawing.Imaging;

namespace CivilCalc.Areas.CAL_Calculator.Controllers
{
    [Area("CAL_Calculator")]
    public class CAL_CalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(CAL_CalculatorModel d)
        {
            var vModel = DBConfig.dbCAL.dbo_PR_CAL_Calculator_SelectAll().ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult _AddEdit(int? CalculatorID)
        {
            ViewBag.Action = "Add";

            if (CalculatorID != null)
            {
                ViewBag.Action = "Edit";

                var d = DBConfig.dbCAL.dbo_PR_CAL_Calculator_SelectByPK(CalculatorID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<dbo_PR_CAL_Calculator_SelectByPK_Result, CAL_CalculatorModel>());
                var vModel = AutoMapper.Mapper.Map<dbo_PR_CAL_Calculator_SelectByPK_Result, CAL_CalculatorModel>(d);

                return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(CAL_CalculatorModel d)
        {
            if (d.CalculatorID == 0)
            {
                var vReturn = DBConfig.dbCAL.dbo_PR_CAL_Calculator_Insert(d.CategoryID, d.CalculatorName, d.CalculatorIcon, d.URLName, d.HeaderName, d.SubHeaderName, d.CalculatorDescription, d.PageSection1, d.PageSection2, d.PageSecton3, d.MetaTitle, d.MetaKeyword, d.MetaDescription, d.MetaAuthor, d.MetaOgTitle, d.MetaOgImage, d.MetaOgDescription, d.MetaOgUrl, d.MetaOgType, d.Sequence, d.Description, d.UserID);
            }
            else
            {
                DBConfig.dbCAL.dbo_PR_CAL_Calculator_UpdateByPK( d.CalculatorID, d.CategoryID, d.CalculatorName, d.CalculatorIcon, d.URLName, d.HeaderName, d.SubHeaderName, d.CalculatorDescription, d.PageSection1, d.PageSection2, d.PageSecton3, d.MetaTitle, d.MetaKeyword, d.MetaDescription, d.MetaAuthor, d.MetaOgTitle, d.MetaOgImage, d.MetaOgDescription, d.MetaOgUrl, d.MetaOgType, d.Sequence, d.Description, d.UserID);
            }
            return Content(null);
        }
        #endregion        

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int CalculatorID)
        {
            DBConfig.dbCAL.dbo_PR_CAL_Calculator_Delete(CalculatorID);
            return Content(null);
        }
        #endregion
    }
}
