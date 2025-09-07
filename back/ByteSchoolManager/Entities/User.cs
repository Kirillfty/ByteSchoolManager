using Microsoft.AspNetCore.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ByteSchoolManager.Common.Abstractions;

namespace ByteSchoolManager.Entities
{
    [Table("Users")]
    public class User : IDbEntity
    {
        [Key] public int Id { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public string Role { get; set; } = UserRole.User;
        public string? RefreshToken { get; set; }
    }
}