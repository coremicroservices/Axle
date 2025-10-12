using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{
    public class SupplierModel
    {
        [Key]
        [Required]
        [StringLength(32)]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Contact Number")]
        public string ContactNo { get; set; }
         
    }
}
