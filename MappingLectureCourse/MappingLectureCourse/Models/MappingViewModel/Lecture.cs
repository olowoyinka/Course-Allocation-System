using MappingLectureCourse.Models.ContentViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MappingLectureCourse.Models.MappingViewModel
{
    public class Lecture
    {
        public Guid LectureID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public int DesignationID { get; set; }

        [Display(Name = "Available")]
        public bool Available { get; set; }

        [Required]
        [Display(Name = "Department")]
        public Guid DepartmentID { get; set; }

        public Designation Designation { get; set; }

        public Department Department { get; set; }

        public ICollection<LectureQualification> LectureQualifications { get; set; }

        public ICollection<LectureResearchArea> LectureResearchAreas { get; set; }
    }
}
