using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("Courses")]
    public class Course
    {
        [Flags]
        public enum DayOfWeek
        {
            Sunday = 1,
            Monday = 2,
            Tuesday = 4,
            Wednesday = 8,
            Thursday = 16,
            Friday = 32,
            Saturday = 64
        }

        [Key]
        public int Id { get; set; }

        public required DayOfWeek DaysOfWeek { get; set; }

        public required TimeOnly TimeOfLesson { get; set; }  

        public required DateOnly DateOfStartCourse { get; set; }

        public required DateOnly DateOfEndCourse { get; set; }

        public required string Title { get; set; }

        public required int CoachId { get; set; }
        
        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; }
       
        public required List<Student> Students { get; set; }
    }
}
