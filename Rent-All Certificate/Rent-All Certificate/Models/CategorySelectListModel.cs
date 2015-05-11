using System.Collections.Generic;

namespace Rent_All_Certificate.Models
{
    public class CategorySelectListModel
    {
        public int? SelectedCategoryId { get; set; }

        public List<Category> CategoriesSelectList { get; set; }
    }
}