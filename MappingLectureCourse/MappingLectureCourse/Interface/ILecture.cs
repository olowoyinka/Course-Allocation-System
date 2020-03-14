using MappingLectureCourse.Models.MappingViewModel;
using System;
using System.Threading.Tasks;

namespace MappingLectureCourse.Interface
{
    public interface ILecture
    {
        Task<Lecture> getLectureById(Guid? Id);

        Task<Lecture> getLectureByIdtwo(Guid? Id);

        Task<bool> createLecture(Lecture lecture);

        Task<bool> updateLecture(Guid? Id, Lecture lecture, Guid DepartmentID);

        Task<bool> updateAllLecture(Guid? Id, Lecture lecture, Guid DepartmentID);

        Task<bool> checkLectureExist(Lecture lecture);

        Task<bool> deleteLecture(Guid? Id);
    }
}