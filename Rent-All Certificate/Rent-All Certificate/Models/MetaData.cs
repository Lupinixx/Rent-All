using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rent_All_Certificate.Models
{
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

    [MetadataType(typeof(EmployeeMetaData))]
    public partial class Employee
    {
    }

    public class EmployeeMetaData
    {
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(50, ErrorMessage = "{0} has a maximum length of {1}")]
        [DataType(DataType.Text)]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }

        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname { get; set; }
    }


    [MetadataType(typeof(ManufacturerMetaData))]
    public partial class Manufacturer
    {

    }

    public class ManufacturerMetaData
    {
        [Required]
        [Display(Name = "Manufacturer")]
        public string ManufacturerName { get; set; }
    }

    [MetadataType(typeof(PhaseMetaData))]
    public partial class Phase
    {

    }

    public class PhaseMetaData
    {
        [Display(Name = "Phase")]
        [Required]
        public string PhaseName { get; set; }
    }

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {

    }

    public class ProductMetaData
    {
        [Display(Name = "Category")]
        [Required]
        public int CategoryID { get; set; }

        [Display(Name = "Manufacturer")]
        [Required]
        public int ManufacturerID { get; set; }

        [Display(Name = "Phase")]
        [Required]
        public int PhaseID { get; set; }

        [Display(Name = "Product")]
        [Required]
        public string ProductName { get; set; }

        [Display(Name = "Key")]
        [Required]
        public string ProductKey { get; set; }
    }

    [MetadataType(typeof(RoleMetaData))]
    public partial class Role
    {

    }

    public class RoleMetaData
    {
        [Display(Name = "Role")]
        public string Role1 { get; set; }
    }
}