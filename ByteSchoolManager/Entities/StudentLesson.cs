using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ByteSchoolManager.Common.Abstractions;

namespace ByteSchoolManager.Entities
{
    [Table("StudentLessons")]
    public class StudentLesson : IDbEntity
    {
        public enum StudentLessonStatus
        {
            InClass,
            Miss,
            MissNoReason,
            Online
        }

        [Key]
        public int Id { get; set; }
        public required int StudentId { get; set; }
        public required int LessonId { get; set; }
        public required StudentLessonStatus LessonStatus { get; set; }
        
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; }
    }
}
