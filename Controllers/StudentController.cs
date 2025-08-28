using MaksGym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MaksGym.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using MaksGym.Models.ViewModels;

[Authorize(Roles = "Admin")]
public class StudentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
    {
        _context = context;
        _userManager = userManager;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var model = new StudentViewModel
        {
            NewStudent = new Student(),
            students = await _context.Students.ToListAsync(),
            Users = await _context.Users.ToListAsync(),
        };
        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentViewModel student, IFormFile? PhotoFile)
    {
        var user = await _userManager.FindByIdAsync(student.NewStudent.UserId);
        if (user == null)
        {
            ModelState.AddModelError("NewStudent.UserId", "Користувача не знайдено.");
        }
        else
        {
            bool exists = await _context.Students.AnyAsync(c => c.UserId == student.NewStudent.UserId && !c.IsDeleted);
            if (exists)
            {
                ModelState.AddModelError("NewStudent.UserId", "Цей користувач вже доданий як студент");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(r => r != "Student"))
            {
                ModelState.AddModelError("NewStudent.UserId", "Користувач вже має іншу роль.");
            }
        }
        if (ModelState.IsValid)
        {
            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await PhotoFile.CopyToAsync(fileStream);
                }

                student.NewStudent.PhotoPath = "/uploads/" + fileName;
            }

            _context.Students.Add(student.NewStudent);
            await _context.SaveChangesAsync();

            if (user != null)
            {
                if (!await _userManager.IsInRoleAsync(user, "Student"))
                {
                    await _userManager.AddToRoleAsync(user, "Student");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.UsersList = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName", student.NewStudent.UserId);
        var model = new StudentViewModel
        {
            NewStudent = new Student(),
            students = await _context.Students.ToListAsync(),
            Users = await _context.Users.ToListAsync(),
        };
        return View("Index", model);
    }
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var student = _context.Students.Find(id);
        if (student != null)
        {
            student.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }
}
