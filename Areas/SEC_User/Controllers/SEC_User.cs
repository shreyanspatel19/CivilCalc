using Microsoft.AspNetCore.Mvc;
using CivilCalc.DAL;
using CivilCalc.Areas.SEC_User.Models;
using AutoMapper;
using CivilCalc.DAL.SEC.SEC_User;

namespace CivilCalc.Areas.SEC_User.Controllers
{
    [Area("SEC_User")]
    public class SEC_UserController : Controller
    {
        #region Index
        public IActionResult Index()
        {
            ViewBag.UserList = DBConfig.dbSECUser.SelectComboBoxUser().ToList();
            return View();
        }
        #endregion

        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(SEC_UserModel objUserModel)
        {
            var vModel = DBConfig.dbSECUser.SelectByUserID(objUserModel.UserID).ToList();
            return PartialView("_List", vModel);
        }
        #endregion

        #region _AddEdit
        public IActionResult _AddEdit(int? UserID)
        {
            ViewBag.Action = "Add";

            if (UserID != null)
            {
                ViewBag.Action = "Edit";

                var vUserModel = DBConfig.dbSECUser.SelectPK(UserID).SingleOrDefault();

                Mapper.Initialize(config => config.CreateMap<SelectPK_Result, SEC_UserModel>());
                var vModel = AutoMapper.Mapper.Map<SelectPK_Result, SEC_UserModel>(vUserModel);

                return PartialView(vModel);
            }
            return PartialView();
        }
        #endregion

        #region _Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Save(SEC_UserModel objUserModel)
        {
            if (objUserModel.UserID == 0)
            {
                var vReturn = DBConfig.dbSECUser.Insert(objUserModel);
            }
            else
            {
                DBConfig.dbSECUser.Update(objUserModel);
            }
            return Content(null);
        }
        #endregion        

        #region _Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Delete(int UserID)
        {
            DBConfig.dbSECUser.Delete(UserID);
            return Content(null);
        }
        #endregion

    }
}
