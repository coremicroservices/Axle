using Booking.Data.Tables;

namespace Booking.DTO
{
    public class ShipmentDto
    {
        public string Id { get; set; }
        public string BookingId { get; set; }
        public string FileId { get; set; }

        // Source details
        public string SourcePincode { get; set; }
        public string SourceAddress { get; set; }

        // Destination details
        public string DestinationPincode { get; set; }
        public string DestinationAddress { get; set; }

        // Invoice details
        public string? InvoiceNumber { get; set; }
        public decimal? InvoiceValue { get; set; }

        // Audit fields
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime BookingDate { get; set; }
        public ShipmentFile  ShipmentFile { get; set; }

    }
}
