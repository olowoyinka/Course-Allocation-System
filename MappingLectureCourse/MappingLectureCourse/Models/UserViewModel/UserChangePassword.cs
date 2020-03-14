using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.UserViewModel
{
    public class UserChangePassword
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string Oldpassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Newpassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Newpassword", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm New Password")]
        public string confirmNewPassword { get; set; }
    }
}
