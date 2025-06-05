using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        public int Id { get; set; }

        public required DayOfWeek DayOfWeekend { get; set; }

        public required TimeOnly TimeOfLesson { get; set; }  

        public required DateOnly TimeOfStartCourse { get; set; }

        public required DateOnly TimeOfEndCourse { get; set; }

        public required string Title { get; set; }

        public required int CoachId { get; set; }
        
        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; }
       
        public required List<Student> Students { get; set; }
    }
}
