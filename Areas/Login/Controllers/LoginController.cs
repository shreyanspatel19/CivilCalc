using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.Login.Controllers
{
    [Area("Login")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
