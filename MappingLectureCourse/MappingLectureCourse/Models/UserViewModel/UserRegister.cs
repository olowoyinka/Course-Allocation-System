using MappingLectureCourse.Models.ContentViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.UserViewModel
{
    public class UserRegister
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string confirmPassword { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public Guid DepartmentID { get; set;}

        public Department Department { get; set; }
    }
}
