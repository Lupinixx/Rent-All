using System.Web.Mvc;

namespace Rent_All_Certificate.Controllers
{
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