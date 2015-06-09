using Rent_All_Certificate.Models;
using System.Web.Mvc;
using System.Linq;
using Rent_All_Certificate.Attributes;
using System.Web.Security;
using Helpers;
using Roles = Helpers.Roles;

namespace Rent_All_Certificate.Controllers
{
    [LoginValidRole(ValidRoleId = new[] { Roles.TechnicalStaff, Roles.TechnicalAdministrator })]
    public class DashboardController : Controller
    {
        private RentAllUserEntities db = new RentAllUserEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            DashboardModel model = new DashboardModel
            {
                ProductsCount = db.Product.Select(p => p.ProductKey).Count(),
                InventoryCount = db.Inventory.Select(i => i.InventoryID).Count(),
                CategoryCount = db.Category.Select(c => c.CategoryID).Count(),
                ManufactureCount = db.Manufacturer.Select(m => m.ManufacturerID).Count()
            };
            return View(model);
        }

        public ActionResult Manual()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}