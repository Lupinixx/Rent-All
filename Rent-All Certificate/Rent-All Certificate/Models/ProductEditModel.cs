using System.Collections.Generic;

namespace Rent_All_Certificate.Models
{
    public class ProductEditModel
    {
        public virtual Product Product { get; set; }

        public List<CategorySelectListModel> CategorySelectList { get; set; }
    }
}