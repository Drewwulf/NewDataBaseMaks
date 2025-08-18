using MaksGym.Data;
using MaksGym.Models;
using MaksGym.Models.ViewModels;
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

    public IActionResult Index()
    {

        var model = new RoomViewModel
        {
            NewRoom = new Room(), 
            RoomList  = _context.Rooms.ToList(),
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Room newRoom)
    {
        if (ModelState.IsValid)
        {
            _context.Rooms.Add(newRoom);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        var model = new RoomViewModel
        {
            NewRoom = newRoom,
            RoomList = _context.Rooms.ToList()
        };
        return View("Index", model);
    }


}
