using System.Collections.Generic;
using PagedList;
using PagedList.Mvc;

namespace Rent_All_Certificate.Models
{
    public class ProductIndexModel
    {
        public ProductIndexModel()
        {
            SelectedCategory = null;
        }
        public IPagedList<Product> ProductTabelList { get; set; }

        public List<CategorySelectListModel> CategorySelectList { get; set; }

        public int? SelectedCategory { get; set; }

        public string Search { get; set; }
    }
}