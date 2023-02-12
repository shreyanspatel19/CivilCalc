﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult _SearchResult(CAL_CategoryModel objCategoryModel)
        {
            var vModel = DBConfig.dbCAL.SelectByCategoryNameUserName(objCategoryModel.F_CategoryName, objCategoryModel.F_UserName).ToList();
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

                var vCategoryModel = DBConfig.dbCAL.SelectPK(CategoryID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<SelectPK_Result, CAL_CategoryModel>());
                var vModel = AutoMapper.Mapper.Map<SelectPK_Result, CAL_CategoryModel>(vCategoryModel);

                return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(CAL_CategoryModel objCategoryModel)
        {            
            if (objCategoryModel.CategoryID == 0)
            {                
                var vReturn = DBConfig.dbCAL.Insert(objCategoryModel);
            }
            else
            {
                DBConfig.dbCAL.Update(objCategoryModel);
            }
            return Content(null);
        }
        #endregion        

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int CategoryID)
        {
            DBConfig.dbCAL.Delete(CategoryID);
            return Content(null);
        }
        #endregion
    }
}
