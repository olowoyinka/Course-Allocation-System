using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.ContentViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System;
using System.Threading.Tasks;

namespace MappingLectureCourse.Controllers
{
    [Authorize(Policy = "AdminAccess")]
    public class DepartmentController : Controller
    {
        private readonly IDepartment _departmentService;

        public DepartmentController(IDepartment departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index(string Search, int pageindex = 1)
        {
            var model = PagingList.Create(await _departmentService.getAllDepartment(Search), 5, pageindex);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create(MessageNote? message = null)
        {
            ViewData["Exist"] =
                message == MessageNote.Exist ? "This Department Already Exist"
                : "";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (await _departmentService.checkDepartmentExist(department))
            {
                return RedirectToAction("Create", new { Message = MessageNote.Exist });
            }

            var addDepartment = new Department
            {
               DepartmentID = Guid.NewGuid(),
               Name = department.Name
            };

            await _departmentService.createDepartment(addDepartment);

            return RedirectToAction("Detail", new { id = addDepartment.DepartmentID, Message = MessageNote.Exist });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid? Id, MessageNote? message = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "New Department Added; Here is the Detail"
                : message == MessageNote.Update ? "Department Updated; Here is the Detail" 
                :"";

            var getDepartment = await _departmentService.getDepartmentById(Id);

            return View(getDepartment);
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid? Id, MessageNote? message = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "This Department Already Exist"
                : "";

            var department = await _departmentService.getDepartmentById(Id);

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid? Id, Department department)
        {
            if (Id == null)
            {
                return NotFound();
            }

            if (await _departmentService.checkDepartmentExist(department))
            {
                return RedirectToAction("Update", new { id = Id, Message = MessageNote.Exist });
            }

            await _departmentService.updateDepartment(Id, department);

            return RedirectToAction("Detail", new { id = Id, Message = MessageNote.Update });
        }

        //[HttpGet]
        //public async Task<IActionResult> Delete(Guid? Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    var department = await _departmentService.getDepartmentById(Id);

        //    return View(department);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(Guid Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    await _departmentService.deleteDepartment(Id);

        //    return RedirectToAction("Index");
        //}

        public enum MessageNote
        {
            Add,
            Exist,
            Update
        }
    }
}