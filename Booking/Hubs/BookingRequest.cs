namespace Booking.Hubs
{
    public class BookingRequest
    {
        public string CustomerName { get; set; }
        public string Pickup { get; set; }
        public string Drop { get; set; }
        public string TruckType { get; set; }
        public string ScheduledTime { get; set; }

    }
}
