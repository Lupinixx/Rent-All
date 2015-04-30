using System.Web.Mvc;

namespace Rent_All_Certificate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() {
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LogOff() {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}