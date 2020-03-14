using MappingLectureCourse.Models.ContentViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.MappingViewModel
{
    public class Course
    {
        public Guid CourseID { get; set; }

        [Required]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Required]
        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }

        [Required]
        [Display(Name = "Course Unit")]
        public int CourseUnit { get; set; }

        [Required]
        [Display(Name = "Level")]
        public int LevelID { get; set; }

        [Required]
        [Display(Name = "Research Area")]
        public Guid ResearchAreaID { get; set; }

        [Required]
        [Display(Name = "Semester")]
        public int SemesterID { get; set; }

        [Display(Name = "Department")]
        public Guid DepartmentID { get; set; }

        public ResearchArea ResearchArea { get; set; }

        public Semester Semester { get; set; }

        public Department Department { get; set; }

        public Level Level { get; set; }
    }
}
