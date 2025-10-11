using Microsoft.AspNetCore.Identity;

namespace TruckBookingApp.Api.Models;
public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public bool IsDriver { get; set; }
}
