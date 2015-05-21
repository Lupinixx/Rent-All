using System.Collections.Generic;

namespace Rent_All_Certificate.Models
{
    public class MenuModel
    {
        public MenuModel()
        {
            SelectedCategory = null;
        }
        public int? SelectedCategory { get; set; }

        public List<Category> Categories { get; set; }
    }
}