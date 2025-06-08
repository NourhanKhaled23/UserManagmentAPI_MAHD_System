using Domain.Common;
using System;

namespace Domain.Entities
{
    public class User : BaseEntity  // BaseEntity could include CreatedAt, UpdatedAt, etc.
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Student";  // Default role
         // Navigation Property for One-to-One Relationship with UserSettings
        public UserSettings? UserSettings { get; set; }  

    }
}




