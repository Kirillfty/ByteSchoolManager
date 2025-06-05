using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("WorkedLessons")]
    public class WorkedLesson
    {
        [Key]
        public int Id { get; set; }

        public required int TimeTableLessonId { get; set; }

        public required DateTime DayOfWorkedLesson { get; set; }

        public required int CoachId { get; set; }

        public List<Student> Students { get; set; }


        [ForeignKey(nameof(TimeTableLessonId))]
        public TimeTableLesson TimeTableLesson { get; set; }


        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; }
    }
}
