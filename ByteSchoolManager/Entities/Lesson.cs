using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("Lessons")]
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        public required int TimeTableLessonId { get; set; }

        public required DateTime DayOfWorkedLesson { get; set; }

        public required int CoachId { get; set; }

        public List<Student> Students { get; set; }


        [ForeignKey(nameof(TimeTableLessonId))]
        public Course Course { get; set; }


        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; }
    }
}
