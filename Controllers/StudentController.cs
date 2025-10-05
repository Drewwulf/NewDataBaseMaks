using MaksGym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MaksGym.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using MaksGym.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

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

    private DateTime CalculateEndDate(DateTime start, int sessions, List<DayOfWeek> lessonDays)
    {
        int counted = 0;
        DateTime date = start;
        while (counted < sessions)
        {
            if (lessonDays.Contains(date.DayOfWeek))
                counted++;
            date = date.AddDays(1);

        }
        return date.AddDays(-1);
    }
    public async Task<IActionResult> Details(int id)
    {
        var student = _context.Students
            .Include(s => s.User).Include(ss => ss.StudentsToSubscriptions).Include(sg => sg.StudentToGroups).ThenInclude(g => g.Group).ThenInclude(sh => sh.Shedules).Include(ss => ss.StudentsToSubscriptions).ThenInclude(sts => sts.Subscription).Include(ss => ss.StudentsToSubscriptions).ThenInclude(sts => sts.Freezes)
            .FirstOrDefault(s => s.StudentId == id);







        if (student == null)
        {
            return NotFound("Студент не знайдений");
        }
        var groups = student.StudentToGroups.Select(s => s.Group).ToList();

        var hasFrozen = await _context.StudentsToSubscriptions
            .Include(sts => sts.Freezes)
            .AnyAsync(sts => sts.StudentId == id && sts.Freezes.Any(f => f.FreezeEnd > DateTime.Now));
        foreach (var sts in student.StudentsToSubscriptions)
        {
            var upcomingLessons = await _context.Shedules
    .Where(s => s.GroupsId == sts.GroupId && !s.IsDeleted)
    .ToListAsync();
            var lessonDays = upcomingLessons
     .Select(s => (DayOfWeek)s.DayOfWeek)
     .ToList();
            DateTime today = DateTime.Today;
            DateTime dateTime = sts.StartDate;
            int countedSessions = 0;
            while (countedSessions < sts.ActiveSessions)
            {
                if (lessonDays.Contains(dateTime.DayOfWeek))
                {
                    countedSessions++;
                }
                dateTime = dateTime.AddDays(1);
            }

            DateTime subscriptionEndDate = dateTime.AddDays(-1);
            sts.EndDate = subscriptionEndDate;
            _context.SaveChanges();
        }

        foreach (var sts in student.StudentsToSubscriptions)
        {
            sts.EndDate = CalculateEndDate(sts.StartDate, sts.ActiveSessions, student.StudentToGroups.FirstOrDefault(sg => sg.GroupsId == sts.GroupId)?.Group.Shedules.Select(sh => (DayOfWeek)sh.DayOfWeek).ToList() ?? new List<DayOfWeek>());

        }
        var model = new StudentDetailsViewModel
        {
            NewStudent = student,
            Subscriptions = await _context.Subscriptions.Where(s => !s.IsDeleted).ToListAsync(),

            StudentGroup = groups,

        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSubscription(StudentDetailsViewModel studentDetailsViewModel)
    {
        var student = await _context.Students
            .Include(s => s.StudentsToSubscriptions).Include(s => s.User).Include(g => g.StudentToGroups).ThenInclude(s => s.Group)
            .Where(s => !s.IsDeleted)
            .FirstOrDefaultAsync(s => s.StudentId == studentDetailsViewModel.NewStudent.StudentId);

       
        if (student == null)
        {
            return NotFound("Студент не знайдений");
        }
        var subscription = await _context.Subscriptions.FindAsync(studentDetailsViewModel.SubscriptionId);
        if (subscription == null)
        {
            return NotFound("Абонемент не знайдений");
        }

        var upcomingLessons = await _context.Shedules
     .Where(s => s.GroupsId == studentDetailsViewModel.GroupId && !s.IsDeleted)
     .ToListAsync();
        DateTime today = DateTime.Today;
        DateTime nearestLessonDate = today.AddDays(7);

        foreach (var s in upcomingLessons)
        {
            int daysUntilLesson = ((int)s.DayOfWeek - (int)today.DayOfWeek + 7) % 7;
            DateTime lessonDate = today.AddDays(daysUntilLesson);
            if (lessonDate < nearestLessonDate)
                nearestLessonDate = lessonDate;

        }
        if (student.StudentsToSubscriptions.Any(sts => sts.SubscriptionId == subscription.SubscriptionId && sts.EndDate > DateTime.Now))
        {
            ModelState.AddModelError("SubscriptionId", "Цей студент вже має цю підписку");

            var model = new StudentDetailsViewModel
            {
                NewStudent = student,
                Subscriptions = await _context.Subscriptions.Where(s => !s.IsDeleted).ToListAsync(),
                StudentGroup = student.StudentToGroups.Select(s => s.Group).ToList(),
            };

            return View("Details", model);
        }


        var studentSubscription = new StudentsToSubscription
        {
            StudentId = student.StudentId,
            SubscriptionId = subscription.SubscriptionId,
            StartDate = nearestLessonDate,
            EndDate = DateTime.Now.AddDays(subscription.DurationInDays),
            GroupId = studentDetailsViewModel.GroupId,
            ActiveSessions = studentDetailsViewModel.DaysToPay

        };


        _context.StudentsToSubscriptions.Add(studentSubscription);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { id = student.StudentId });
    }

    [HttpGet]
    public async Task<IActionResult> FrozeSubscription(int studentId, int subscriptionId)
    {
        var studentSubsription = await _context.StudentsToSubscriptions.Include(s => s.Freezes).FirstOrDefaultAsync(sts => sts.StudentId == studentId && sts.SubscriptionId == subscriptionId);
        if (studentSubsription == null)
        {
            return NotFound("Абонемент студента не знайдений");
        }
        _context.SubscriptionFreezeTimes.Add(new SubscriptionFreezeTime
        {
            StudentsToSubscriptionId = studentSubsription.StudentsToSubscriptionId,
            FreezeStart = DateTime.Now,
            FreezeEnd = null
        });
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id = studentId });
    }
    [HttpGet]
    public async Task<IActionResult> UnfrozeSubscription(int studentId, int subscriptionId)
    {
        var studentSubsription = await _context.StudentsToSubscriptions
            .Include(s => s.Freezes)
            .FirstOrDefaultAsync(sts => sts.StudentId == studentId && sts.SubscriptionId == subscriptionId);

        if (studentSubsription == null)
        {
            return NotFound("Абонемент студента не знайдений");
        }

        var currentFreeze = studentSubsription.Freezes
            .FirstOrDefault(f => f.FreezeEnd == null);

        if (currentFreeze != null)
        {
            currentFreeze.FreezeEnd = DateTime.Now;

            var frozenDays = (studentSubsription.EndDate - currentFreeze.FreezeStart).TotalDays;

            studentSubsription.EndDate = DateTime.Now.AddDays(frozenDays);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Details", new { id = studentId });
    }

    [HttpGet]
    public async Task<IActionResult> StudentSubscriptionDetails(int studentId, int subscriptionId)


    {
        var student = await _context.Students.Where(s => s.StudentId == studentId).FirstOrDefaultAsync();
        var subscription = await _context.Subscriptions.Where(s => s.SubscriptionId == subscriptionId).FirstOrDefaultAsync();
        var currentSubsctuptuon = await _context.StudentsToSubscriptions.Where(s => s.SubscriptionId == subscriptionId && s.StudentId == studentId).FirstOrDefaultAsync();
        if (subscription == null || student == null || currentSubsctuptuon == null)
        {
            return NotFound();
        }
        var studentSub = await _context.StudentsToSubscriptions
    .Where(s => s.StudentId == studentId && s.SubscriptionId == subscriptionId)
    .OrderByDescending(s => s.StartDate)
    .FirstOrDefaultAsync();

        if (studentSub != null)
        {
            var subscriptionAtThatTime = await _context.Subscriptions
                .TemporalAsOf(studentSub.StartDate)
                .Where(s => s.SubscriptionId == subscriptionId)
                .FirstOrDefaultAsync();
        }
        var model = new SubscriptionStudentDetailsViewModel
        {
            CurrentStudent = student,
            CurrentSubscription = subscription,
            CurrentStudentsToSubscription = currentSubsctuptuon
        };

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> ExtendSubscription(SubscriptionStudentDetailsViewModel subscriptionDetails)
    {

        var cSub = _context.StudentsToSubscriptions.Find(subscriptionDetails.CurrentStudentsToSubscription.StudentsToSubscriptionId);
        cSub.ActiveSessions += subscriptionDetails.CurrentStudentsToSubscription.ActiveSessions;
        _context.SaveChanges();
        return RedirectToAction("StudentSubscriptionDetails", new { studentId = subscriptionDetails.CurrentStudentsToSubscription.StudentId, subscriptionId = subscriptionDetails.CurrentStudentsToSubscription.SubscriptionId });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBalance(StudentViewModel student)
    {

        return RedirectToAction("Details");
    }
}
