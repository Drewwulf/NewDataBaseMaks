using MaksGym.Data;
using MaksGym.Models;
using MaksGym.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MaksGym.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            

            var model = new SubscriptionViewModel
            {
                subscription = new Subscription(),
                subscriptions = _context.Subscriptions.ToList(),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Subscriptions.Add(subscription);
                _context.SaveChanges();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var model = new SubscriptionViewModel
            {
                subscription = new Subscription(),
                subscriptions = _context.Subscriptions.ToList(),
            };


            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            var subscription = await _context.Subscriptions.FindAsync(id);
    
            if (subscription != null)
            {
                _context.Remove(subscription);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
