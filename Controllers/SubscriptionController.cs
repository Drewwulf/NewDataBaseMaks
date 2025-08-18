using MaksGym.Data;
using MaksGym.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MaksGym.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Subscriptions.Add(subscription);
                _context.SaveChanges();
                await _context.SaveChangesAsync();
                return View(subscription);  
            }



            return View();
        }
    }
}
