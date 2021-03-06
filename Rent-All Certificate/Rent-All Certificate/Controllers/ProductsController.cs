﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Helpers;
using PagedList;
using Rent_All_Certificate.Attributes;
using Rent_All_Certificate.Models;

namespace Rent_All_Certificate.Controllers
{
    [LoginValidRole(ValidRoleId = new[] { Roles.TechnicalStaff, Roles.TechnicalAdministrator })]
    public class ProductsController : Controller
    {
        private RentAllUserEntities db = new RentAllUserEntities();

        // GET: Products
        public ActionResult Index(int? page, int? selectedCategory, string search = "")
        {
            var model = new ProductIndexModel
            {
                CategorySelectList = new CategoriesController().GetCategorySelectLists(selectedCategory),
                SelectedCategory = selectedCategory,
                Search = search
            };
            if (selectedCategory == null)
            {
                model.ProductTabelList =
                        db.Product.Include(p => p.Category)
                            .Where(x => x.ProductKey.StartsWith(search))
                            .ToList()
                            .ToPagedList(page ?? 1, Pagenumber.MaxResults);

            }
            else
            {
                //Get all possible subcategories
                var selectedCategoryIds = GetAllSubCategorieIds((int)selectedCategory);
                //Get all products with one of the selected categorie ids
                selectedCategoryIds.Add((int)selectedCategory);

                model.ProductTabelList =
                    db.Product.Where(p => selectedCategoryIds.Contains(p.CategoryID) && p.ProductKey.StartsWith(search))
                        .ToList()
                        .ToPagedList(page ?? 1, Pagenumber.MaxResults);
            }
            return View(model);
        }

