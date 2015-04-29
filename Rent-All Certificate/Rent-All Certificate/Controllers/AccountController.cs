using System.Web.Mvc;

namespace Rent_All_Certificate.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public ActionResult Login() {
            return View();
        }
    }
}