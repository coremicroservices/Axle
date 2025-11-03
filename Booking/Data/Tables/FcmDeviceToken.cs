using Booking.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("FcmDeviceTokens")]
    public class FcmDeviceToken
    {
        [Key]
        public string Id { get; set; } = GuideHelper.GetGuide();

        [Required]
        [ForeignKey(nameof(Suppliers))]

        public string UserId { get; set; }

        [Required]
        [MaxLength(512)]
        public string DeviceToken { get; set; }

        [MaxLength(50)]
        public string Platform { get; set; } // Android, iOS, Web

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public virtual Suppliers Suppliers { get; set; }
    }

    public class FcmDeviceTokenConfiguration : IEntityTypeConfiguration<FcmDeviceToken>
    {
        public void Configure(EntityTypeBuilder<FcmDeviceToken> builder)
        {
            builder.HasOne(f => f.Suppliers)
    .WithOne(s => s.FcmDeviceToken)
    .HasForeignKey<FcmDeviceToken>(f => f.UserId);

        }
    }
}
