using Booking.FCMNotification;
using Booking.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        public string Id { get; set; } = GuideHelper.GetGuide();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MaxLength(512)]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

       public virtual FcmDeviceToken? FcmDeviceToken { get; set; } 

        public string FcmDeviceTokenId { get; set; }
    }

    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DeviceToken { get; set; }
    }
}
