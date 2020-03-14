using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.EntryViewModel;
using MappingLectureCourse.Models.MappingViewModel;
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
    public class LectureController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ApplicationDbContext _contextTwo;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILecture _lectureService;

        public LectureController(ApplicationDbContext context,
                                    ApplicationDbContext contextTwo,
                                        UserManager<ApplicationUser> userManager,
                                        ILecture lectureService)
        {
            _context = context;
            _contextTwo = contextTwo;
            _userManager = userManager;
            _lectureService = lectureService;
        }


        public async Task<IActionResult> Index(string search, int pageindex = 1)
        {
            var user = await GetCurrentUserAsync();

            var getUser = await _userManager.Users
                        .Include(i => i.Department)
                        .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == user.Id);

            var courses = from m in _context.lectures
                                   .Include(x => x.Department)
                                   .Include(l => l.Designation)
                                   .Include(s => s.LectureQualifications)
                                   .Include(r => r.LectureResearchAreas)
                          select m;

            if (!String.IsNullOrEmpty(search))
            {
                courses = courses.Where(s => s.DepartmentID == getUser.DepartmentID
                                    && ((s.FirstName.Contains(search)) || (s.LastName.Contains(search)) 
                                    || (s.Designation.Name.Contains(search)) ));
            }

            var model = PagingList.Create(await courses.OrderByDescending(s => s.LectureID).ToListAsync(), 10, pageindex);

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Register(MessageNote? message = null)
        {
            var user = await GetCurrentUserAsync(); 

            PopulateAssignedResearchAreaRegister(user.DepartmentID);
            PopulateAssignedQualificationRegister();

            ViewData["Exist"] =
               message == MessageNote.Exist ? "This Lecture First Name and Last Name Already Exist"
               : "";

            listItem();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string[] selectedResearch, string[] selectedQualification, Lecture lectures)
        {
            var user = await GetCurrentUserAsync();

            var addLecture = new Lecture
            {
                LectureID = Guid.NewGuid(),
                FirstName = lectures.FirstName,
                LastName = lectures.LastName,
                DesignationID = lectures.DesignationID,
                Available = lectures.Available,
                DepartmentID = user.DepartmentID
            };

            if (await _lectureService.checkLectureExist(addLecture))
            {
                return RedirectToAction("Register", new { Message = MessageNote.Exist });
            }

            await _lectureService.createLecture(addLecture);

            var getLecture = await _lectureService.getLectureByIdtwo(addLecture.LectureID);

            UpdateAssignedResearchAreaData(selectedResearch, getLecture);

            UpdateAssignedQualificationData(selectedQualification, getLecture);

            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", new { id = addLecture.LectureID, Message = MessageNote.Exist });
        }


        [HttpGet]
        public async Task<IActionResult> Detail(Guid? Id, MessageNote? message = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "New Lecture Added; Here is the Detail"
                : message == MessageNote.Update ? "Lecture Updated; Here is the Detail"
                : "";

            var getLecture = await _lectureService.getLectureById(Id);

            return View(getLecture);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid? Id, MessageNote? message = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "This Lecture First Name and Last Name Already Exist"
                : "";

            var user = await GetCurrentUserAsync();

            var lecture  =  await _context.lectures
                                .Include(x => x.Department)
                                .Include(l => l.Designation)
                                .Include(s => s.LectureQualifications)
                                        .ThenInclude(s => s.Qualification)
                                .Include(r => r.LectureResearchAreas)
                                        .ThenInclude(s => s.ResearchArea)
                                .AsNoTracking()
                         .SingleOrDefaultAsync(x => x.LectureID == Id);

            PopulateAssignedResearchAreaData(lecture, user.DepartmentID);

            PopulateAssignedQualificationData(lecture);

            listItem();

            return View(lecture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid? Id, string[] selectedResearch, 
                                                       string[] selectedQualification,Lecture lecture)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var user = await GetCurrentUserAsync();

            var getLectures = await _context.lectures
                                .Include(s => s.LectureQualifications)
                                        .ThenInclude(s => s.Qualification)
                                .Include(r => r.LectureResearchAreas)
                                        .ThenInclude(s => s.ResearchArea)
                                .Include(x => x.Department)
                                .Include(l => l.Designation)
                         .SingleOrDefaultAsync(x => x.LectureID == Id);

            UpdateAssignedResearchAreaData(selectedResearch, getLectures);

            UpdateAssignedQualificationData(selectedQualification, getLectures);

            await _context.SaveChangesAsync();

            if (await _lectureService.checkLectureExist(lecture))
            {
                getLectures.DesignationID = lecture.DesignationID;
                getLectures.Available = lecture.Available;
                getLectures.DepartmentID = user.DepartmentID;

                _contextTwo.lectures.Update(getLectures);

                await _contextTwo.SaveChangesAsync();

                return RedirectToAction("Detail", new { id = Id, Message = MessageNote.Update });
            }
            else
            {
                getLectures.FirstName = lecture.FirstName;
                getLectures.LastName = lecture.LastName;
                getLectures.DesignationID = lecture.DesignationID;
                getLectures.Available = lecture.Available;
                getLectures.DepartmentID = user.DepartmentID;

                _contextTwo.lectures.Update(getLectures);

                await _contextTwo.SaveChangesAsync();

                return RedirectToAction("Detail", new { id = Id, Message = MessageNote.Update });
            }
        }


        //[HttpGet]
        //public async Task<IActionResult> Delete(Guid? Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    listItem();

        //    var course = await _lectureService.getLectureById(Id);

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

        //    await _lectureService.deleteLecture(Id);

        //    return RedirectToAction("Index");
        //}


        private void PopulateAssignedResearchAreaRegister(Guid DepartmentID)
        {
            var allresearchAreas = _context.researchAreas.Where(s => s.DepartmentID.Equals(DepartmentID));

            var viewResearchAreas = new List<AssignedResearchData>();

            foreach (var researchAreas in allresearchAreas)
            {
                viewResearchAreas.Add(new AssignedResearchData
                {
                    ResearchAreaID = researchAreas.ResearchAreaID,
                    Name = researchAreas.Name,
                    Assigned = false
                });
            }

            ViewData["ResearchAreas"] = viewResearchAreas;
        }

        private void PopulateAssignedQualificationRegister()
        {
            var allQualification = _context.qualifications;

            var viewQualification = new List<AssignedQualificationData>();

            foreach (var qualification in allQualification)
            {
                viewQualification.Add(new AssignedQualificationData
                {
                    QualificationID = qualification.QualificationID,
                    Name = qualification.Name,
                    Assigned = false
                });
            }

            ViewData["Qualification"] = viewQualification;
        }

        private void PopulateAssignedResearchAreaData(Lecture lecture, Guid DepartmentID)
        { 
            var allresearchAreas = _context.researchAreas.Where(s => s.DepartmentID.Equals(DepartmentID));

            var lectureResearchAreas = new HashSet<Guid>(lecture.LectureResearchAreas.Select(c => c.ResearchArea.ResearchAreaID));

            var viewResearchAreas = new List<AssignedResearchData>();

            foreach (var researchAreas in allresearchAreas) 
            {
                viewResearchAreas.Add(new AssignedResearchData
                {
                    ResearchAreaID = researchAreas.ResearchAreaID,
                    Name = researchAreas.Name,
                    Assigned = lectureResearchAreas.Contains(researchAreas.ResearchAreaID)
                });
            }         

            ViewData["ResearchAreas"] = viewResearchAreas; 
        }

        private void PopulateAssignedQualificationData(Lecture lecture)
        {
            var allQualification = _context.qualifications;

            var lectureQualification = new HashSet<int>(lecture.LectureQualifications.Select(c => c.Qualification.QualificationID));

            var viewQualification = new List<AssignedQualificationData>();

            foreach (var qualification in allQualification)
            {
                viewQualification.Add(new AssignedQualificationData
                {
                    QualificationID = qualification.QualificationID,
                    Name = qualification.Name,
                    Assigned = lectureQualification.Contains(qualification.QualificationID)
                });
            }

            ViewData["Qualification"] = viewQualification;
        }

        private void UpdateAssignedResearchAreaData(string[] selectedResearch, Lecture lectureToUpdate)
        {
            if (selectedResearch == null)
            {
                lectureToUpdate.LectureResearchAreas = new List<LectureResearchArea>();
                return;
            }

            var selectedResearchHS = new HashSet<string>(selectedResearch);

            var lectureResearchArea = new HashSet<Guid>(lectureToUpdate.LectureResearchAreas.Select(c => c.ResearchArea.ResearchAreaID));

            foreach (var researchArea in _context.researchAreas)
            {
                if (selectedResearchHS.Contains(researchArea.ResearchAreaID.ToString()))
                {
                    if (!lectureResearchArea.Contains(researchArea.ResearchAreaID))
                    {
                        lectureToUpdate.LectureResearchAreas.Add(new LectureResearchArea
                        {
                            LectureID = lectureToUpdate.LectureID,
                            ResearchAreaID = researchArea.ResearchAreaID
                        });
                    }
                }
                else
                {
                    if (lectureResearchArea.Contains(researchArea.ResearchAreaID))
                    {
                        LectureResearchArea researchAreaToRemove = lectureToUpdate.LectureResearchAreas.SingleOrDefault(i => i.ResearchAreaID == researchArea.ResearchAreaID);
                        
                        _context.Remove(researchAreaToRemove);
                    }
                }
            }
        }

        private void UpdateAssignedQualificationData(string[] selectedQualification, Lecture lectureToUpdate)
        {
            if (selectedQualification == null)
            {
                lectureToUpdate.LectureQualifications = new List<LectureQualification>();
                return;
            }

            var selectedQualificationHS = new HashSet<string>(selectedQualification);

            var lectureQualification = new HashSet<int>(lectureToUpdate.LectureQualifications.Select(c => c.Qualification.QualificationID));

            foreach (var qualifications in _context.qualifications)
            {
                if (selectedQualificationHS.Contains(qualifications.QualificationID.ToString()))
                {
                    if (!lectureQualification.Contains(qualifications.QualificationID))
                    {
                        lectureToUpdate.LectureQualifications.Add(new LectureQualification
                        {
                            LectureID = lectureToUpdate.LectureID,
                            QualificationID = qualifications.QualificationID
                        });
                    }
                }
                else
                {
                    if (lectureQualification.Contains(qualifications.QualificationID))
                    {
                        LectureQualification qualificationToRemove = lectureToUpdate.LectureQualifications.SingleOrDefault(i => i.QualificationID == qualifications.QualificationID);

                        _context.Remove(qualificationToRemove);
                    }
                }
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        private void listItem()
        {
            ViewData["DepartmentID"] = new SelectList(_context.departments.OrderBy(m => m.Name), "DepartmentID", "Name");
            ViewData["DesignationID"] = new SelectList(_context.designations.OrderBy(m => m.Name), "DesignationID", "Name");
        }

        public enum MessageNote
        {
            Add,
            Exist,
            Update
        }
    }
}