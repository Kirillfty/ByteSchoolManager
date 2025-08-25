using ByteSchoolManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace ByteSchoolManager
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<StudentLesson> StudentLessons { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Students);

            modelBuilder.Entity<StudentLesson>()
                .HasOne(s => s.Lesson)
                .WithMany(c => c.Students);
        }
    }
}
