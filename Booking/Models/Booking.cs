using System;

namespace TruckBookingApp.Api.Models;
public class Booking
{
    public int Id { get; set; }
    public string? CustomerId { get; set; }
    public ApplicationUser? Customer { get; set; }
    public int? TruckId { get; set; }
    public Truck? Truck { get; set; }
    public string? PickupLocation { get; set; }
    public string? DropLocation { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Status { get; set; }
}
