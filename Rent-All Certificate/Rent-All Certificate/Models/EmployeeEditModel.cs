using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rent_All_Certificate.Models
{
    public class EmployeeEditModel
    {
        public Employee EmployeeModel { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password (Leave empty to leave unchanged)")]
        public string EditPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("EditPassword", ErrorMessage = "The two passwords do not match.")]
        public string EditPasswordConfirm { get; set; }
    }
}