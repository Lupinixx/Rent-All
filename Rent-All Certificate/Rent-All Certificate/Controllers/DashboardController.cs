using System.Web.Mvc;
using Helpers;
using Rent_All_Certificate.Attributes;

namespace Rent_All_Certificate.Controllers
{
    [LoginValidRole(ValidRoleId = new[] { Roles.TechnicalStaff, Roles.TechnicalAdministrator })]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manual()
        {
            return View();
        }
    }
}