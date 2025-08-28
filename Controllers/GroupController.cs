using MaksGym.Data;
using MaksGym.Models;
using MaksGym.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MaksGym.Controllers
{
    [Authorize(Roles = "Admin")]

    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public GroupController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var model = new GroupViewModel
            {
                Groups = await _context.Groups
                    .Where(g => !g.IsDeleted)
                    .Include(g => g.Direction)
                    .ToListAsync(),

                Directions = await _context.Directions
                    .Where(d => !d.isDeleted)
                    .ToListAsync(),

                Coaches = await _context.Coaches
                    .Include(c => c.User)
                    .Where(c => !c.IsDeleted)
                    .ToListAsync(),

                GroupName = string.Empty,
                CoachId = 0,
                DirectionId = 0,
                GroupDescription = string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupViewModel group)
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                       .Select(x => new { x.Key, x.Value.Errors })
                       .ToList();

            var createdGroup = new Group
            {
                GroupName = group.GroupName,
                CoachId = group.CoachId,
                DirectionId = group.DirectionId,
                GroupDescription = group.GroupDescription,
            };
            if (ModelState.IsValid)
            {
                _context.Groups.Add(createdGroup);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            var model = new GroupViewModel
            {
                Groups = await _context.Groups.Where(g => !g.IsDeleted).ToListAsync(),
                Directions = await _context.Directions.Where(d => !d.isDeleted).ToListAsync(),

                Coaches = await _context.Coaches.Include(c => c.User).Where(c => !c.IsDeleted).ToListAsync(),

            };
            return View("Index", model);
        }
        public IActionResult Delete(int id)
        {
            var group = _context.Groups.Find(id);
            if (group != null)
            {
                group.IsDeleted = true;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var group = new StudentsInGroupViewModel
            {
                Students = await _context.Students
                    .Include(s => s.User)
                    .Where(s => !s.IsDeleted)
                    .ToListAsync(),
                StudentInGroup = await _context.StudentToGroups
                    .Include(sg => sg.Student)
                    .ThenInclude(s => s.User)
                    .Where(sg => sg.GroupsId == id)
                    .ToListAsync(),
                GroupId = id,
                NewGroup = await _context.Groups
                    .Include(g => g.Coach)
                    .ThenInclude(c => c.User)
                    .Include(g => g.Direction)
                    .FirstOrDefaultAsync(g => g.GroupsId == id) ?? new Group()

            };
           
            return View(group);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(StudentsInGroupViewModel students)
        {
            if(ModelState.IsValid)
            {
               _context.StudentToGroups.Add(new StudentToGroup
               {
                   StudentId = students.UserId.Value,
                   GroupsId = students.GroupId
               });
                await _context.SaveChangesAsync();
            }

            var group = new StudentsInGroupViewModel
            {
                Students = await _context.Students
                       .Include(s => s.User)
                       .Where(s => !s.IsDeleted)
                       .ToListAsync(),
                StudentInGroup = await _context.StudentToGroups
                       .Include(sg => sg.Student)
                       .ThenInclude(s => s.User)
                       .Where(sg => sg.GroupsId == students.GroupId)
                       .ToListAsync(),
                GroupId = students.GroupId,
                NewGroup = await _context.Groups
                       .Include(g => g.Coach)
                       .ThenInclude(c => c.User)
                       .Include(g => g.Direction)
                       .FirstOrDefaultAsync(g => g.GroupsId == students.GroupId) ?? new Group()

            };
            return View("Details",group);
        }
    }
}