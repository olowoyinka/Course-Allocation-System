using System;

namespace MappingLectureCourse.Models.EntryViewModel
{
    public class AssignedResearchData
    {
        public Guid ResearchAreaID { get; set; } 

        public string Name { get; set; }
         
        public bool Assigned { get; set; }
    }

    public class AssignedQualificationData
    {
        public int QualificationID { get; set; }
         
        public string Name { get; set; }

        public bool Assigned { get; set; }
    }
}
