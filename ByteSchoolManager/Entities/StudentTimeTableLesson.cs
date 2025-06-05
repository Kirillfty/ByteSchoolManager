using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("StudentTimeTableLessons")]
    public class StudentTimeTableLesson
    {
        [Key]
        public int Id { get; set; }

        public required int  StudentId { get; set; }

        public required int TimetableLessonsId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student student { get; set; }

        [ForeignKey(nameof(TimetableLessonsId))]
        public TimeTableLesson TimetableLessons { get; set; }


    }
}
