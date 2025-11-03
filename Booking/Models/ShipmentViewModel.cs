using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{

    public class ShipmentViewModel
    {
        [Display(Name = "Invoice (optional)")]
        public IFormFile InvoiceFile { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be 6 digits.")]
        [Display(Name = "Source Pincode")]
        public string SourcePincode { get; set; }

        [Required]
        [Display(Name = "Source Address")]
        [StringLength(1000)]
        public string SourceAddress { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be 6 digits.")]
        [Display(Name = "Destination Pincode")]
        public string DestinationPincode { get; set; }

        [Required]
        [Display(Name = "Destination Address")]
        [StringLength(1000)]
        public string DestinationAddress { get; set; }

        // Invoice details
        [Display(Name = "Invoice Number")]
        [StringLength(100)]
        public string InvoiceNumber { get; set; }

        [Display(Name = "Invoice Value (INR)")]
        [Range(0, 50000000, ErrorMessage = "Invoice value must be between 0 and 50,000,000")]
        public decimal? InvoiceValue { get; set; }

        // Shipment details
        [Required]
        [Display(Name = "Product")]
        public string ProductCode { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Number of boxes must be at least 1")]
        [Display(Name = "No. of boxes")]
        public int NumberOfBoxes { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "Length must be positive")]
        public decimal? Length { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "Breadth must be positive")]
        public decimal? Breadth { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "Height must be positive")]
        public decimal? Height { get; set; }

        [Required]
        [Range(0.1, 100000.0, ErrorMessage = "Total weight must be > 0")]
        [Display(Name = "Total weight (Kg)")]
        public decimal TotalWeight { get; set; }

        [Required]
        [FutureDate]
        public DateTime BookingDate { get; set; }

            // Add any other fields as needed...
        }


    public class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute()
        {
            ErrorMessage = "Booking date must be in the future.";
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date >= DateTime.UtcNow;
            }
            return false;
        }
    }

}
