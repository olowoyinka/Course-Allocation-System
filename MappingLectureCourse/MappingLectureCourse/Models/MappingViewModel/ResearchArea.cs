using MappingLectureCourse.Models.ContentViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.MappingViewModel
{
    public class ResearchArea
    {
        public Guid ResearchAreaID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Department")]
        public Guid DepartmentID { get; set; }

        public Department Department { get; set; }

        public ICollection<LectureResearchArea> LectureResearchAreas { get; set; }
    }
}
