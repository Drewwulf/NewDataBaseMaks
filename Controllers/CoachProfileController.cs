using MaksGym.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MaksGym.Controllers
{

    [Authorize(Roles = "Coach")]
    public class CoachProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoachProfileController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var coach = _context.Coaches.Include(c => c.User)
                .FirstOrDefault(c => c.UserId == userId && !c.IsDeleted);
            var groups = _context.Groups
                .Where(g => g.CoachId == coach.CoachId && !g.IsDeleted)
                .Include(g => g.Direction)
                .Include(g => g.Shedules)
                .ToList();
            var model = new Models.ViewModels.CoachProfileViewModel()
            {
                Coach = coach
            };



            return View(model);
        }
    }
}
