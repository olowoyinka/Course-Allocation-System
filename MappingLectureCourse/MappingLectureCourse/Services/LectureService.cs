using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.MappingViewModel;
using Microsoft.EntityFrameworkCore;

namespace MappingLectureCourse.Services
{
    public class LectureService : ILecture
    {
        private readonly ApplicationDbContext _context;

        public LectureService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Lecture> getLectureById(Guid? Id)
        {
              return await _context.lectures
                                .Include(x => x.Department)
                                .Include(l => l.Designation)
                                .Include(s => s.LectureQualifications)
                                        .ThenInclude(s => s.Qualification)
                                .Include(r => r.LectureResearchAreas)
                                        .ThenInclude(s => s.ResearchArea)
                                .AsNoTracking()
                         .SingleOrDefaultAsync(x => x.LectureID == Id);
        }

        public async Task<Lecture> getLectureByIdtwo(Guid? Id)
        {
            return await _context.lectures
                              .Include(x => x.Department)
                              .Include(l => l.Designation)
                              .Include(s => s.LectureQualifications)
                                      .ThenInclude(s => s.Qualification)
                              .Include(r => r.LectureResearchAreas)
                                      .ThenInclude(s => s.ResearchArea)
                       .SingleOrDefaultAsync(x => x.LectureID == Id);
        }

        public async Task<bool> createLecture(Lecture lecture)
        {
            await _context.lectures.AddAsync(lecture);
            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> updateLecture(Guid? Id, Lecture lecture, Guid DepartmentID)
        {
            var Updatelectures = await getLectureById(Id);

            Updatelectures.DesignationID = lecture.DesignationID;
            Updatelectures.Available = lecture.Available;
            Updatelectures.DepartmentID = DepartmentID;

            _context.lectures.Update(lecture);

            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> updateAllLecture(Guid? Id, Lecture lecture, Guid DepartmentID)
        {
            var Updatelectures = await getLectureById(Id);

            Updatelectures.FirstName = lecture.FirstName;
            Updatelectures.LastName = lecture.LastName;
            Updatelectures.DesignationID = lecture.DesignationID;
            Updatelectures.Available = lecture.Available;
            Updatelectures.DepartmentID = DepartmentID;

            _context.lectures.Update(lecture);

            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> deleteLecture(Guid? Id)
        {
            var lecture = await getLectureById(Id);

            if (lecture == null)
                return false;

            _context.lectures.Remove(lecture);

            var deleted = await _context.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> checkLectureExist(Lecture lecture)
        {
            return await _context.lectures
                        .AnyAsync(s => s.FirstName.Equals(lecture.FirstName) && s.LastName.Equals(lecture.LastName));
        }
    }
}