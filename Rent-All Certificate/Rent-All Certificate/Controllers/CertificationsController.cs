using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Rent_All_Certificate.Models;

namespace Rent_All_Certificate.Controllers
{
    public class CertificationsController : Controller
    {
        private RentAllEntities db = new RentAllEntities();

        // GET: Certifications/Create
        public ActionResult Index()
        {
            ViewBag.BranchID = new SelectList(db.Branch, "BranchID", "BranchName");
            return View();
        }

        // POST: Certifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CertificationsUploadModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in model.Certificates)
                {
                    if (file.ContentType != "application/pdf")
                    {
                        ModelState.AddModelError("", "The file :\"" + file.FileName + "\" isn't recognized as a pdf file.");
                        continue;
                    }
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    if (fileNameWithoutExtension != null)
                    {
                        var keys = fileNameWithoutExtension.Split('-');

                        if (keys.Count() != 2)
                        {
                            ModelState.AddModelError("", "The file :\"" + file.FileName + "\" has an incorrect file name.");
                            continue;
                        }

                        int inventoryId;
                        var productKey = keys[0];

                        if (db.Product.Any(p => p.ProductKey == productKey) == false)
                        {
                            ModelState.AddModelError("", "The key :\"" + productKey + "\" was not found.");
                            continue;
                        }

                        bool parsed = Int32.TryParse(keys[1], out inventoryId);

                        if (parsed == false)
                        {
                            ModelState.AddModelError("", "The inventory id from file :\"" + file.FileName + "\" is not a number.");
                            continue;
                        }

                        if (db.Inventories.Any(i => i.ProductKey == productKey && i.InventoryID == inventoryId) == false)
                        {
                            db.Inventories.Add(new Inventory
                            {
                                InventoryID = inventoryId,
                                ProductKey = productKey
                            });
                            db.SaveChanges();
                        }

                        var fileName = productKey + "-" + inventoryId + ".pdf";
                        var path = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                        file.SaveAs(path);

                        model.Certification.InventoryID = inventoryId;
                        model.Certification.ProductKey = productKey;
                        model.Certification.EmployeeID = (int)Session["EmployeeID"];
                        model.Certification.Date = DateTime.Now;
                        db.Certifications.Add(model.Certification);
                        db.SaveChanges();
                    }
                }
            }
            if (ModelState.IsValid)
            {
                RedirectToAction("Index");
            }
            model.Certificates = null;
            ViewBag.BranchID = new SelectList(db.Branch, "BranchID", "BranchName", model.Certification.BranchID);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
