using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("StudentLessons")]
    public class StudentLesson
    {
        public enum StudentStatus
        {
            Present,
            AbsentWithoutExcuse,
            AbsentRespect,
            Online
        }

        [Key]
        public int Id { get; set; }

        public required int StudentId { get; set; }

        public required int LessonId { get; set; }

        public required StudentStatus Status { get; set; }
        
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; }
    }


}
