
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Booking.FCMNotification
{
    public interface IFCMNotification
    {       
        public Task SendPushNotification(string deviceToken, string title, string body);
        public Task<string> GetAccessTokenAsync();

    }
    public class FCMNotification : IFCMNotification
    {
        string keyPath = Path.Combine(Directory.GetCurrentDirectory(), "axle-notification-firebase-adminsdk-fbsvc-248339f014.json");
        private Google.Apis.Http.HttpClientFactory HttpClientFactory;
        public FCMNotification()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(keyPath)
                });
            }

        }
 

        public async Task SendPushNotification(string deviceToken, string title, string body)
        {          
            var message = new Message()
            {
                Token = await GetAccessTokenAsync(),
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };
             

         //   string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            //Console.WriteLine("Successfully sent message: " + response);

        }

        public async Task<string> GetAccessTokenAsync()
        {
            var credential = GoogleCredential.FromFile(keyPath)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

            var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            return accessToken;
        }
    }
}
