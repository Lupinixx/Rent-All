using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Helpers;
using Rent_All_Certificate.Models;
using PagedList;
using Rent_All_Certificate.Attributes;

namespace Rent_All_Certificate.Controllers
{
    [LoginValidRole(ValidRoleId = new[] { Roles.TechnicalStaff, Roles.TechnicalAdministrator })]
    public class ManufacturersController : Controller
    {
        private RentAllEntities db = new RentAllEntities();

        // GET: Manufacturers
        public ActionResult Index(string search, int? page)
        {
            if (search == null)
            {
                return View(db.Manufacturer
                    .ToList().ToPagedList(page ?? 1, Pagenumber.MaxResults));
            }
            else {
                return View(db.Manufacturer.Where(x => x.ManufacturerName.StartsWith(search))
                    .ToList().ToPagedList(page ?? 1, Pagenumber.MaxResults));
            }
        }

        // GET: Manufacturers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManufacturerID,ManufacturerName")] Manufacturer manufacturer)
        {
            ValidateManufacturer(manufacturer);
            if (ModelState.IsValid)
            {
                db.Manufacturer.Add(manufacturer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manufacturer);
        }

        // GET: Manufacturers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacturer manufacturer = db.Manufacturer.Find(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }
            return View(manufacturer);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManufacturerID,ManufacturerName")] Manufacturer manufacturer)
        {
            ValidateManufacturer(manufacturer);
            if (ModelState.IsValid)
            {
                db.Entry(manufacturer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufacturer);
        }

        // GET: Manufacturers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacturer manufacturer = db.Manufacturer.Find(id);
            if (manufacturer == null)
            {
                return HttpNotFound();
            }
            if (manufacturer.Product.Count == 0)
            {
                db.Manufacturer.Remove(manufacturer);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manufacturer manufacturer = db.Manufacturer.Find(id);
            db.Manufacturer.Remove(manufacturer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ValidateManufacturer(Manufacturer manufacturer)
        {
            if (db.Manufacturer.Any(m => m.ManufacturerName.Equals(manufacturer.ManufacturerName) && m.ManufacturerID != manufacturer.ManufacturerID))
                ModelState.AddModelError("", "Manufacturer must have a unique name.");
        }
    }
}
