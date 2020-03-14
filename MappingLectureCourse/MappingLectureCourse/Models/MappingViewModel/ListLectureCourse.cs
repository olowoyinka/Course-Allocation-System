using MappingLectureCourse.Models.ContentViewModel;
using System;

namespace MappingLectureCourse.Models.MappingViewModel
{
    public class ListLectureCourse
    {
        public int ListLectureCourseID { get; set; }

        public Guid SessionID { get; set; }

        public int SemesterID { get; set; }

        public Guid DepartmentID { get; set; }

        public Session Session { get; set; }

        public Semester Semester { get; set; }

        public Department Department { get; set; }
    }
}
