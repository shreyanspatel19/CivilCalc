using Microsoft.AspNetCore.Mvc;
using CivilCalc.DAL;
using CivilCalc.Areas.CAL_Category.Models;
using AutoMapper;
using CivilCalc.DAL.CAL_Category;

namespace CivilCalc.Areas.CAL_Category.Controllers
{
    [Area("CAL_Category")]
    public class CAL_CategoryController : Controller
    {

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(CAL_CategoryModel d)
        {
            var vModel = DBConfig.dbCAL.dbo_PR_CAL_Category_SelectForSearch(d.C_CategoryName, d.C_UserName).ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult _AddEdit(int? CategoryID)
        {
            ViewBag.Action = "Add";

            if (CategoryID != null)
            {
                ViewBag.Action = "Edit";

                var d = DBConfig.dbCAL.dbo_PR_CAL_Category_SelectByPK(CategoryID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<dbo_PR_CAL_Category_SelectByPK_Result, CAL_CategoryModel>());
                var vModel = AutoMapper.Mapper.Map<dbo_PR_CAL_Category_SelectByPK_Result, CAL_CategoryModel>(d);

                return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(CAL_CategoryModel d)
        {
            if (d.CategoryID == 0)
            {
                var vReturn = DBConfig.dbCAL.dbo_PR_CAL_Category_Insert(d);
            }
            else
            {
                DBConfig.dbCAL.dbo_PR_CAL_Category_UpdateByPK(d.CategoryID, d.CategoryName, d.Description, d.Sequence, d.UserID);
            }
            return Content(null);
        }
        #endregion        

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int CategoryID)
        {
            DBConfig.dbCAL.dbo_PR_CAL_Category_Delete(CategoryID);
            return Content(null);
        }
        #endregion
    }
}
