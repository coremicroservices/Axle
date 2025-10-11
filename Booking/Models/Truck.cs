namespace TruckBookingApp.Api.Models;
public class Truck
{
    public int Id { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? Type { get; set; }
    public bool IsAvailable { get; set; }
}
