using Booking.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("shipmentdetail")]
    public class ShipmentDetail
    {
        [Key]
        [StringLength(32)]
        public string Id { get; set; }  = GuideHelper.GetGuide();

        [StringLength(32)]
        public string ShipmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductCode { get; set; }

        [Required]
        public int NumberOfBoxes { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Length { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Breadth { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Height { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalWeight { get; set; }
    }
}
