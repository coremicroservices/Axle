using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TruckBookingApp.Api.Hubs;
[Authorize]
public class TrackingHub : Hub
{
    public async Task SendDriverLocation(string bookingId, double lat, double lng)
    {
        await Clients.Group(bookingId).SendAsync("DriverLocationUpdated", new { bookingId, lat, lng });
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
}
