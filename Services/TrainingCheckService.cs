using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MaksGym.Data;
using MaksGym.Models;
using Microsoft.EntityFrameworkCore;

public class TrainingCheckService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _scopeFactory;

    public TrainingCheckService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    private void DoWork(object state)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var today = DateTime.Today;

            var activeSubs = _context.StudentsToSubscriptions
                .Where(sts => sts.EndDate >= today)
                .Include(sts => sts.Student)
                .Include(sts => sts.Subscription)
                .ToList();

            foreach (var sub in activeSubs)
            {
                var schedules = _context.Shedules
                    .Include(s => s.Group)
                        .ThenInclude(g => g.Coach)
                    .Include(s => s.Group)
                        .ThenInclude(g => g.Direction)
                    .Include(s => s.Room)
                    .Where(s => s.GroupsId == sub.GroupId && !s.IsDeleted)
                    .ToList();

                DateTime date = today;
                while (date <= sub.EndDate)
                {
                    var currentDay = (int)date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek;

                    var todaysSchedules = schedules
                        .Where(s => (int)s.DayOfWeek == currentDay)
                        .ToList();

                    foreach (var schedule in todaysSchedules)
                    {
                       
                        var trainingDateTime = date.Date + schedule.StartTime;


                        bool trainingExists = _context.Trainings
                            .Any(t => t.StudentId == sub.StudentId
                                   && t.StartTime == trainingDateTime
                                   && t.SheduleId == schedule.SheduleId);

                        if (!trainingExists)
                        {
                            var training = new Training
                            {
                                StudentId = sub.StudentId,
                                SheduleId = schedule.SheduleId,
                                StartTime = trainingDateTime, 
                                CoachId = schedule.Group.CoachId,
                                RoomId = schedule.RoomId,
                                subscriptionId = sub.SubscriptionId,
                                directionId = schedule.Group.DirectionId,
                                IsConducted = false
                            };

                            _context.Trainings.Add(training);
                        }

                        
                    }

                    date = date.AddDays(1);
                }
            }

            
            var pastTrainings = _context.Trainings
                .Where(t => t.StartTime < DateTime.Now && !t.IsConducted)
                .ToList();

            foreach (var training in pastTrainings)
            {
                training.IsConducted = true;

                var student = _context.Students.Find(training.StudentId);
                var subscription = _context.Subscriptions.Find(training.subscriptionId);
                var studentToSubscription = _context.StudentsToSubscriptions
                    .FirstOrDefault(sts => sts.StudentId == training.StudentId && sts.SubscriptionId == training.subscriptionId && !sts.IsDeleted);

                if (student == null || subscription == null)
                    continue;

                if (studentToSubscription != null && studentToSubscription.IsFrozen != true)
                {
                    if (studentToSubscription.ActiveSessions > 0)
                    {
                        studentToSubscription.ActiveSessions -= 1;
                        student.Balance -= subscription.Price * (student.discount / 100);
                        _context.Transactions.Add(new Transaction
                        {
                            StudentId = student.StudentId,
                            Value = -subscription.Price * (student.discount / 100),
                            PaymentDate = DateTime.Now, 
                           
                        });
                    }
                    else
                        continue;
                }
            }

            _context.SaveChanges();
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
