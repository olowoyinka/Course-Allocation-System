using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.MappingViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReflectionIT.Mvc.Paging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MappingLectureCourse.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourse _courseService;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        public CourseController(ICourse courseService,
                                  ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager)
        {
            _courseService = courseService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string Search, int pageindex = 1)
        {
            var user = await GetCurrentUserAsync();

            var model = PagingList.Create(await _courseService.getAllCourse(user.DepartmentID, Search), 10, pageindex);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create(MessageNote? message = null)
        {
            listItem();

            ViewData["Exist"] =
                message == MessageNote.Exist ? "This Course Already Exist"
                : "";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if(await _courseService.checkCourseExist(course))
            {
                return RedirectToAction("Create", new { Message = MessageNote.Exist } );
            }

            var user = await GetCurrentUserAsync();

            var addCourse = new Course
            {
                CourseID = Guid.NewGuid(),
                CourseCode = course.CourseCode,
                CourseTitle = course.CourseTitle,
                CourseUnit = course.CourseUnit,
                LevelID = course.LevelID,
                ResearchAreaID = course.ResearchAreaID,
                SemesterID = course.SemesterID,
                DepartmentID = user.DepartmentID
            };

            await _courseService.createCourse(addCourse);

            return RedirectToAction("Detail", new { id = addCourse.CourseID, Message = MessageNote.Exist });
        }


        [HttpGet]
        public async Task<IActionResult> Detail(Guid? Id, MessageNote? message = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "New Course Added; Here is the Detail"
                : message == MessageNote.Update ? "Course Updated; Here is the Detail"
                : "";

            var getCourse = await _courseService.getCourseById(Id);

            return View(getCourse);
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid? Id, MessageNote? message = null)
        {
            if(Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "This Course Already Exist"
                : "";

            listItem();

            var course =  await _courseService.getCourseById(Id);

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid? Id, Course course)
        {
            if(Id == null)
            {
                return NotFound();
            }

            if (await _courseService.checkCourseExist(course))
            {
                await _courseService.updateCourse(Id, course);

                return RedirectToAction("Detail", new { id = Id, Message = MessageNote.Update });
            }

            await _courseService.updateAllCourse(Id, course);

            return RedirectToAction("Detail", new { id = Id, Message = MessageNote.Update });
        }

        //[HttpGet]
        //public async Task<IActionResult> Delete(Guid? Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    listItem();

        //    var course = await _courseService.getCourseById(Id);

        //    return View(course);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(Guid Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    await _courseService.deleteCourse(Id);

        //    return RedirectToAction("Index");
        //}

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        private void listItem()
        {
            ViewData["DepartmentID"] = new SelectList(_context.departments.OrderBy(m => m.Name), "DepartmentID", "Name");
            ViewData["LevelID"] = new SelectList(_context.levels.OrderByDescending(m => m.LevelName), "LevelID", "LevelName");
            ViewData["SemesterID"] = new SelectList(_context.semesters.OrderByDescending(m => m.Name), "SemesterID", "Name");
            ViewData["ResearchAreaID"] = new SelectList(_context.researchAreas.OrderByDescending(m => m.Name), "ResearchAreaID", "Name");
        }

        public enum MessageNote
        {
            Add,
            Exist,
            Update
        }
    }
}