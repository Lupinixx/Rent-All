using System.Collections.Generic;

namespace Rent_All_Certificate.Models
{
    public class ProductIndexModel
    {
        public ProductIndexModel()
        {
            StarterCategoryId = null;
        }
        public List<Product> ProductTabelList { get; set; }

        public List<CategorySelectListModel> CategorySelectList { get; set; }

        public int? StarterCategoryId { get; set; }
    }
}