using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ByteSchoolManager.Common.Abstractions;

namespace ByteSchoolManager.Entities
{
    [Table("StudentCourses")]
    public class StudentCourse : IDbEntity
    {
        public enum StudentCourseStatus
        {
            Engaged,
            NotEngaged,
            Paused,
            Online
        }

        [Key] public int Id { get; set; }
        public required int StudentId { get; set; }
        public required int CourseId { get; set; }
        public required StudentCourseStatus CourseStatus { get; set; }

        [ForeignKey(nameof(StudentId))] public Student Student { get; set; }

        [ForeignKey(nameof(CourseId))] public Course Course { get; set; }
    }
}