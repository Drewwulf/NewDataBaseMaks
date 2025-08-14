using MaksGym.Data;
using MaksGym.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class DirectionController : Controller
{
    private readonly ApplicationDbContext _context;

    public DirectionController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Direction direction)
    {
        if (ModelState.IsValid)
        {
            _context.Directions.Add(direction);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(direction);
    }
}
