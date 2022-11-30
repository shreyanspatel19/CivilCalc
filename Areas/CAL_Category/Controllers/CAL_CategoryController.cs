using Microsoft.AspNetCore.Mvc;

namespace CivilCalc.Areas.CAL_Category.Controllers
{
    [Area("CAL_Category")]
    public class CAL_CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddEditCategory()
        {
            return View();
        }
    }
}
