using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{
    public class SupplierOnboardingDto
    {
        public string Id { get; set; }
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public int TruckCount { get; set; } = 0;

        public List<string> TruckTypes { get; set; } = [];

        public string BaseLocation { get; set; }

        public string ServiceRegions { get; set; }

        public IFormFile TruckImage { get; set; } = null;// For upload

    }
}
