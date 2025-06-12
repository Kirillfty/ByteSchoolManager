using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("Lessons")]
    public class Lesson
    {
        public enum LessonStatus
        {
            NotDone,
            Сanceled,
            Done,
            Mooved,
            Replaced
        }
        [Key]
        public int Id { get; set; }

        public required int CourseId { get; set; }

        public required DateTime DateAndTime { get; set; }

        public required int CoachId { get; set; }

        public List<Student> Students { get; set; }
        public LessonStatus Status { get; set; }


        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }


        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; }
    }
}
