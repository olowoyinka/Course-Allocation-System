using MappingLectureCourse.Data;
using MappingLectureCourse.Models.UserViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MappingLectureCourse.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ILogger _logger;

        public HomeController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<UsersController>();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserLogin userLogin)
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

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
