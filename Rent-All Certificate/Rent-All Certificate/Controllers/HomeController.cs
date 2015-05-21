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
                        db.Product.Include(p => p.Category)
                            .ToList()
                            .ToPagedList(page ?? 1, 20);
                }
                else
                {
                    model.ProductTabelList =
                            db.Product.Include(p => p.Category)
                                .Where(x => x.ProductKey.StartsWith(search))
                                .ToList()
                                .ToPagedList(page ?? 1, 20);
                }

            }
            else
            {
                var selectedCategoryIds = GetAllSubCategorieIds(selectedCategory);
                selectedCategoryIds.Add(selectedCategory);

                model.ProductTabelList =
                    db.Product.Where(p => selectedCategoryIds.Contains(p.CategoryID))
                        .ToList()
                        .ToPagedList(page ?? 1, 20);
            }
            return View(model);
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