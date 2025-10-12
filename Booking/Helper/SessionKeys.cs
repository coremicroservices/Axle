namespace Booking.Helper
{
    public static class SessionKeys
    {
        public static class User
        {
            public const string LoggedInUserDetail = nameof(LoggedInUserDetail);
            public const string SendNotificationToPartner = nameof(SendNotificationToPartner);
        }   
        public static class Supplier
        {
            public const string LoggedInSupplierName = nameof(LoggedInSupplierName);
        }
    }
}
