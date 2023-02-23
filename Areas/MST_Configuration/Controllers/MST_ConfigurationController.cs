using Microsoft.AspNetCore.Mvc;
using CivilCalc.DAL;
using CivilCalc.BAL;
using CivilCalc.Areas.MST_Configuration.Models;
using AutoMapper;
using CivilCalc.DAL.MST.MST_Configuration;


namespace CivilCalc.Areas.MST_Configuration.Controllers
{
    [CheckAccess]
    [Area("MST_Configuration")]
    public class MST_ConfigurationController : Controller
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
        public IActionResult _SearchResult(MST_ConfigurationModel obj_MST_Configuration)
        {
            var vModel = DBConfig.dbMSTConfiguration.SelectAll().ToList();
            return PartialView("_List",vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult AddEdit(int? ConfigurationID)
        {
            ViewBag.Action = "Add";

            if (ConfigurationID != null)
            {
                ViewBag.Action = "Edit";

                var vUserModel = DBConfig.dbMSTConfiguration.SelectPK(ConfigurationID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<SelectPK_Result, MST_ConfigurationModel>());
                var vModel = AutoMapper.Mapper.Map<SelectPK_Result, MST_ConfigurationModel>(vUserModel);

                return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(MST_ConfigurationModel obj_MST_Configuration)
        {
            if (obj_MST_Configuration.ConfigurationID == 0)
            {
                var vReturn = DBConfig.dbMSTConfiguration.Insert(obj_MST_Configuration);
            }
            else
            {
                DBConfig.dbMSTConfiguration.Update(obj_MST_Configuration);
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int ConfigurationID)
        {
            DBConfig.dbMSTConfiguration.Delete(ConfigurationID);
            return Content(null);
        }
        #endregion

    }
}
