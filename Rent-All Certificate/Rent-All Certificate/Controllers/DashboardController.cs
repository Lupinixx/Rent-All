using Rent_All_Certificate.Models;
using System.Web.Mvc;
using System.Linq;

namespace Rent_All_Certificate.Controllers
{
    public class DashboardController : Controller
    {
        private RentAllEntities db = new RentAllEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            DashboardModel model = new DashboardModel
            {
                ProductsCount = db.Product.Select(p => p.ProductKey).Count(),
                InvertoryCount = 0,
                CategoryCount = db.Category.Select(c => c.CategoryID).Count(),
                ManufactureCount = db.Manufacturer.Select(m => m.ManufacturerID).Count()
            };
            return View(model);
        }

        public ActionResult Manual()
        {
            return View();
        }
    }
}