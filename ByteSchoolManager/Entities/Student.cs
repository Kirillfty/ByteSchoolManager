using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ByteSchoolManager.Common.Abstractions;

namespace ByteSchoolManager.Entities
{
    [Table("Students")]
    public class Student : IDbEntity
    {
        [Key]
        public int Id { get; set; }

        public required string Name{get;set;} 

        public required string ParentName { get; set; }

        public required string StudentPhoneNumber { get; set; }

        public required string ParentPhoneNumber { get; set; }

        public List<Course> Courses { get; set; }

        public List<Lesson> Lessons { get; set; }

        //Список отработанных занятий (связь Таблица Студент - Отработанное занятие)
    }
}
