using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ByteSchoolManager.Entities
{
    [Table("Coaches")]
    public class Coach
    {
        [Key]
        public int Id { get; set; }
        public required string NameOfCoach { get; set; }
        public required int NumberOfCoach { get; set; }

        public List<Student> Students { get; set; }
    }
}
