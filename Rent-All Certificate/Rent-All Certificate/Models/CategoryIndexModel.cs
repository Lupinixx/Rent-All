using System.Collections.Generic;

namespace Rent_All_Certificate.Models
{
    public class CategoryIndexModel
    {
        public CategoryIndexModel()
        {
            StarterCategoryId = null;
        }
        public List<Category> CategorieTabelList { get; set; }

        public List<CategorySelectListModel> CategorySelectList { get; set; }

        public int? StarterCategoryId { get; set; }
    }
}