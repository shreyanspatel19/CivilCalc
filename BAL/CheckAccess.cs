using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CivilCalc.BAL
{
    public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var rd = filterContext.RouteData;
            string currentAction = rd.Values["action"].ToString();
            string currentController = rd.Values["controller"].ToString();
            //string currentArea = rd.DataTokens["area"].ToString();

            if (filterContext.HttpContext.Session.GetString("UserID") != null)
            {
                filterContext.Result = new RedirectResult("~/Login/Login");
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            filterContext.HttpContext.Response.Headers["Expires"] = "-1";
            filterContext.HttpContext.Response.Headers["Pragma"] = "no-cache";
            base.OnResultExecuting(filterContext);
        }
    }
}
