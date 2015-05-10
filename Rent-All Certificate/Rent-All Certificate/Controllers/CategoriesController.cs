using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Rent_All_Certificate.Models;
using System.Collections.Generic;

namespace Rent_All_Certificate.Controllers
{
    //[LoginValidRole(ValidRoleId = new[]{Roles.TechnicalStaff, Roles.TechnicalAdministrator})]
    public class CategoriesController : Controller
    {
        private RentAllEntities db = new RentAllEntities();

        // GET: Categories
        public ActionResult Index()
        {
            var cim = new CategoryIndexModel();
            cim.CategorieTabelList = db.Category.Include(c => c.Category2).ToList();
            cim.CategorySelectList = new List<CategorySelectListModel>
            {
                new CategorySelectListModel
                {
                    CategoriesSelectList = db.Category.Where(c => c.ParentID == null).ToList(),
                    SelectedCategoryId = 0
                }
            };
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
                db.Category.Add(model.Category);
                db.SaveChanges();
                return RedirectToAction("Index");
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
                CategorySelectList = GetCategorySelectLists(category.ParentID)
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
                db.Entry(model.Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            model.CategorySelectList = GetCategorySelectLists(model.Category.ParentID);
            return View(model);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Category.Find(id);
            db.Category.Remove(category);
            db.SaveChanges();
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

        public PartialViewResult UpdateEditCategoryDropDownLists(int SelectedCategory)
        {
            return PartialView("_categoryDropDownLists", GetCategorySelectLists(SelectedCategory));
        }

        public List<CategorySelectListModel> GetCategorySelectLists(int? categoryId)
        {
            var categorySelectLists = new List<CategorySelectListModel>();
            //haal de volgende lijst op
            var temp = new CategorySelectListModel
            {
                CategoriesSelectList = db.Category.Where(c => c.ParentID == categoryId).Include(c => c.Category2).ToList(),
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
                                db.Category.Where(c => c.ParentID == id).Include(c => c.Category2).ToList(),
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
