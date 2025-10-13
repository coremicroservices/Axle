using Booking.Helper;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("buyersuppliermapping")]
    public class BuyerSupplierMapping
    {
        public string Id { get; set; } = GuideHelper.GetGuide();
        public string ShipmentId { get; set; }
        public string SupplierId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
}
