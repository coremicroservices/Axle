using Booking.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("Shipments")]
    public class Shipment
    {
        [Key]
        [StringLength(32)]
        public string Id { get; set; } = GuideHelper.GetGuide();

        public string BookingId { get; set; } = GenerateTruckBookingCode();

        [Required]
        [StringLength(32)]
        public string ShipmentFileId { get; set; }
        // Source details
        [Required]
        [StringLength(6)]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be 6 digits.")]
        public string SourcePincode { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string SourceAddress { get; set; } = string.Empty;

        // Destination details
        [Required]
        [StringLength(6)]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be 6 digits.")]
        public string DestinationPincode { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string DestinationAddress { get; set; } = string.Empty;

        // Invoice details
        [StringLength(100)]
        public string? InvoiceNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 50000000, ErrorMessage = "Invoice value must be between 0 and 50,000,000")]
        public decimal? InvoiceValue { get; set; }

        // Audit fields
        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime BookingDate { get; set; }

        public virtual ICollection<BuyerSupplierMapping> BuyerSupplierMappings { get; set; } = [];

        public static string GenerateTruckBookingCode()
        {
            string prefix = "AXLE";        
            string randomPart = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(); // 3-char random
            return $"{prefix}-{DateTime.UtcNow.Ticks}-{randomPart}";
        }

        public virtual ShipmentFile ShipmentFile { get; set; }
    }

    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasMaxLength(32);
            builder.Property(s => s.ShipmentFileId).IsRequired().HasMaxLength(32);
            builder.Property(s => s.SourcePincode).IsRequired().HasMaxLength(6);
            builder.Property(s => s.SourceAddress).IsRequired().HasMaxLength(1000);
            builder.Property(s => s.DestinationPincode).IsRequired().HasMaxLength(6);
            builder.Property(s => s.DestinationAddress).IsRequired().HasMaxLength(1000);
            builder.Property(s => s.InvoiceNumber).HasMaxLength(100);
            builder.Property(s => s.InvoiceValue).HasColumnType("decimal(18,2)");
            builder.Property(s => s.CreatedBy).HasMaxLength(100);
            builder.Property(s => s.CreatedOn).IsRequired();
            builder.Property(s => s.IsActive).IsRequired();
            builder.Property(s => s.BookingDate).IsRequired();
            builder.HasMany(s => s.BuyerSupplierMappings)
                   .WithOne()
                   .HasForeignKey(bsm => bsm.ShipmentId);      
            
            builder.HasOne(s => s.ShipmentFile)
                   .WithOne(sf => sf.Shipment)
                   .HasForeignKey<Shipment>(s => s.ShipmentFileId)
                   .OnDelete(DeleteBehavior.Cascade);   
        }
    }   

}
