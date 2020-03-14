using System;

namespace MappingLectureCourse.Models.MappingViewModel
{
    public class LectureResearchArea
    {
        public int LectureResearchAreaID { get; set; }

        public Guid LectureID { get; set; }
         
        public Guid ResearchAreaID { get; set; }

        public Lecture Lecture { get; set; }

        public ResearchArea ResearchArea { get; set; }
    }
}
