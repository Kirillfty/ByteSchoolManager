using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public required string NameOfStudent{get;set;} 

        public required string SurNameOfStudent { get; set; }

        public required string NameOfParent { get; set; }

        public required string SurNameOfParent { get; set; }

        public required int NumberOfStudent { get; set; }

        public required int NumberOfParent { get; set; }

        public List<Course> Courses { get; set; }

        public List<Lesson> Lessons { get; set; }

        //Список отработанных занятий (связь Таблица Студент - Отработанное занятие)
    }
}
