using System;
using System.Collections.Generic;
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
    [LoginInvalid]
    public class HomeController : Controller
    {
        private RentAllEntities db = new RentAllEntities();

        public ActionResult Index() {
            return View();
        }

        public ActionResult Products(int? page, int? selectedCategory, string search)
        {
            var model = new ProductIndexModel
            {
                CategorySelectList = new CategoriesController().GetCategorySelectLists(selectedCategory),
                SelectedCategory = selectedCategory
            };

            if (selectedCategory == null)
            {
                if (search == null)
                {
                    model.ProductTabelList =
                        db.Product.Include(p => p.Category).Include(p => p.Manufacturer)
                            .OrderBy(p => p.ProductKey)
                            .ToList()
                            .ToPagedList(page ?? 1, Pagenumber.MaxResults);
                }
                else
                {
                    model.ProductTabelList =
                            db.Product.Include(p => p.Category).Include(p => p.Manufacturer)
                                .Where(x => x.ProductKey.StartsWith(search))
                                .OrderBy(p => p.ProductKey)
                                .ToList()
                                .ToPagedList(page ?? 1, Pagenumber.MaxResults);
                }

            }
            else
            {
                var selectedCategoryIds = GetAllSubCategorieIds(selectedCategory);
                selectedCategoryIds.Add(selectedCategory);

                model.ProductTabelList =
                    db.Product.Where(p => selectedCategoryIds.Contains(p.CategoryID)).Include(p => p.Manufacturer)
                        .OrderBy(p => p.ProductKey)
                        .ToList()
                        .ToPagedList(page ?? 1, Pagenumber.MaxResults);
            }
            return View(model);
        }


        public ActionResult Inventory(string product, int? inventory, int? page )
        {
            if (product != null && inventory == null)
            {
                return View(db.Certification.Where(c => c.ProductKey == product)
                                        .OrderBy(c => c.InventoryID)
                                        .ToList()
                                        .ToPagedList(page ?? 1, Pagenumber.MaxResults));  
            }
            else if (product != null && inventory != null)
            {
                return View(db.Certification.Where(c => c.ProductKey == product)
                                        .Where(c => c.InventoryID == inventory)
                                        .OrderBy(c => c.InventoryID)
                                        .ToList()
                                        .ToPagedList(page ?? 1, Pagenumber.MaxResults));
            }
            else
            {
                return View(db.Certification.OrderBy(c => c.InventoryID).ToList().ToPagedList(page ?? 1, Pagenumber.MaxResults));
            }
           
        }

        public ActionResult search(string searchkey, int? searchid)
        {
            if (searchkey == null && searchid == null)
            {
                return RedirectToAction("Index");
            }
            else if (searchkey != null && searchid == null)
            {
                return RedirectToAction("Products", new{search = searchkey});
            }
            else
            {
                return RedirectToAction("Inventory", new{product = searchkey, inventory = searchid});
            }
        }


        private List<int?> GetAllSubCategorieIds(int? catId)
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

    }
}