using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rent_All_Certificate.Models;

namespace Rent_All_Certificate.Controllers
{
    public class Methods : Controller
    {
        private RentAllEntities db = new RentAllEntities();

        public List<Category> GetAllSubCategoriesFromParent(int parentId)
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
    }
}