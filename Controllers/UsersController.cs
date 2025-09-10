using MaksGym.Models.AccountViewModels;
using MaksGym.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MaksGym.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

 
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();

            var model = new RegisterViewModel
            {
                Users = users 
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Users = _userManager.Users.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.PhoneNumber,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Email = $"{model.PhoneNumber}@example.com"
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Users");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            model.Users = _userManager.Users.ToList();

            return View("Register", model);
        }
    }
}
