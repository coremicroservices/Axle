using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Booking.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}