using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("StudentWorkedLessons")]
    public class StudentWorkedLesson
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

        public required int WorkedLessonId { get; set; }

        public required StudentStatus Status { get; set; }
        
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [ForeignKey(nameof(WorkedLessonId))]
        public WorkedLesson WorkedLesson { get; set; }
    }


}
