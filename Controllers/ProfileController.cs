using MaksGym.Data;
using MaksGym.Models.ViewModels;
using MaksGym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MaksGym.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.IsInRole("Admin"))
            {
                ViewData["Layout"] = "~/Views/Shared/_AdminLayout.cshtml";
            }
            else if (User.IsInRole("Student"))
            {
                ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
            }
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            else if (User.IsInRole("Coach"))
            {
                return RedirectToAction("Index", "CoachProfile");
            }
            else
            {
                var student = await _context.Students
                    .Include(s => s.Transactions)
                    .Include(s => s.StudentsToSubscriptions)
                        .ThenInclude(sts => sts.Subscription)
                    .Include(s => s.StudentsToSubscriptions)
                        .ThenInclude(sts => sts.Freezes)
                    .FirstOrDefaultAsync(s => s.UserId == userId);

                if (student == null)
                    return NotFound("Студент не знайдений");

                var groups = await _context.Groups
                    .Where(g => _context.StudentToGroups
                        .Any(sg => sg.StudentId == student.StudentId && sg.GroupsId == g.GroupsId))
                    .Include(g => g.Direction)
                    .Include(g => g.Coach)
                        .ThenInclude(c => c.User)
                    .Include(g => g.Shedules)
                    .ToListAsync();

                var trainings = await _context.Trainings
                    .Where(t => t.StudentId == student.StudentId)
                    .ToListAsync();

                var model = new ProfileViewModel
                {
                    UserName = User.Identity!.Name!,
                    PhotoUrl = student.PhotoPath,
                    groups = groups,
                    subscriptions = student.StudentsToSubscriptions.Select(sts => sts.Subscription).ToList(),
                    studentsToSubscriptions = student.StudentsToSubscriptions.ToList(),
                    transactions = student.Transactions.ToList(),
                    trainings = trainings
                };

                return View(model);
            }
        }
    }
}
