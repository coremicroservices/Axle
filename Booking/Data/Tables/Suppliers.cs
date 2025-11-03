using Booking.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("Suppliers")]
    public class Suppliers
    {
        [Key]
        [Required]
        [Column("SupplierId")]
        public string Id { get; set; } = GuideHelper.GetGuide();

        [Required, MaxLength(100)]
        public string CompanyName { get; set; }

        [Required, MaxLength(100)]
        public string OwnerName { get; set; }

        [Required, MaxLength(20)]
        public string ContactNumber { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        public int TruckCount { get; set; }

        [MaxLength(200)]
        public string? TruckTypes { get; set; } // Comma-separated

        [MaxLength(100)]
        public string? BaseLocation { get; set; }

        [MaxLength(200)]
        public string? ServiceRegions { get; set; }

        //[MaxLength(200)]
        public string? TruckImagePath { get; set; } = null;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual FcmDeviceToken FcmDeviceToken { get; set; }

    }
}
