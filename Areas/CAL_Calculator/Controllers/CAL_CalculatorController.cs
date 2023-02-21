﻿using AutoMapper;
using CivilCalc.Areas.CAL_Calculator.Models;
using CivilCalc.DAL;
using CivilCalc.DAL.CAL.CAL_Calculator;
using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.CAL_Calculator.Controllers
{
    [Area("CAL_Calculator")]
    public class CAL_CalculatorController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.CategoryList = DBConfig.dbCALCategory.SelectComboBoxCategory().ToList();
            ViewBag.CalculatorList = DBConfig.dbCALCalculator.SelectComboBoxUser().ToList();
            ViewBag.UserList = DBConfig.dbSECUser.SelectComboBoxUser().ToList();
            return View();
        }

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(CAL_CalculatorModel obj_CAL_Calculator)
        {
            
            var vModel = DBConfig.dbCALCalculator.SelectForSearch(obj_CAL_Calculator.CategoryID, obj_CAL_Calculator.CalculatorID, obj_CAL_Calculator.UserID).ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult _AddEdit(int? CalculatorID)
        {
            ViewBag.Action = "Add";
            ViewBag.CategoryList = DBConfig.dbCALCategory.SelectComboBoxCategory().ToList();

            if (CalculatorID != null)
            {
                ViewBag.Action = "Edit";

                var d = DBConfig.dbCALCalculator.SelectPK(CalculatorID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<SelectPK_Result, CAL_CalculatorModel>());
                var vModel = AutoMapper.Mapper.Map<SelectPK_Result, CAL_CalculatorModel>(d);

                 return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(CAL_CalculatorModel obj_CAL_Calculator)
        {
            if (obj_CAL_Calculator.File != null)
            {
                string FilePath = "wwwroot\\Upload\\Calculator";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNamewithPath = Path.Combine(path, obj_CAL_Calculator.File.FileName);
                obj_CAL_Calculator.CalculatorIcon = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + obj_CAL_Calculator.File.FileName;

                using (var stream = new FileStream(fileNamewithPath, FileMode.Create))
                {
                    obj_CAL_Calculator.File.CopyTo(stream);
                }
            }
            if (obj_CAL_Calculator.CalculatorID == 0)
            {
                var vReturn = DBConfig.dbCALCalculator.Insert(obj_CAL_Calculator);
            }
            else
            {
                DBConfig.dbCALCalculator.Update(obj_CAL_Calculator);
            }
            return Content(null);
        }
        #endregion        

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int CalculatorID)
        {
            DBConfig.dbCALCalculator.Delete(CalculatorID);
            return Content(null);
        }
        #endregion
    }
}
