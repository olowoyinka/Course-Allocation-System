using MappingLectureCourse.Models.ContentViewModel;
using System;

namespace MappingLectureCourse.Models.MappingViewModel
{
    public class LectureQualification
    {
        public int LectureQualificationID { get; set; }

        public Guid LectureID { get; set; }

        public int QualificationID { get; set; }

        public Qualification Qualification { get; set; }
         
        public Lecture Lecture { get; set; }
    }
}
