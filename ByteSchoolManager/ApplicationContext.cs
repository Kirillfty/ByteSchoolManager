using ByteSchoolManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<TimeTableLesson> TimetableLessons { get; set; }
        public DbSet<StudentTimeTableLesson> StudentTimetableLessons { get; set; }
        public DbSet<WorkedLesson> WorkedLessons { get; set; }
        public DbSet<StudentWorkedLesson> StudentWorkedLessons { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Student>()
               .HasMany(c => c.Lessons)
               .WithMany(u => u.Students)
               .UsingEntity<StudentTimeTableLesson>();

            modelBuilder.Entity<Student>()
               .HasMany(c => c.WorkedLessons)
               .WithMany(u => u.Students)
               .UsingEntity<StudentWorkedLesson>();
        }
    }
}
