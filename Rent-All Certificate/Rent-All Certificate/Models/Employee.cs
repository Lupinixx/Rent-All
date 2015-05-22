
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
    
    public partial class Employee
    {

        public int EmployeeID { get; set; }

        public int RoleID { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
    


        public virtual Role Role { get; set; }

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
    }
}
