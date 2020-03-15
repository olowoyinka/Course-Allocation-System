using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.EntryViewModel;
using MappingLectureCourse.Models.MappingViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MappingLectureCourse.Controllers
{
    [Authorize(Policy = "RequireAccess")]
    public class MappingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        private readonly IMapping _mappingService;

        private IEnumerable<ResearchArea> allresearchAreas { get; set; }

        private List<Course> allcourse = new List<Course>();

        private List<Lecture> allLecture = new List<Lecture>();

        private List<LectureCourse> allLectureCourse = new List<LectureCourse>();

        public MappingController(UserManager<ApplicationUser> userManager,
                                    ApplicationDbContext context,
                                    IMapping mappingService)
        {
            _userManager = userManager;
            _context = context;
            _mappingService = mappingService;
        }

        public IActionResult Mapping(MessageNote? message = null)
        {
            ViewData["Exist"] =
               message == MessageNote.Exist ? "Your Password Has Been Changed. Welcome to Unilag Course Allocation System"
               : "";

            return View();
        }


        [HttpGet]
        public IActionResult GenerateMapping()
        {
            listItem();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateMapping(SelectSemester selectItem)
        {
            var user = await GetCurrentUserAsync();

            var getUser = await _userManager.Users
                        .Include(i => i.Department)
                        .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == user.Id);

            var DeleteLectureCourse = from s in _context.lectureCourses
                                          .Include(s => s.Semester)
                                          .Include(s => s.Department)
                                          .Include(s => s.Session)
                                      select s;

            DeleteLectureCourse = DeleteLectureCourse.Where(s => s.Semester.Name.Equals(selectItem.SemesterID)
                                                            && s.Department.Name.Equals(getUser.Department.Name)
                                                            && s.Session.SessionName.Equals(_mappingService.lastNameofSession()));

            foreach (var items in DeleteLectureCourse.ToList())
            {
                LectureCourse LectureCourseToRemove = DeleteLectureCourse
                                                            .SingleOrDefault(i => i.LectureCourseID == items.LectureCourseID);

                _context.lectureCourses.Remove(LectureCourseToRemove);

                _context.SaveChanges();
            }

            var getCourse = from s in _context.courses
                        .Include(s => s.ResearchArea)
                        .Include(s => s.Level)
                        .Include(se => se.Semester)
                        .Include(s => s.Department)
                            select s;

            getCourse = getCourse.Where(s => s.Semester.Name.Equals(selectItem.SemesterID)
                                                            && s.Department.Name.Equals(getUser.Department.Name))
                                                                    .OrderByDescending(s => s.Level.LevelName);

            allcourse = getCourse.ToList();


            foreach (var rankItem in _context.designations.OrderByDescending(s => s.ScalingFactor).ToList())
            {
                var getLecture = from s in _context.lectures
                                         .Include(d => d.Department)
                                         .Include(r => r.Designation)
                                         .Include(s => s.LectureResearchAreas)
                                             .ThenInclude(s => s.ResearchArea)
                                 select s;

                getLecture = getLecture.Where(s => s.Department.Name.Equals(getUser.Department.Name)
                                                        && s.Designation.Name.Equals(rankItem.Name)
                                                        && s.Available.Equals(true));

                allLecture = getLecture.ToList();

                foreach (var listcourse in allcourse)
                {
                    foreach (var listlecture in allLecture)
                    {
                        Guid getspecialization = allcourse.FirstOrDefault(c => c.CourseID == listcourse.CourseID).ResearchAreaID;

                        Lecture lecture = allLecture.FirstOrDefault(a => a.LectureID == listlecture.LectureID);

                        Course course = allcourse.FirstOrDefault(a => a.CourseID == listcourse.CourseID);

                        allresearchAreas = lecture.LectureResearchAreas.Select(b => b.ResearchArea);

                        foreach (var listspecialization in allresearchAreas)
                        {
                            Guid getlecturespecialization = allresearchAreas.FirstOrDefault(r => r.ResearchAreaID == listspecialization.ResearchAreaID).ResearchAreaID;

                            if (getlecturespecialization == getspecialization)
                            {
                                if (allLectureCourse.FindAll(s => s.LectureID == lecture.LectureID).Count() < 4 )
                                {
                                    if(allLectureCourse.FindAll(s => s.CourseID == course.CourseID).Count() < 4)
                                    {
                                        allLectureCourse.Add(new LectureCourse
                                        {
                                            CourseID = listcourse.CourseID,
                                            LectureID = listlecture.LectureID,
                                            SemesterID = listcourse.SemesterID,
                                            DepartmentID = listcourse.DepartmentID,
                                            SessionID = _mappingService.lastValueofSession()

                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _context.lectureCourses.AddRange(allLectureCourse);

            _context.SaveChanges();

            var SessionValue =  _context.semesters.SingleOrDefault(s => s.Name == selectItem.SemesterID).SemesterID;

            if (!await _mappingService.checkListLectureCourseExist(getUser.DepartmentID, SessionValue))
            {
                await _mappingService.createListLectureCourse(getUser.DepartmentID, SessionValue);
            }

            return RedirectToAction("Index", new { Message = MessageNote.Exist });
        }


        //Let it be PDF

        public async Task<IActionResult> PrintMapping(string semesterID, string SessionID)
        {
            if(semesterID == null || SessionID == null)
            {
                return RedirectToAction("HistoryofMapping", new { Message = MessageNote.Add });
            }


            var user = await GetCurrentUserAsync();

            var getUser = await _userManager.Users
                                .Include(s => s.Department)
                                .SingleOrDefaultAsync(s => s.Id == user.Id);

            var printmapping = new PrintMapping();

            printmapping.Department = getUser.Department.Name;

            printmapping.Semester = semesterID;

            printmapping.Session = SessionID;

            printmapping.levels = await _context.levels
                                            .OrderByDescending(s => s.LevelName).ToListAsync();

            printmapping.courses = await _context.courses
                                                .Include(s => s.Department)
                                                .Include(s => s.Level)
                                                .Include(s => s.ResearchArea)
                                                .Include(s => s.Semester)
                                            .Where(s => s.DepartmentID.Equals(user.DepartmentID) 
                                                    && s.Semester.Name.Equals(semesterID)).OrderBy(s => s.CourseTitle).ToListAsync();

            printmapping.lectureCourses = await _context.lectureCourses
                                                        .Include(s => s.Department)
                                                        .Include(s => s.Session)
                                                        .Include(s => s.Semester)
                                                        .Include(s => s.Lecture)
                                                            .ThenInclude(s => s.Designation)
                                                    .Where(s => s.DepartmentID.Equals(user.DepartmentID) 
                                                            && s.Session.SessionName.Equals(SessionID)
                                                            && s.Semester.Name.Equals(semesterID))
                                                    .OrderByDescending(s => s.Lecture.Designation.ScalingFactor).ToListAsync();

            return View(printmapping);
        }


        public async Task<IActionResult> Index(int pageindex = 1, MessageNote? message = null)
        {
            var user = await GetCurrentUserAsync();

            ViewData["Exist"] =
                message == MessageNote.Exist ? "Allocation of Lecture to their Courses Generated;":
                message == MessageNote.Add ? "Select One of the Record"
                : "";

            var model = PagingList.Create(await _mappingService.HistoyofMapping(user.DepartmentID), 5, pageindex);

            return View(model);
        }


        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        private void listItem()
        {
            ViewData["SemesterID"] = new SelectList(_context.semesters.OrderByDescending(m => m.Name), "SemesterID", "Name");
        }

        public enum MessageNote
        {
            Add,
            Exist,
            Update
        }

    }
}