using MappingLectureCourse.Models.MappingViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MappingLectureCourse.Interface
{
    public interface ICourse
    {
        Task<List<Course>> getAllCourse(Guid id, string Search);

        Task<Course> getCourseById(Guid? Id);

        Task<bool> createCourse(Course course);

        Task<bool> updateCourse(Guid? Id, Course course);

        Task<bool> updateAllCourse(Guid? Id, Course course);

        Task<bool> deleteCourse(Guid? Id);

        Task<bool> checkCourseExist(Course course);
    }
}
