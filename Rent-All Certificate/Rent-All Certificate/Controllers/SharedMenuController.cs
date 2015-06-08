using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rent_All_Certificate.Models;

namespace Rent_All_Certificate.Controllers
{
    public class SharedMenuController : Controller
    {
        private RentAllUserEntities db = new RentAllUserEntities();
        // GET: SharedMenu
        private List<Category> sortAllCategories()
        {
            var categoryList = db.Category.Where(c => c.ParentID == null).ToList();
            var categoryListOut = new List<Category>();
            var sub = new List<Category>();
            Methods method = new Methods();

            foreach (var parent in categoryList)
            {
                categoryListOut.Add(parent);
                sub = method.GetAllSubCategoriesFromParent(parent.CategoryID);
                foreach (var item in sub)
                {
                    categoryListOut.Add(item);
                }
            }
            return categoryListOut;
        }

        public ActionResult Index(int? selectedCategory)
        {
            var model = new MenuModel
            {
                SelectedCategory = selectedCategory,
                Categories = sortAllCategories()
            };
            return PartialView("_Menu", model);
        }

        public ActionResult MenuName(int? selectedCategory)
        {
            return PartialView("_MenuName", db.Category.Where(c => c.CategoryID == selectedCategory).Distinct().ToList());
        }


    }
}