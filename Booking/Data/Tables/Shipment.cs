using Booking.Helper;
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

        [Required]
        [StringLength(32)]
        public string fileId { get; set; } 
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
    }
}
