﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Rent_All_Certificate.Models;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using Helpers;
using Rent_All_Certificate.Attributes;

namespace Rent_All_Certificate.Controllers
{
    [LoginValidRole(ValidRoleId = new[]{Roles.TechnicalStaff, Roles.TechnicalAdministrator})]
    public class CategoriesController : Controller
    {
        private RentAllUserEntities db = new RentAllUserEntities();

        // GET: Categories
        public ActionResult Index()
        {
            var cim = new CategoryIndexModel
            {
                CategorieTabelList = null,
                CategorySelectList = new List<CategorySelectListModel>
                {
                    new CategorySelectListModel
                    {
                        CategoriesSelectList = db.Category.Where(c => c.ParentID == null).ToList(),
                        SelectedCategoryId = 0
                    }
                }
            };
            //db.Category.Include(c => c.Category2).ToList();
            return View(cim);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.ParentID = new SelectList(db.Category, "CategoryID", "CategoryName");
            var model = new CategoryEditModel
            {
                CategorySelectList = new List<CategorySelectListModel>
                {
                    new CategorySelectListModel
                    {
                        CategoriesSelectList = db.Category.Where(c => c.ParentID == null).ToList(),
                        SelectedCategoryId = 0
                    }
                }
            };
            return View(model);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Category.Add(model.Category);
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
            model.CategorySelectList = GetCategorySelectLists(model.Category.ParentID);
            return View(model);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var categoryEditModel = new CategoryEditModel
            {
                Category = category,
                CategorySelectList = GetCategorySelectLists(category.ParentID, category.CategoryID)
            };
            return View(categoryEditModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model.Category).State = EntityState.Modified;
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
            model.CategorySelectList = GetCategorySelectLists(model.Category.ParentID, model.Category.CategoryID);
            return View(model);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            if (category.Product.Count == 0 && category.Category1.Count == 0)
            {
                db.Category.Remove(category);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult UpdateCategoryList(int SelectedCategory)
        {
            var cslm = new CategoryIndexModel
            {
                StarterCategoryId = db.Category.First(c => c.CategoryID == SelectedCategory).ParentID,
                CategorieTabelList = GetAllSubCategoriesFromParent(SelectedCategory),
                CategorySelectList = GetCategorySelectLists(SelectedCategory)
            };
            //Add parent (GetAllSubCategoriesFromParent() only gets the subs)
            cslm.CategorieTabelList.AddRange(db.Category.Where(c => c.CategoryID == SelectedCategory).Include(c => c.Category2).ToList());

            return PartialView("_categoryList", cslm);
        }

        public PartialViewResult UpdateEditCategoryDropDownLists(int SelectedCategory, int exclude=0)
        {
            return PartialView("_categoryDropDownLists", GetCategorySelectLists(SelectedCategory, exclude));
        }

        public List<CategorySelectListModel> GetCategorySelectLists(int? categoryId, int exclude=0)
        {
            var categorySelectLists = new List<CategorySelectListModel>();
            //Check of categoryId bestaat
            if (categoryId != null && db.Category.Any(c => c.CategoryID == categoryId) == false)
            {
                categoryId = null;
            }
            //haal de volgende lijst op
            var temp = new CategorySelectListModel
            {
                CategoriesSelectList = db.Category.Where(c => c.ParentID == categoryId && c.CategoryID != exclude).Include(c => c.Category2).ToList(),
                SelectedCategoryId = 0
            };
            if (temp.CategoriesSelectList.Count > 0)
            {
                categorySelectLists.Add(temp);
            }
            // haal de eerder geselecteerde items met lijst op
            if (categoryId != null)
            {
                do
                {
                    int? selected = categoryId;
                    categoryId = db.Category.SingleOrDefault(c => c.CategoryID == categoryId).ParentID;
                    var id = categoryId;
                    categorySelectLists.Insert(0,
                        new CategorySelectListModel
                        {
                            //haal de volgende lijst op
                            CategoriesSelectList =
                                db.Category.Where(c => c.ParentID == id && c.CategoryID != exclude).Include(c => c.Category2).ToList(),
                            SelectedCategoryId = selected
                        });
                } while (categoryId != null);
            }
            return categorySelectLists;
        }

        private List<Category> GetAllSubCategoriesFromParent(int parentId)
        {
            var categoryList = db.Category.Where(c => c.ParentID == parentId).Include(c => c.Category2).ToList();
            var categoryList2 = new List<Category>();
            foreach (Category cat in categoryList)
            {
                if (db.Category.Count(c => c.ParentID == cat.CategoryID) > 0)
                {
                    categoryList2.AddRange(GetAllSubCategoriesFromParent(cat.CategoryID));
                }
            }
            categoryList.AddRange(categoryList2);
            return categoryList;
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
