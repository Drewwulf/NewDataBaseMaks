using MaksGym.Data;
using MaksGym.Models.ViewModels;
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
                return RedirectToAction("Index", "CoachDashboard");
            }
            else
            {
                var student = await _context.Students
          .Include(s => s.User)
          .FirstOrDefaultAsync(s => s.UserId == userId);


                var groupsWithDirections = await _context.StudentToGroups
         .Where(stg => stg.Student.UserId == userId && !stg.IsDeleted && !stg.Group.IsDeleted)
         .Select(stg => new
         {
             GroupName = stg.Group.GroupName,
             DirectionName = stg.Group.Direction.DirectionName
         })
         .ToListAsync();

                var model = new ProfileViewModel
                {
                    UserName = User.Identity!.Name!,
                    Groups = groupsWithDirections.Select(g => g.GroupName).ToList(),
                    Directions = groupsWithDirections.Select(g => g.DirectionName).Distinct().ToList()
                    ,
                    groups = await _context.Groups
                    .Include(sh => sh.Shedules)
                        .Include(g => g.Direction)
                        .Include(g => g.Coach)
                        .ThenInclude(c => c.User)
                        .Where(g => !g.IsDeleted)
                        .ToListAsync()
                };

                return View(model);
            }
        }
    }
}
