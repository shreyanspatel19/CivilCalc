using Microsoft.AspNetCore.Mvc;
using CivilCalc.DAL;
using CivilCalc.Areas.CAL_Category.Models;
using AutoMapper;
using CivilCalc.DAL.CAL.CAL_Category;

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
        public IActionResult _SearchResult(SEC_UserModel objCategoryModel)
        {
            var vModel = DBConfig.dbCALCategory.SelectByCategoryNameUserName(objCategoryModel.F_CategoryName, objCategoryModel.F_UserName).ToList();
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

                var vCategoryModel = DBConfig.dbCALCategory.SelectPK(CategoryID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<SelectPK_Result, SEC_UserModel>());
                var vModel = AutoMapper.Mapper.Map<SelectPK_Result, SEC_UserModel>(vCategoryModel);

                return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(SEC_UserModel objCategoryModel)
        {            
            if (objCategoryModel.CategoryID == 0)
            {                
                var vReturn = DBConfig.dbCALCategory.Insert(objCategoryModel);
            }
            else
            {
                DBConfig.dbCALCategory.Update(objCategoryModel);
            }
            return Content(null);
        }
        #endregion        

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int CategoryID)
        {
            DBConfig.dbCALCategory.Delete(CategoryID);
            return Content(null);
        }
        #endregion
    }
}
