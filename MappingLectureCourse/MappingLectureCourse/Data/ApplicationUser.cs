using MappingLectureCourse.Models.ContentViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public override string Email { get; set; }

        [Required]
        public Guid DepartmentID { get; set; }

        public Department Department { get; set; }

        public DateTime RegisterDateTime { get; set; }
    }
}