using System.Collections.Generic;
using PagedList;
using PagedList.Mvc;

namespace Rent_All_Certificate.Models
{
    public class CategoryIndexModel
    {
        public CategoryIndexModel()
        {
            StarterCategoryId = null;
        }
        public IPagedList<Category> CategorieTabelList { get; set; }

        public List<CategorySelectListModel> CategorySelectList { get; set; }

        public int? StarterCategoryId { get; set; }
    }
}