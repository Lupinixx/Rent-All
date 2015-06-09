using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rent_All_Certificate.Models
{
    public class DashboardModel
    {
        public int InventoryCount { get; set; }
        public int ProductsCount { get; set; }
        public int CategoryCount { get; set; }
        public int ManufactureCount { get; set; }
    }
}