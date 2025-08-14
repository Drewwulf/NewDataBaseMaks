using MaksGym.Data;
using MaksGym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class RoomController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoomController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Room room)
    {
        if (ModelState.IsValid)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(room);
    }
}
