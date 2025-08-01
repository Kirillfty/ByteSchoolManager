using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("StudentCourses")]
    public class StudentCourse
    {
        public enum StudentStatus
        {
            Engaged,
            NotEngaged,
            Paused,
            Online
        }
        
        [Key]
        public int Id { get; set; }
        public required int  StudentId { get; set; }
        public required int CourseId { get; set; }
        public required StudentStatus Status { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }
        
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
    }
}
