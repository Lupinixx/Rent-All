using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Helpers;
using Rent_All_Certificate.Attributes;
using Rent_All_Certificate.Models;

namespace Rent_All_Certificate.Controllers
{
    [LoginValidRole(ValidRoleId = new[] { Roles.TechnicalStaff, Roles.TechnicalAdministrator })]
    public class CertificationsController : Controller
    {
        private RentAllEntities db = new RentAllEntities();

        // GET: Certifications/Create
        public ActionResult NEN3140()
        {
            ViewBag.BranchID = new SelectList(db.Branch, "BranchID", "BranchName");
            return View();
        }

        // POST: Certifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NEN3140(CertificationsUploadModel model)
        {
            if (ModelState.IsValid)
            {
                var fileSuccessCount = 0;
                foreach (var file in model.Certificates)
                {
                    if (file == null)
                    {
                        continue;
                    }
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

                        if (db.Inventory.Any(i => i.ProductKey == productKey && i.InventoryID == inventoryId) == false)
                        {
                            db.Inventory.Add(new Inventory
                            {
                                InventoryID = inventoryId,
                                ProductKey = productKey
                            });
                            db.SaveChanges();
                        }

                        var fileName = productKey + "-" + inventoryId + ".pdf";
                        var path = Path.Combine(Server.MapPath("~/Content/Uploads/NEN3140"), fileName);
                        file.SaveAs(path);

                        model.Certification.InventoryID = inventoryId;
                        model.Certification.CertificateTypeID = CertificationTypes.NEN3140;
                        model.Certification.ProductKey = productKey;
                        model.Certification.EmployeeID = (int)Session["EmployeeID"];
                        model.Certification.Date = DateTime.Now;
                        db.Certification.Add(model.Certification);
                        db.SaveChanges();

                        fileSuccessCount++;
                    }
                }
                ViewBag.FileSuccessCount = fileSuccessCount;
            }
            if (ModelState.IsValid)
            {
                ViewBag.UploadMessage = "Your files have successfully been uploaded.";
                RedirectToAction("NEN3140");
            }
            model.Certificates = null;
            ViewBag.BranchID = new SelectList(db.Branch, "BranchID", "BranchName", model.Certification.BranchID);
            return View(model);
        }

        public ActionResult Hoist()
        {
            ViewBag.BranchID = new SelectList(db.Branch, "BranchID", "BranchName");
            return View();
        }

        // POST: Certifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Hoist(CertificationsUploadModel model)
        {
            if (ModelState.IsValid)
            {
                var fileSuccessCount = 0;
                foreach (var file in model.Certificates)
                {
                    if (file == null)
                    {
                        continue;
                    }
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

                        if (db.Inventory.Any(i => i.ProductKey == productKey && i.InventoryID == inventoryId) == false)
                        {
                            db.Inventory.Add(new Inventory
                            {
                                InventoryID = inventoryId,
                                ProductKey = productKey
                            });
                            db.SaveChanges();
                        }

                        var fileName = productKey + "-" + inventoryId + ".pdf";
                        var path = Path.Combine(Server.MapPath("~/Content/Uploads/Hoist"), fileName);
                        file.SaveAs(path);

                        model.Certification.InventoryID = inventoryId;
                        model.Certification.ProductKey = productKey;
                        model.Certification.CertificateTypeID = CertificationTypes.Hoist;
                        model.Certification.EmployeeID = (int)Session["EmployeeID"];
                        model.Certification.Date = DateTime.Now;
                        try
                        {
                            db.Certification.Add(model.Certification);
                            db.SaveChanges();
                            fileSuccessCount++;
                        }
                        catch (DbUpdateException dbex)
                        {
                            var ex = (SqlException)dbex.InnerException.InnerException;
                            foreach (SqlError error in ex.Errors)
                            {
                                if (error.Number == 50000)
                                {
                                    ModelState.AddModelError("", error.Message + " Filename: " + fileName);
                                }
                            }
                        }
                    }
                }
                ViewBag.FileSuccessCount = fileSuccessCount;
            }
            if (ModelState.IsValid)
            {
                ViewBag.UploadMessage = "Your files have successfully been uploaded.";
                RedirectToAction("Hoist");
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
