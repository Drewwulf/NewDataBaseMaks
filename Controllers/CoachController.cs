using MaksGym.Models;
using MaksGym.Models.ViewModels;
using MaksGym.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MaksGym.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CoachController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public CoachController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var model = new CoachViewModel
            {
                NewCoach = new Coach(),
                CoachList = await _context.Coaches.Include(c => c.User).ToListAsync(),
                Users = await _context.Users.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoachViewModel coachVM, IFormFile? PhotoFile)
        {
            var user = await _userManager.FindByIdAsync(coachVM.NewCoach.UserId);

            if (user == null)
            {
                ModelState.AddModelError("NewCoach.UserId", "Користувача не знайдено.");
            }
            else
            {
                bool exists = await _context.Coaches.AnyAsync(c => c.UserId == coachVM.NewCoach.UserId && !c.IsDeleted);
                if (exists)
                {
                    ModelState.AddModelError("NewCoach.UserId", "Цей користувач вже доданий як тренер.");
                }

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(r => r != "Coach")) 
                {
                    ModelState.AddModelError("NewCoach.UserId", "Користувач вже має іншу роль.");
                }
            }

            if (!ModelState.IsValid)
            {
                coachVM.CoachList = await _context.Coaches.Include(c => c.User).ToListAsync();
                coachVM.Users = await _context.Users.ToListAsync();
                return View("Index", coachVM);
            }

            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PhotoFile.CopyToAsync(stream);
                }

                coachVM.NewCoach.PhotoPath = "/uploads/" + fileName;
            }

            _context.Coaches.Add(coachVM.NewCoach);
            await _context.SaveChangesAsync();

            await _userManager.AddToRoleAsync(user, "Coach");

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach != null)
            {
                coach.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
