using Microsoft.EntityFrameworkCore;
using Taskmanager_MVCAPI.Models;

namespace Taskmanager_MVCAPI.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<Batch> Batches { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<NewTask> NewTasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewTask>()
                .Property(t => t.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<NewTask>().Property(t => t.DueDate).HasDefaultValueSql("DATEADD(HOUR, 24, GETDATE())");

            SeedData.Seed(modelBuilder);

            modelBuilder.Entity<Batch>().HasMany(b => b.Students).WithOne(s => s.Batch).HasForeignKey(s => s.BatchId);
            modelBuilder.Entity<NewTask>().HasMany(t => t.TaskAssignments).WithOne(nt => nt.Task).HasForeignKey(s => s.TaskId);
            modelBuilder.Entity<TaskAssignment>().HasOne(ta => ta.Task).WithMany(t => t.TaskAssignments).HasForeignKey(ta => ta.TaskId);

            modelBuilder.Entity<TaskAssignment>().HasOne(ta => ta.Student).WithMany(s => s.TaskAssignments).HasForeignKey(ta => ta.StudentId);
        }

    }
}
