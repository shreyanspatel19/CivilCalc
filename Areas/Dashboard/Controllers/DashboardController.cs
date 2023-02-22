using Microsoft.AspNetCore.Mvc;
using CivilCalc.BAL;
namespace CivilCalc.Areas.Dashboard.Controllers
{
    [CheckAccess]
    [Area("Dashboard")]
    public class DashboardController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
