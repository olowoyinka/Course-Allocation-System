using MappingLectureCourse.Models.ContentViewModel;
using MappingLectureCourse.Models.MappingViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MappingLectureCourse.Interface
{
    public interface IMapping
    {
        Task<bool> createListLectureCourse(Guid DepartmentID, int SemesterID);

        Task<bool> checkListLectureCourseExist(Guid DepartmentID, int SemesterID);

        Task<List<ListLectureCourse>> HistoyofMapping(Guid DepartmentID);

        Guid lastValueofSession();

        string lastNameofSession();
    }
}
