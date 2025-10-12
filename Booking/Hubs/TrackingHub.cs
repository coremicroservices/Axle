using Booking.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Axle.Hubs
{
    [Authorize]
    public class TrackingHub : Hub
    {
        public async Task SendDriverLocation(string bookingId, double lat, double lng)
        {
            await Clients.All.SendAsync("DriverLocationUpdated", new { bookingId, lat, lng });
            await Clients.Group(bookingId).SendAsync("DriverLocationUpdated", new { bookingId, lat, lng });
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }

    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message,CancellationToken cancellationToken = default)
        {
            await Clients.All.SendAsync(SessionKeys.User.SendNotificationToPartner, user, message, cancellationToken);
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}


