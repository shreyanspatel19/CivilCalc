using CivilCalc.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Data.Common;

namespace CivilCalc.Areas.CAL_Category.Controllers
{
    [Area("CAL_Category")]
    public class CAL_CategoryController : Controller
    {
        private IConfiguration configuration;

        public CAL_CategoryController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Index()
        {
            string str = this.configuration.GetConnectionString("myconnectionstring");
            //CAL_DAL dal=  new CAL_DAL();
            //DataTable dataTable = dal.dbo_PR_CAL_Catagory_SelectAll(str);
            SqlDatabase SQLDB = new SqlDatabase(str);
            DbCommand dbCommand = SQLDB.GetStoredProcCommand("PR_CAL_Category_SelectAll");
            DataTable dataTable = new DataTable();
            using (IDataReader dr = SQLDB.ExecuteReader(dbCommand))
            {
                dataTable.Load(dr);
            }
            return View("CategoryList",dataTable);
        }
        public IActionResult AddEditCategory()
        {
            return View();
        }
    }
}
