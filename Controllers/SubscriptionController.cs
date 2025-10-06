using MaksGym.Data;
using MaksGym.Models;
using MaksGym.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public async Task<IActionResult> Details(int subscriptionId)
        {
          
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.SubscriptionId == subscriptionId && !s.IsDeleted);

            if (subscription == null)
                return NotFound(); 

            var model = new SubscriptionDetailViewModels
            {
                CurrentSubscription = subscription
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubscriptionDetailViewModels vm)
        {
            if (ModelState.IsValid)
            {
                var subscription = await _context.Subscriptions.FindAsync(vm.CurrentSubscription.SubscriptionId);
                if (subscription == null) return NotFound();

                subscription.Name = vm.CurrentSubscription.Name;
                subscription.DurationInDays = vm.CurrentSubscription.DurationInDays;
                subscription.Price = vm.CurrentSubscription.Price;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View("Details", vm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePrice(SubscriptionDetailViewModels model)
        {
           
        
                var subscription = await _context.Subscriptions
                   .FirstOrDefaultAsync(s => s.SubscriptionId == model.CurrentSubscription.SubscriptionId);

                if (subscription != null)
                {
                    subscription.Price = model.CurrentSubscription.Price;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { subscriptionId = subscription.SubscriptionId });
                }
            
            return View("Details", new { subscriptionId = model.CurrentSubscription.SubscriptionId });
        }


    }
}
