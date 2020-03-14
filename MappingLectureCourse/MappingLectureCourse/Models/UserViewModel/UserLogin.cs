using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.UserViewModel
{
    public class UserLogin
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Username/Email Address")]
        public string email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
