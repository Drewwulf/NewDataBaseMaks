using MaksGym.Data;
using MaksGym.Models;
using MaksGym.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    [HttpGet]
    public IActionResult Delete(int id)
    {  

        var room = _context.Rooms.Find(id);
        if (room != null) { 
        room.IsDeleted = true;
            _context.SaveChanges(); return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Details(int id)
    {
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.RoomId == id && !r.IsDeleted);

        if (room == null)
            return NotFound();

        var vm = new RoomViewModel { NewRoom = room };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(RoomViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var room = await _context.Rooms.FindAsync(vm.NewRoom.RoomId);
            if (room == null) return NotFound();

            room.RoomName = vm.NewRoom.RoomName;
            room.RoomDescription = vm.NewRoom.RoomDescription;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View("Details", vm);
    }

}
