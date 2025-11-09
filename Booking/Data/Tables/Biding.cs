using Booking.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("Bid")]
    public class Biding
    {
        [Key]
        [StringLength(32)]
        public string Id { get; set; } = GuideHelper.GetGuide();

        [Required]
        [StringLength(32)]
        public string CustomerId { get; set; }

        [Required]
        [StringLength(32)]
        public string SupplierId { get; set; }

        [Required]
        [StringLength(32)]
        public string ShipmentID { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BidAmount { get; set; }

        [Required]
        public DateTime BidTime { get; set; } = DateTime.Now;

        [Required]
        public bool IsWinningBid { get; set; } = false;

        [StringLength(500)]
        public string? Remarks { get; set; }

        [Required]
        public int Status { get; set; } = 0;

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

    }
}
