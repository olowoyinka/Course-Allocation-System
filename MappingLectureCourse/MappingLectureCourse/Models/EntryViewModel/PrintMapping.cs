using MappingLectureCourse.Models.ContentViewModel;
using MappingLectureCourse.Models.MappingViewModel;
using System.Collections.Generic;



namespace MappingLectureCourse.Models.EntryViewModel
{
    public class PrintMapping
    {
        public IEnumerable<Level> levels { get; set; }

        public IEnumerable<Course> courses { get; set; }

        public IEnumerable<LectureCourse> lectureCourses { get; set; }

        public string Session { get; set; }

        public string Department { get; set; }

        public string Semester { get; set; }
    }
}
