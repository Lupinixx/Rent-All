
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Rent_All_Certificate.Models
{

    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {

        public string ProductKey { get; set; }

        public int PhaseID { get; set; }

        public int CategoryID { get; set; }

        public int ManufacturerID { get; set; }

        public string ProductName { get; set; }
    


        public virtual Category Category { get; set; }

        public virtual Hoist Hoist { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public virtual Phase Phase { get; set; }

    }

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {

    }

    public class ProductMetaData
    {
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Manufacturer")]
        public int ManufacturerID { get; set; }

        [Display(Name = "Phase")]
        public int PhaseID { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; }

        [Display(Name = "Key")]
        public string ProductKey { get; set; }
    }
}
