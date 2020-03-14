using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.MappingViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MappingLectureCourse.Controllers
{
    [Authorize(Policy = "RequireAccess")]
    public class ResearchAreaController : Controller
    {
        private readonly IResearchArea _researchAreaService;

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public ResearchAreaController(IResearchArea researchAreaService,
                                        ApplicationDbContext context,
                                        UserManager<ApplicationUser> userManager)
        {
            _researchAreaService = researchAreaService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string search, int pageindex = 1)
        {
            var user = await GetCurrentUserAsync();

            var researchAreas = from m in _context.researchAreas
                                        .Include(s => s.Department)
                         select m;

            if (!String.IsNullOrEmpty(search))
            {
                researchAreas = researchAreas.Where(s => s.DepartmentID == user.DepartmentID
                                            && s.Name.Contains(search));
            }

            var model = PagingList.Create(await researchAreas.OrderByDescending(s => s.ResearchAreaID).ToListAsync(), 5, pageindex);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create(MessageNote? message = null)
        {
            listItem();

            ViewData["Exist"] =
                message == MessageNote.Exist ? "This Research Area Already Exist with the Department"
                : "";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResearchArea researchArea)
        {
            if (await _researchAreaService.checkResearchAreaExist(researchArea))
            {
                return RedirectToAction("Create", new { Message = MessageNote.Exist });
            }

            var user = await GetCurrentUserAsync();

            var addResearchArea = new ResearchArea
            {
                ResearchAreaID = Guid.NewGuid(),
                Name = researchArea.Name,
                DepartmentID = user.DepartmentID
            };

            await _researchAreaService.createResearchArea(addResearchArea);

            return RedirectToAction("Detail", new { id = addResearchArea.ResearchAreaID, Message = MessageNote.Exist });
        }


        [HttpGet]
        public async Task<IActionResult> Detail(Guid? Id, MessageNote? message = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "New Research Area Added; Here is the Detail"
                : message == MessageNote.Update ? "Research Area Updated; Here is the Detail"
                : "";

            var getResearchArea = await _researchAreaService.getResearchAreaById(Id);

            return View(getResearchArea);
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid? Id, MessageNote? message = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "This Research Area Already Exist With Your Department"
                : "";

            listItem();

            var getResearchArea = await _researchAreaService.getResearchAreaById(Id);

            return View(getResearchArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid? Id, ResearchArea researchArea)
        {
            if (Id == null)
            {
                return NotFound();
            }

            if (await _researchAreaService.checkResearchAreaExist(researchArea))
            {
                return RedirectToAction("Update", new { id = Id, Message = MessageNote.Exist });
            }

            var getResearchArea = await _researchAreaService.getResearchAreaById(Id);

            getResearchArea.Name = researchArea.Name;

            await _researchAreaService.updateResearchArea(getResearchArea);

            return RedirectToAction("Detail", new { id = Id, Message = MessageNote.Update });
        }

        //[HttpGet]
        //public async Task<IActionResult> Delete(Guid? Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    var getResearchArea = await _researchAreaService.getResearchAreaById(Id);

        //    return View(getResearchArea);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(Guid Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["Deleted"] = "Research Area Deleted Successfully";

        //    await _researchAreaService.deleteResearchArea(Id);

        //    return RedirectToAction("Index");
        //}

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        private void listItem()
        {
            ViewData["DepartmentID"] = new SelectList(_context.departments.OrderBy(m => m.Name), "DepartmentID", "Name");
        }

        public enum MessageNote
        {
            Add,
            Exist,
            Update
        }
    }
}