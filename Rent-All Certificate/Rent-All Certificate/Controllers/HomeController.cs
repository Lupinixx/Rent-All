using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Helpers;
using Rent_All_Certificate.Attributes;
using Rent_All_Certificate.Models;

namespace Rent_All_Certificate.Controllers
{
    [LoginInvalid]
    public class HomeController : Controller
    {
        public ActionResult Index() {
            return View();
        }

    }
}