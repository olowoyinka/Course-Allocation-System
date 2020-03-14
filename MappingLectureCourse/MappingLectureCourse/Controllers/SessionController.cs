using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.ContentViewModel;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System;
using System.Threading.Tasks;

namespace MappingLectureCourse.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessions _sessionService;

        public SessionController(ISessions sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(int pageindex = 1)
        {
            var model = PagingList.Create(await _sessionService.getAllSession(), 10, pageindex);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create(MessageNote? message = null)
        {
            ViewData["Exist"] =
                message == MessageNote.Exist ? "This Session Already Exist"
                : "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Session session)
        {
            if (await _sessionService.checkSessionExist(session))
            {
                return RedirectToAction("Create", new { Message = MessageNote.Exist });
            }

            var addSession = new Session
            {
                SessionID = Guid.NewGuid(),
                SessionName = session.SessionName
            };

            await _sessionService.createSession(addSession);

            return RedirectToAction("Detail", new { id = addSession.SessionID, Message = MessageNote.Exist });
        }


        [HttpGet]
        public async Task<IActionResult> Detail(Guid? Id, MessageNote? message = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "New Session Added; Here is the Detail"
                : message == MessageNote.Update ? "Session Updated; Here is the Detail"
                : "";

            var getSession = await _sessionService.getSessionById(Id);

            return View(getSession);
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid? Id, MessageNote? message = null)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ViewData["Exist"] =
                message == MessageNote.Exist ? "This Session Already Exist"
                : "";

            var session = await _sessionService.getSessionById(Id);

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid? Id, Session session)
        {
            if (Id == null)
            {
                return NotFound();
            }

            if (await _sessionService.checkSessionExist(session))
            {
                return RedirectToAction("Update", new { id = Id, Message = MessageNote.Exist });
            }

            var sessions = await _sessionService.getSessionById(Id);

            sessions.SessionName = session.SessionName;

            await _sessionService.updateSession(sessions);

            return RedirectToAction("Detail", new { id = Id, Message = MessageNote.Update });
        }

        //[HttpGet]
        //public async Task<IActionResult> Delete(Guid? Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["Deleted"] = "Session Deleted Successfully";

        //    var session = await _sessionService.getSessionById(Id);

        //    return View(session);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(Guid Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    await _sessionService.deleteSession(Id);

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