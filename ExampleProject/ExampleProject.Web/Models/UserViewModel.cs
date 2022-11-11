using ExampleProject.DataAccess.Enums;
using ExampleProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleProject.Web.Models
{
    public class UserViewModel
    {
        public int EmployeeId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Employee Role")]
        public UserRole UserRole { get; set; }
    }
}
