using Microsoft.AspNetCore.Mvc;
using CivilCalc.DAL;
using CivilCalc.Areas.SEC_User.Models;
using AutoMapper;

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

    }
}
