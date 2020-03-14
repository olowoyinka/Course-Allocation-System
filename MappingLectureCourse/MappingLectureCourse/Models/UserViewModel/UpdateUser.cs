using MappingLectureCourse.Models.ContentViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.UserViewModel
{
    public class UpdateUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public Guid DepartmentID { get; set; }

        public Department Department { get; set; }
    }
}
