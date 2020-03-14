using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.ContentViewModel
{
    public class Level
    {
        public int LevelID { get; set; }

        [Required]
        [Display(Name = "Course Code")]
        public int LevelName { get; set; }
    }
}
