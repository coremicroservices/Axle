using System.ComponentModel.DataAnnotations;

namespace Booking.Models
{
    public class SupplierLoginViewModel
    {
        [Required]       
        public string Contact { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
