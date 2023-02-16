using Microsoft.AspNetCore.Mvc;
using CivilCalc.DAL;
using CivilCalc.Areas.MST_Configuration.Models;
using AutoMapper;

namespace CivilCalc.Areas.MST_Configuration.Controllers
{
    [Area("MST_Configuration")]
    public class MST_ConfigurationController : Controller
    {
        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

    }
}
