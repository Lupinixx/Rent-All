//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rent_All_Certificate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            this.Product = new HashSet<Product>();
        }
    
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
    
        public virtual ICollection<Product> Product { get; set; }
    }
}
