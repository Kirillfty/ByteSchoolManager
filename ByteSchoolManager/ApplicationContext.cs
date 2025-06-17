using ByteSchoolManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<StudentLesson> StudentLessons { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Student>()
               .HasMany(c => c.Courses)
               .WithMany(u => u.Students)
               .UsingEntity<StudentCourse>();

            modelBuilder.Entity<Student>()
               .HasMany(c => c.Lessons)
               .WithMany(u => u.Students)
               .UsingEntity<StudentLesson>();
        }
    }
}
