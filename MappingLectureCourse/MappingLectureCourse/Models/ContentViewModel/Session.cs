using System;
using System.ComponentModel.DataAnnotations;

namespace MappingLectureCourse.Models.ContentViewModel
{
    public class Session
    {
        public Guid SessionID { get; set; }

        [Required]
        public string SessionName { get; set; }
    }
}
