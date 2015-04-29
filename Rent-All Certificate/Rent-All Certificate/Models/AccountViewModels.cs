using System.ComponentModel.DataAnnotations;

namespace Rent_All_Certificate.Models
{
    public class LoginViewModel
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
        public string Password { get; set; }
    }
}