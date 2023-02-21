using Microsoft.AspNetCore.Mvc;
using CivilCalc.DAL;
using CivilCalc.Areas.SEC_User.Models;
using AutoMapper;
using CivilCalc.DAL.SEC.SEC_User;
using System.Data;

namespace CivilCalc.Areas.SEC_User.Controllers
{
    [Area("SEC_User")]
    public class SEC_UserController : Controller
    {
        #region Index
        public IActionResult Index()
        {
            
            return View();
        }
        #endregion
        #region pageLogin
        public IActionResult Login()
        {
            return View("_Login");
        }

            #endregion
            #region _Login
            public IActionResult _Login(string Email, string Password)
        {
            
            DataTable dt = DBConfig.dbSECUser.SelectUserNamePassword(Email, Password);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                    HttpContext.Session.SetString("DisplayName", dr["DisplayName"].ToString());
                    HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                    break;
                }
            }
            else
            {
                TempData["Error"] = "User Name or Password is invalid!";
                return RedirectToAction("Index");
            }
            if (HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("Password") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
        #endregion


        #region _SearchResult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _SearchResult(SEC_UserModel obj_SEC_User)
        {
            var vModel = DBConfig.dbSECUser.SelectForSearch(obj_SEC_User.UserID).ToList();
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
        public IActionResult _Save(SEC_UserModel obj_SEC_User)
        {
            if (obj_SEC_User.UserID == 0)
            {
                var vReturn = DBConfig.dbSECUser.Insert(obj_SEC_User);
            }
            else
            {
                DBConfig.dbSECUser.Update(obj_SEC_User);
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
