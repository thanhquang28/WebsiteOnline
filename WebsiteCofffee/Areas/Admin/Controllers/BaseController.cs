using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebsiteCofffee.Common;
using Model.EF;

namespace WebsiteCofffee.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }
        
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type=="Success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type=="Warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type=="Error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}