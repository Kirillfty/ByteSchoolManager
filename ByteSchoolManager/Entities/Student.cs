using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public required string Name{get;set;} 

        public required string ParentName { get; set; }

        public required int StudentPhoneNumber { get; set; }

        public required int ParentPhoneNumber { get; set; }

        public List<Course> Courses { get; set; }

        public List<Lesson> Lessons { get; set; }

        //Список отработанных занятий (связь Таблица Студент - Отработанное занятие)
    }
}
