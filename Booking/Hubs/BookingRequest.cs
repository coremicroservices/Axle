namespace Booking.Hubs
{
    public class BookingRequest
    {
        public string BookingId { get; set; }
        public string CustomerName { get; set; }
        public string PickupLocation { get; set; }
        public string DropLocation { get; set; }
        public string TruckType { get; set; }
        public DateTime? ScheduledTime { get; set; }
        public decimal? Charges { get; set; }
    }
}
