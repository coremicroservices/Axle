namespace Booking.Helper
{
    public static class GuideHelper
    {
        public static string GetGuide()
        {
            return Guid.NewGuid().ToString("N").ToLower(); // 32-char GUID (no dashes)    
        }
    }
}
