using System;
using System.Linq;
using System.Threading.Tasks;
using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using Microsoft.EntityFrameworkCore;
using MappingLectureCourse.Models.MappingViewModel;
using System.Collections.Generic;
using MappingLectureCourse.Models.ContentViewModel;

namespace MappingLectureCourse.Services
{
    public class MappingService : IMapping
    {
        private readonly ApplicationDbContext _context;


        public MappingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> createListLectureCourse(Guid DepartmentID, int SemesterID)
        {
            var addListLectureCourse = new ListLectureCourse
            { 
                DepartmentID = DepartmentID,
                SemesterID = SemesterID,
                SessionID = lastValueofSession()
            };

            await _context.listLectureCourses.AddAsync(addListLectureCourse);

            var created = await _context.SaveChangesAsync();

            return created > 0;
        }
        

        public async Task<bool> checkListLectureCourseExist(Guid DepartmentID, int SemesterID)
        {
            Guid Value = lastValueofSession();

            return await _context.listLectureCourses
                                .Include(s => s.Semester)
                        .AnyAsync(s => s.SessionID.Equals(Value) 
                                && s.SemesterID.Equals(SemesterID) && s.DepartmentID.Equals(DepartmentID));
        }


        public Guid lastValueofSession()
        {
            return  _context.sessions.OrderByDescending(s => s.SessionID).FirstOrDefault().SessionID;
        }

        public string lastNameofSession()
        {
            return _context.sessions.OrderByDescending(s => s.SessionID).FirstOrDefault().SessionName;
        }

        public async Task<List<ListLectureCourse>> HistoyofMapping(Guid DepartmentID)
        {
            return await _context.listLectureCourses
                                        .Include(s => s.Semester)
                                        .Include(s => s.Session)
                                        .Include(s => s.Department)
                                            .Where(s => s.DepartmentID.Equals(DepartmentID))
                                            .OrderByDescending(s => s.ListLectureCourseID)
                                            .ToListAsync();
        }
    }
}
