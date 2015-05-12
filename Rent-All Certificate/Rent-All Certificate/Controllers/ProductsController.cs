using System.Collections.Generic;
using System.Data.Entity;
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
        private RentAllEntities db = new RentAllEntities();

        // GET: Products
        public ActionResult Index(int? page, int? selectedCategory)
        {
            var model = new ProductIndexModel
            {
                CategorySelectList = new CategoriesController().GetCategorySelectLists(selectedCategory),
                SelectedCategory = selectedCategory
            };
            if (selectedCategory == null)
            {
                model.ProductTabelList =
                    db.Product.Include(p => p.Category)
                        //.Include(p => p.Hoist)
                        //.Include(p => p.Manufacturer)
                        //.Include(p => p.Phase)
                        .ToList()
                        .ToPagedList(page ?? 1, 20);
            }
            else
            {
                //Get all selected categories from the selected categories ids
                var selectedCategoryIds = model.CategorySelectList.Select(csl => csl.SelectedCategoryId).Distinct().ToList();
                //Get all possible subcategories
                selectedCategoryIds.AddRange(GetAllSubCategorieIds((int)selectedCategory));
                //Get all products with one of the selected categorie ids
                model.ProductTabelList =
                    db.Product.Where(p => selectedCategoryIds.Contains(p.CategoryID))
                        //.Include(p => p.Hoist)
                        //.Include(p => p.Manufacturer)
                        //.Include(p => p.Phase)
                        .ToList()
                        .ToPagedList(page ?? 1, 20);
            }
            return View(model);
        }

        public PartialViewResult UpdateProductIndex(int selectedCategory)
        {
            // Create model and add the tabel data
            var catController = new CategoriesController();
            var model = new ProductIndexModel
            {
                SelectedCategory = selectedCategory,
                CategorySelectList = catController.GetCategorySelectLists(selectedCategory)
            };

            //Get all selected categories from the selected categories ids
            var selectedCategoryIds = model.CategorySelectList.Select(csl => csl.SelectedCategoryId).Distinct().ToList();
            //Get all possible subcategories
            selectedCategoryIds.AddRange(GetAllSubCategorieIds(selectedCategory));
            //Get all products with one of the selected categorie ids
            model.ProductTabelList = db.Product.Where(p => selectedCategoryIds.Contains(p.CategoryID))
                //.Include(p => p.Hoist)
                //.Include(p => p.Manufacturer)
                //.Include(p => p.Phase)
                .ToList()
                .ToPagedList(1, 20);

            return PartialView("_productIndex", model);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var model = new ProductEditModel
            {
                CategorySelectList = new CategoriesController().GetCategorySelectLists(null)
            };
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer, "ManufacturerID", "ManufacturerName");
            ViewBag.PhaseID = new SelectList(db.Phase, "PhaseID", "PhaseName");
            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductEditModel model)
        {
            ValidateProduct(model.Product);
            if (ModelState.IsValid)
            {
                db.Product.Add(model.Product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            model.CategorySelectList = new CategoriesController().GetCategorySelectLists(model.Product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer, "ManufacturerID", "ManufacturerName", model.Product.ManufacturerID);
            ViewBag.PhaseID = new SelectList(db.Phase, "PhaseID", "PhaseName", model.Product.PhaseID);
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
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer, "ManufacturerID", "ManufacturerName", product.ManufacturerID);
            ViewBag.PhaseID = new SelectList(db.Phase, "PhaseID", "PhaseName", product.PhaseID);
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
            ViewBag.ManufacturerID = new SelectList(db.Manufacturer, "ManufacturerID", "ManufacturerName", model.Product.ManufacturerID);
            ViewBag.PhaseID = new SelectList(db.Phase, "PhaseID", "PhaseName", model.Product.PhaseID);
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
