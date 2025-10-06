using MaksGym.Models;
using MaksGym.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaksGym.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            
            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserDetailViewModel
            {
                User = new UserDetailViewModel.ApplicationUser
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Role = roles.FirstOrDefault() ?? "Student",
                }
            };

            return View("~/Views/Users/Details.cshtml", model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDetailViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Details", vm);

            var user = await _userManager.FindByIdAsync(vm.User.Id);
            if (user == null) return NotFound();

            user.FullName = vm.User.FullName;
            user.PhoneNumber = vm.User.PhoneNumber;
            user.DateOfBirth = vm.User.DateOfBirth;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);

                return View("~/Views/Users/Details.cshtml");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(vm.User.Role))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, vm.User.Role);
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Details", new {id = user.Id});
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
                    

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Profile");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string phoneNumber, string password)
        {
            var user = await _userManager.FindByNameAsync(phoneNumber); 

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                    
                    return RedirectToAction("Index", "Profile");
            }

            ModelState.AddModelError("", "Невірний номер телефону або пароль");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
