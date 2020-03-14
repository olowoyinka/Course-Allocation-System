using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.MappingViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MappingLectureCourse.Services
{
    public class CourseService : ICourse
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Course> getCourseById(Guid? Id)
        {
            return await _context.courses
                        .Include(x => x.Department)
                        .Include(l => l.Level)
                        .Include(s => s.Semester)
                        .Include(r => r.ResearchArea)
                        .SingleOrDefaultAsync(x => x.CourseID == Id);
        }

        public async Task<List<Course>> getAllCourse(Guid id, string Search)
        {
            var courses = from m in _context.courses
                           .Include(x => x.Department)
                           .Include(l => l.Level)
                           .Include(s => s.Semester)
                           .Include(r => r.ResearchArea)
                          select m;

            if (!String.IsNullOrEmpty(Search))
            {
                courses = courses.Where(s => s.DepartmentID == id 
                                    && (( s.CourseCode.Contains(Search)) || (s.CourseTitle.Contains(Search))
                                    || (s.ResearchArea.Name.Contains(Search)) || (s.Semester.Name.Contains(Search) 
                                    || (s.Level.LevelName.ToString().Contains(Search))) || (s.CourseUnit.ToString().Contains(Search))
                                    ));
            }
            else
            {
                courses = courses.Where(s => s.DepartmentID == id);
            }

            return await courses.OrderByDescending(s => s.CourseID).ToListAsync();
        }

        public async Task<bool> createCourse(Course course)
        {
            await _context.courses.AddAsync(course);
            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> updateCourse(Guid? Id, Course course)
        {
            var courses = await getCourseById(Id);

            courses.CourseTitle = course.CourseTitle;
            courses.CourseUnit = course.CourseUnit;
            courses.LevelID = course.LevelID;
            courses.ResearchAreaID = course.ResearchAreaID;
            courses.SemesterID = course.SemesterID;

            _context.courses.Update(courses);

            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> updateAllCourse(Guid? Id, Course course)
        {
            var courses = await getCourseById(Id);

            courses.CourseCode = course.CourseCode;
            courses.CourseTitle = course.CourseTitle;
            courses.CourseUnit = course.CourseUnit;
            courses.LevelID = course.LevelID;
            courses.ResearchAreaID = course.ResearchAreaID;
            courses.SemesterID = course.SemesterID;

            _context.courses.Update(courses);

            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> deleteCourse(Guid? Id)
        {
            var course = await getCourseById(Id);

            if (course == null)
                return false;

            _context.courses.Remove(course);

            var deleted = await _context.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> checkCourseExist(Course course)
        {
            return await _context.courses
                        .AnyAsync(s => s.CourseCode.Equals(course.CourseCode));
        }
        
    }
}