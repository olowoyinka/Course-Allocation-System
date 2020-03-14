using MappingLectureCourse.Models.ContentViewModel;
using System;

namespace MappingLectureCourse.Models.MappingViewModel
{
    public class LectureCourse
    {
        public int LectureCourseID { get; set; }

        public Guid LectureID { get; set; }

        public Guid CourseID { get; set; }

        public Guid DepartmentID { get; set; }

        public int SemesterID { get; set; }

        public Guid SessionID { get; set; }

        public Lecture Lecture { get; set; }

        public Course Course { get; set; }

        public Department Department { get; set; }

        public Semester Semester { get; set; }

        public Session Session { get; set; }
    }
}
