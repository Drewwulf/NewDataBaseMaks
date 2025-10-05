using MaksGym.Data;
using MaksGym.Models;
using MaksGym.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaksGym.Controllers
{
    [Authorize(Roles = "Admin")]

    public class SheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int groupId)
        {
            var group = _context.Groups
                                .Include(g => g.Coach)
                                .FirstOrDefault(g => g.GroupsId == groupId);

            if (group == null)
                return NotFound();

            var model = new SheduleViewModel
            {
                GroupId = groupId,
                Rooms = _context.Rooms.Where(r => !r.IsDeleted).ToList(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Rooms = _context.Rooms.ToList();
                return View(model);
            }

            bool conflict = _context.Shedules.Any(s =>
                s.RoomId == model.RoomId &&
                s.DayOfWeek == model.DayOfWeek &&
                ((model.StartTime >= s.StartTime && model.StartTime < s.EndTime) ||
                 (model.EndTime > s.StartTime && model.EndTime <= s.EndTime))
            );

            if (conflict)
            {
                TempData["ErrorMessage"] = "У цьому залі вже є заняття в обраний час!";
                model.Rooms = _context.Rooms.ToList();
                return RedirectToAction("Details", "Group", new { id = model.GroupId });
            }

            var schedule = new Schedule
            {
                GroupsId = model.GroupId,
                RoomId = model.RoomId,
                DayOfWeek = model.DayOfWeek,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                IsDeleted = false
            };

            _context.Shedules.Add(schedule);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Заняття додано успішно!";
            return RedirectToAction("Details", "Group", new { id = model.GroupId });
        }

    }
}
