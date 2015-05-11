
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
    
    public partial class Category
    {

        public Category()
        {

            this.Category1 = new HashSet<Category>();

            this.Product = new HashSet<Product>();

        }
    

        public int CategoryID { get; set; }

        public Nullable<int> ParentID { get; set; }

        public string CategoryName { get; set; }
    
        public virtual ICollection<Category> Category1 { get; set; }

        public virtual Category Category2 { get; set; }

        public virtual ICollection<Product> Product { get; set; }

    }

    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {

    }

    public class CategoryMetaData
    {
        [Display(Name = "Parent Category")]
        public Nullable<int> ParentID { get; set; }

        [Display(Name = "Category")]
        [Required]
        public string CategoryName { get; set; }

        [Display(Name = "Products")]
        public virtual ICollection<Product> Product { get; set; }
    }
}
