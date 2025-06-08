/*namespace Domain.Entities
{
    public class UserSettings
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool ReceiveNotifications { get; set; } = true;
        public string PrivacyLevel { get; set; } = "Public";
    }
}

*/


using Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class UserSettings : BaseEntity  // Inherit BaseEntity to track CreatedAt & UpdatedAt
    {
        public int Id { get; set; }

        //Foreign Key Relationship
        public int UserId { get; set; }

        [ForeignKey("UserId")] 
        public User? User { get; set; }

        public bool ReceiveNotifications { get; set; } = true;
        public string PrivacyLevel { get; set; } = "Public";
    }
}
