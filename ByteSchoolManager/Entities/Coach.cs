using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ByteSchoolManager.Common.Abstractions;

namespace ByteSchoolManager.Entities
{
    [Table("Coaches")]
    public class Coach : IDbEntity
    {
        [Key] public int Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Telegram { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))] public User User { get; set; }
    }
}