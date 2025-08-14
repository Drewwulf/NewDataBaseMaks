using MaksGym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MaksGym.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    public async Task<IActionResult> Create()
    {
        ViewBag.UsersList = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Coach coach, IFormFile? PhotoFile)
    {
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

                coach.PhotoPath = "/uploads/" + fileName;
            }

            _context.Coaches.Add(coach);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.UsersList = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName", coach.UserId);
        return View(coach);
    }
}
