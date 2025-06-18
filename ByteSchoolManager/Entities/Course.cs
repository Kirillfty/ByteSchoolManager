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
            None = 0,
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

        public  DayOfWeek DaysOfWeek { get; set; }

        public  TimeOnly TimeOfLesson { get; set; }  

        public  DateOnly DateOfStartCourse { get; set; }

        public  DateOnly DateOfEndCourse { get; set; }

        public string Title { get; set; }

        public int CoachId { get; set; }
        
        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; }
       
        public  List<Student> Students { get; set; }
    }
}
