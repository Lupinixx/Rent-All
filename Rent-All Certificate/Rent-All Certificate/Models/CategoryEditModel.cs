using System.Collections.Generic;

namespace Rent_All_Certificate.Models
{
    public class CategoryEditModel
    {
        public virtual Category Category { get; set; }

        public List<CategorySelectListModel> CategorySelectList { get; set; }
    }
}