using MaksGym.Data;
using MaksGym.Migrations;
using MaksGym.Models;
using MaksGym.Models.ViewModels;
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

    public IActionResult Index()
    {
        var direction = new DirectionViewModel
        {
            direction = new Direction(),
            directions = _context.Directions.ToList()

        };

        return View(direction);
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
        var model = new DirectionViewModel
        {
            direction = new Direction(),
            directions = _context.Directions.ToList()

        };
        return View("Index",model);
    }
    [HttpGet]
    public IActionResult Delete(int id) {

        var direction = _context.Directions.Find(id);
        if (direction != null)
        {
            direction.isDeleted = true;
            _context.SaveChanges(); return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));

    }
}
