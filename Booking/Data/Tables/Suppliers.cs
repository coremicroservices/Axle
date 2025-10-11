using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("Suppliers")]
    public class Suppliers
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [Required]
        [StringLength(15)]
        public string ContactNo { get; set; }
    }
}
