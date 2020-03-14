using MappingLectureCourse.Data;
using MappingLectureCourse.Models.UserViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReflectionIT.Mvc.Paging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MappingLectureCourse.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ILogger _logger;

        public UsersController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<UsersController>();
        }

        public async Task<IActionResult> Index(string search, MessageNote? message = null, int pageindex = 1)
        {
            var users = from m in _userManager.Users
                                                .Include(s => s.Department)
                                select m;

            if (!String.IsNullOrEmpty(search))
            {
                users = users.Where(s => s.Department.Name.Contains(search)
                                            || s.FirstName.Contains(search) || s.LastName.Contains(search)
                                                || s.FirstName.Contains(search) || s.UserName.Contains(search) );
            }

            ViewData["Exist"] =
               message == MessageNote.Exist ? "New User Registered"
               : "";

            var model = PagingList.Create(await users.OrderByDescending(s => s.Id).ToListAsync(), 10, pageindex);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterUser()
        {
            listItem();

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserRegister userRegister)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userRegister.Email,
                    Email = userRegister.Email,
                    FirstName = userRegister.FirstName,
                    LastName = userRegister.LastName,
                    DepartmentID = userRegister.DepartmentID
                };

                var result = await _userManager.CreateAsync(user, userRegister.password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "StaffUserRole" );

                    _logger.LogInformation(3, "User Created a New Account With Password.");

                    return RedirectToAction("Index", new { Message = MessageNote.Exist });
                }

                AddErrors(result);
            }
            
            return View(userRegister);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginUser(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLogin.email);

                var result = await _signInManager.PasswordSignInAsync(userLogin.email, 
                                                                        userLogin.password, userLogin.RememberMe, 
                                                                        lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        return RedirectToAction("ChangeUserPassword");
                    }

                    _logger.LogInformation(1, "User logged in.");

                    return RedirectToAction(nameof(MappingController.Index), "Mapping");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "InCorrect Username or Password");
                    return View(userLogin);
                }
            }

            return View(userLogin);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOutUser()
        {
            await _signInManager.SignOutAsync();

            _logger.LogInformation(4, "User logged out.");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet]
        public async Task<IActionResult> UpdateUserInfo(string Id, MessageNote? message = null)
        {
            var user = await _userManager.Users
                                .Include(s => s.Department)
                            .SingleOrDefaultAsync(s => s.Id == Id);

            ViewData["Exist"] =
               message == MessageNote.Exist ? "User Information is Updated"
               : "";

            listItem();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserInfo(string Id, ApplicationUser updateUser)
        {
            listItem();

            var user = await _userManager.Users
                                .Include(s => s.Department)
                            .SingleOrDefaultAsync(s => s.Id == Id);

            user.LastName = updateUser.LastName;
            user.FirstName = updateUser.FirstName;
            user.DepartmentID = updateUser.DepartmentID;
            user.Email = updateUser.Email;
            user.UserName = updateUser.Email;
            user.EmailConfirmed = false;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("UpdateUserInfo", new { id = Id, Message = MessageNote.Exist });
        }


        [HttpGet]
        public IActionResult ChangeUserPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(UserChangePassword userChangePassword)
        {
            if (!ModelState.IsValid)
            {
                return View(userChangePassword);
            }

            var user = await GetCurrentUserAsync();

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, userChangePassword.Oldpassword, userChangePassword.Newpassword);

                if (result.Succeeded)
                {
                    user.EmailConfirmed = true;

                    await _userManager.UpdateAsync(user);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    _logger.LogInformation(3, "User changed their password successfully.");

                    return RedirectToAction(nameof(MappingController.Index), "Mapping", new { Message = MessageNote.Exist });
                }

                AddErrors(result);

                return View(userChangePassword);
            }

            return View(userChangePassword);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotUserPassword(string Id, MessageNote? message = null)
        {
            var listuser = new ListGetAUserPassword();

            ViewData["Exist"] =
               message == MessageNote.Exist ? "User Password Reset"
               : "";

            listuser.ApplicationUser = await _userManager.Users
                                .Include(s => s.Department)
                            .SingleOrDefaultAsync(s => s.Id == Id);

            return View(listuser);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotUserPassword(string Id, ListGetAUserPassword listGetAUserPassword)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.Users
                                .Include(s => s.Department)
                            .SingleOrDefaultAsync(s => s.Id == Id);

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, code, listGetAUserPassword.UserForgetPassword.Newpassword);

                if (result.Succeeded)
                {
                    return RedirectToAction("ForgotUserPassword", new { id = Id, Message = MessageNote.Exist });
                }

                AddErrors(result);
            }

            return View(listGetAUserPassword);
        }

        private void listItem()
        {
            ViewData["DepartmentID"] = new SelectList(_context.departments.OrderBy(m => m.Name), "DepartmentID", "Name");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        public enum MessageNote
        {
            Add,
            Exist,
            Update
        }
    }
}