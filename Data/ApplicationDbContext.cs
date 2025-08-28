using MaksGym.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MaksGym.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Coach> Coaches => Set<Coach>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<StudentToGroup> StudentToGroups => Set<StudentToGroup>();
        public DbSet<Direction> Directions => Set<Direction>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Schedule> Shedules => Set<Schedule>();

        // Нові сутності
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<StudentsToSubscription> StudentsToSubscriptions => Set<StudentsToSubscription>();
        public DbSet<SubscriptionFreezeTime> SubscriptionFreezeTimes => Set<SubscriptionFreezeTime>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // Students
            b.Entity<Student>(e =>
            {
                e.ToTable("Students");
                e.HasKey(x => x.StudentId);
                e.Property(x => x.UserId).IsRequired();
                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Coaches
            b.Entity<Coach>(e =>
            {
                e.ToTable("Coaches");
                e.HasKey(x => x.CoachId);
                e.Property(x => x.UserId).IsRequired();
                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Groups
            b.Entity<Group>(e =>
            {
                e.ToTable("Groups");
                e.HasKey(x => x.GroupsId);
                e.Property(x => x.GroupName).IsRequired();

                e.HasOne(x => x.Coach)
                 .WithMany(c => c.Groups)
                 .HasForeignKey(x => x.CoachId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Direction)
                 .WithMany(d => d.Groups)
                 .HasForeignKey(x => x.DirectionId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Directions
            b.Entity<Direction>(e =>
            {
                e.ToTable("Directions");
                e.HasKey(x => x.DirectionId);
                e.Property(x => x.DirectionName).IsRequired();
            });

            // Rooms
            b.Entity<Room>(e =>
            {
                e.ToTable("Rooms");
                e.HasKey(x => x.RoomId);
                e.Property(x => x.RoomName).IsRequired();
            });

            // Shedules
            b.Entity<Schedule>(e =>
            {
                e.ToTable("Shedules");
                e.HasKey(x => x.SheduleId);

                e.HasOne(x => x.Group)
                 .WithMany(g => g.Shedules)
                 .HasForeignKey(x => x.GroupsId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Room)
                 .WithMany(r => r.Shedules)
                 .HasForeignKey(x => x.RoomId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // StudentToGroup
            b.Entity<StudentToGroup>(e =>
            {
                e.ToTable("StudentToGroup");
                e.HasKey(x => x.StudentToGroupId);

                e.HasOne(x => x.Student)
                 .WithMany(s => s.StudentToGroups)
                 .HasForeignKey(x => x.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Group)
                 .WithMany(g => g.StudentToGroups)
                 .HasForeignKey(x => x.GroupsId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(x => new { x.StudentId, x.GroupsId }).IsUnique();
            });

            // Subscriptions
            b.Entity<Subscription>(e =>
            {
                e.ToTable("Subscriptions");
                e.HasKey(x => x.SubscriptionId);
                e.Property(x => x.Name).IsRequired();
                e.Property(x => x.Price).HasColumnType("decimal(18,2)");
            });

            // StudentsToSubscription
            b.Entity<StudentsToSubscription>(e =>
            {
                e.ToTable("StudentsToSubscriptions");
                e.HasKey(x => x.StudentsToSubscriptionId);

                e.HasOne(x => x.Student)
                    .WithMany(s => s.StudentsToSubscriptions)
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Subscription)
                    .WithMany(s => s.StudentsToSubscriptions)
                    .HasForeignKey(x => x.SubscriptionId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Transaction)
                    .WithMany(t => t.StudentsToSubscriptions)
                    .HasForeignKey(x => x.TransactionId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // SubscriptionFreezeTime
            b.Entity<SubscriptionFreezeTime>(e =>
            {
                e.ToTable("SubscriptionFreezeTimes");
                e.HasKey(x => x.SubscriptionFreezeTimeId);

                e.HasOne(x => x.StudentsToSubscription)
                    .WithMany(s => s.Freezes)
                    .HasForeignKey(x => x.StudentsToSubscriptionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Transactions
            b.Entity<Transaction>(e =>
            {
                e.ToTable("Transactions");
                e.HasKey(x => x.TransactionId);

                e.HasOne(x => x.Student)
                    .WithMany(s => s.Transactions)
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.Property(x => x.Value).HasColumnType("decimal(18,2)");
            });
        }
    }
}
