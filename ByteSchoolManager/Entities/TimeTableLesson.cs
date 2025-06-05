using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("TimeTableLessons")]
    public class TimeTableLesson
    {
        [Key]
        public int Id { get; set; }

        public required int CoachId { get; set; }

        public required string DayOfWeekend { get; set; }

        public required DateTime TimeOfLesson { get; set; }  

        public required DateTime TimeOfStartKurs { get; set; }

        public required DateTime TimeOfEndKurs { get; set; }

        public required string Curs { get; set; }

        public required List<Student> Students { get; set; }

        [ForeignKey(nameof(CoachId))]
        public Coach Coach { get; set; }
       

    }
}
