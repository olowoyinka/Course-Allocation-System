using MappingLectureCourse.Models.MappingViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.ContentViewModel
{
    public class Department
    {
        public Guid DepartmentID { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<ListLectureCourse> listLectureCourses { get; set; }
    }
}
