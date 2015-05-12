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


        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}