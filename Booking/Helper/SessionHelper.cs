using System.Text;
using System.Text.Json;

namespace Booking.Helper
{
    public static class SessionHelper
    {
        public static T GetObjectFromSession<T>(ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null || data.Length == 0)
                return default;

            var json = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<T>(json);
        }

        public static T GetObjectDetailFromSession<T>( this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null || data.Length == 0)
                return default;

            var json = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
