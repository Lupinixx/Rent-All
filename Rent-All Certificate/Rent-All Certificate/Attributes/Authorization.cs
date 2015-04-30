using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rent_All_Certificate.Attributes
{
    /// <summary>
    /// Checks user IS NOT logged in. If logged in, redirects to home-page.
    /// </summary>
    public class LoginInvalid : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.HttpContext.Session != null && filterContext.HttpContext.Session["id"] != null) {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {
                    controller = "Home",
                    action = "Index"
                }));
            }
        }
    }

    /// <summary>
    /// Checks user IS logged in and has the right role. If not, redirects to login-page.
    /// Note: If user is logged in but has no permissions, the login-page will redirect to home-page.
    /// </summary>
    public class LoginValidRole : ActionFilterAttribute
    {
        public int[] ValidRoleId { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            if (filterContext.HttpContext.Session != null
                && filterContext.HttpContext.Session["id"] != null
                && ValidRoleId.Contains(Convert.ToInt32(filterContext.HttpContext.Session["role"]))) {
                return;
            }

            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {
                controller = "Account",
                action = "Login"
            }));

        }
    }
}