using MappingLectureCourse.Data;
using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.UserViewModel
{
    public class UserForgetPassword
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Enter Password")]
        public string Newpassword { get; set; }

        [Required]
        [Compare("Newpassword", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Enter Confirm Password")]
        public string confirmNewPassword { get; set; }
    }

    public class  ListGetAUserPassword
    {
        public UserForgetPassword UserForgetPassword { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