        public PartialViewResult UpdateProductIndex(int selectedCategory, string search = "")
        {
            // Create model and add the tabel data
            var catController = new CategoriesController();
            var model = new ProductIndexModel
            {
                SelectedCategory = selectedCategory,
                CategorySelectList = catController.GetCategorySelectLists(selectedCategory),
                Search = search
            };
            //Get all possible subcategories
            var selectedCategoryIds = GetAllSubCategorieIds(selectedCategory);
            selectedCategoryIds.Add(selectedCategory);
            //Get all products with one of the selected categorie ids
            model.ProductTabelList =
                db.Product.Where(p => selectedCategoryIds.Contains(p.CategoryID) && p.ProductKey.StartsWith(search))
                    .ToList()
                    .ToPagedList(1, Pagenumber.MaxResults);

            return PartialView("_productIndex", model);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var model = new ProductEditModel
            {
                CategorySelectList = new CategoriesController().GetCategorySelectLists(null)
            };
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer.OrderBy(m => m.ManufacturerName), "ManufacturerID", "ManufacturerName");
            ViewBag.PhaseID = new SelectList(db.Phase.OrderBy(p => p.PhaseName), "PhaseID", "PhaseName");
            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductEditModel model)
        {
            if (db.Product.Any(p => p.ProductKey.Equals(model.Product.ProductKey)))
                ModelState.AddModelError("", "Product must have a unique key.");

            ValidateProduct(model.Product);
            if (ModelState.IsValid)
            {
                try
                {
                    db.Product.Add(model.Product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException dbex)
                {
                    var ex = (SqlException)dbex.InnerException.InnerException;
                    foreach (SqlError error in ex.Errors)
                    {
                        if (error.Number == 50000)
                        {
                            ModelState.AddModelError("", error.Message);
                        }
                    }
                }
            }
            model.CategorySelectList = new CategoriesController().GetCategorySelectLists(model.Product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer.OrderBy(m => m.ManufacturerName), "ManufacturerID", "ManufacturerName", model.Product.ManufacturerID);
            ViewBag.PhaseID = new SelectList(db.Phase.OrderBy(p => p.PhaseName), "PhaseID", "PhaseName", model.Product.PhaseID);
            return View(model);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var model = new ProductEditModel
            {
                Product = product,
                CategorySelectList = new CategoriesController().GetCategorySelectLists(product.CategoryID)
            };
            //ViewBag.CategoryID = new SelectList(db.Category, "CategoryID", "CategoryName", product.CategoryID);
            //ViewBag.ProductKey = new SelectList(db.Hoist, "Productkey", "Description", product.ProductKey);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer.OrderBy(m => m.ManufacturerName), "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            ViewBag.PhaseID = new SelectList(db.Phase.OrderBy(p => p.PhaseName), "PhaseID", "PhaseName", product.PhaseID);
            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductEditModel model)
        {
            ValidateProduct(model.Product);
            if (ModelState.IsValid)
            {
                db.Entry(model.Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            model.CategorySelectList = new CategoriesController().GetCategorySelectLists(model.Product.CategoryID);
            //ViewBag.ProductKey = new SelectList(db.Hoist, "Productkey", "Description", model.Product.ProductKey);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer.OrderBy(m => m.ManufacturerName), "ManufacturerID", "ManufacturerName", model.Product.ManufacturerID);
            ViewBag.PhaseID = new SelectList(db.Phase.OrderBy(p => p.PhaseName), "PhaseID", "PhaseName", model.Product.PhaseID);
            return View(model);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            //if (product.Iventory.Count == 0)
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult InventoryIndex(string key, int? id, int? page)
        {
            if (key == null)
            {
                return View(db.Inventory.OrderBy(i => i.ProductKey).ToList().ToPagedList(page ?? 1, Pagenumber.MaxResults));
            }
            if (id != null)
            {
                return View(db.Inventory.Where(i => i.ProductKey == key).Where(i => i.InventoryID == id).ToList().ToPagedList(page ?? 1, Pagenumber.MaxResults));
            }
            else {
                return View(db.Inventory.Where(i => i.ProductKey == key).ToList().ToPagedList(page ?? 1, Pagenumber.MaxResults));
            }
        }

        public ActionResult CertificatesIndex(string key, int? invetory, int? page)
        {
            if (key == null || invetory == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(db.Certification.Where(c => c.ProductKey == key)
                                        .Where(c => c.InventoryID == invetory)
                                        .ToList()
                                        .ToPagedList(page ?? 1, Pagenumber.MaxResults));
        }
        public ActionResult CertificatesHistory(string key, int? invetory, int? type, int? page)
        {
            if (key == null || invetory == null || type == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(db.CertificationLog.Where(c => c.ProductKey == key)
                                        .Where(c => c.InventoryID == invetory)
                                        .Where(c => c.CertificateTypeID == type)
                                        .OrderBy(c => c.Date)
                                        .ToList()
                                        .ToPagedList(page ?? 1, Pagenumber.MaxResults));
        }

        public ActionResult ExpiredCertificates(int? page)
        {
            DateTime dt = DateTime.UtcNow.AddHours(2);
            var thisYear = new DateTime(dt.Year - 1, dt.Month + 1, dt.Day);

            return View(db.Certification.Where(c => c.Date < thisYear).Where(c => c.CertificateTypeID == CertificationTypes.NEN3140).ToList().ToPagedList(page ?? 1, Pagenumber.MaxResults));
        }

        public ActionResult ExpiredHoistCertificates(int? page)
        {
            DateTime dt = DateTime.UtcNow.AddHours(2);
            var thisYear = new DateTime(dt.Year - 1, dt.Month + 1, dt.Day);

            return View(db.Certification.Where(c => c.Date < thisYear).Where(c => c.CertificateTypeID == CertificationTypes.Hoist).ToList().ToPagedList(page ?? 1, Pagenumber.MaxResults));
        }


        public ActionResult DeleteInventory(int? id, string key)
        {
            if (id == null || key == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var inventory = db.Inventory.FirstOrDefault(i => i.ProductKey == key && i.InventoryID == id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            var pathNEN3140 = Server.MapPath("~/Content/Uploads/NEN3140/" + inventory.ProductKey + "-" + inventory.InventoryID + ".pdf");
            var pathHoists = Server.MapPath("~/Content/Uploads/Hoists/" + inventory.ProductKey + "-" + inventory.InventoryID + ".pdf");
            if (System.IO.File.Exists(pathNEN3140))
            {
                System.IO.File.Delete(pathNEN3140);
            }
            if (System.IO.File.Exists(pathHoists))
            {
                System.IO.File.Delete(pathHoists);
            }
            var certificates = db.Certification.Where(c => c.InventoryID == id && c.ProductKey == key).ToList();
            var certificatesLog = db.CertificationLog.Where(c => c.InventoryID == id && c.ProductKey == key).ToList();
            db.Certification.RemoveRange(certificates);
            db.CertificationLog.RemoveRange(certificatesLog);
            db.Inventory.Remove(inventory);
            db.SaveChanges();
            return RedirectToAction("InventoryIndex", new { key });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<int?> GetAllSubCategorieIds(int catId)
        {
            var catIdList = new List<int?>();
            foreach (var cat in db.Category.Where(c => c.ParentID == catId))
            {
                if (db.Category.Count(c => c.ParentID == cat.CategoryID) > 0)
                    catIdList.AddRange(GetAllSubCategorieIds(cat.CategoryID));

                catIdList.Add(cat.CategoryID);
            }
            return catIdList;
        }

        private void ValidateProduct(Product product)
        {
            if (db.Product.Any(p => p.ProductName.Equals(product.ProductName) && p.ProductKey != product.ProductKey))
                ModelState.AddModelError("", "Product must have a unique name.");
        }
    }
}
