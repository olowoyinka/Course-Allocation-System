using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.EntryViewModel
{
    public class SelectSemester
    {
        [Required]
        public string SemesterID { get; set; }
    }
}
