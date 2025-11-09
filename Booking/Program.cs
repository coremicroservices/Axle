using Axle.Hubs;
using Booking.Attributes;
using Booking.Data;
using Booking.Handler;
using Booking.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace Booking
{
    public class Program
    {
       

        public static void Main(string[] args)
        {
           
            var builder = WebApplication.CreateBuilder(args);

           


            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(CustomErrorFilter)); 
            }).AddSessionStateTempDataProvider();
            builder.Services.AddSession();
            string connetionString = builder.Configuration.GetConnectionString("DefaultConnection");   
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connetionString)); 
            builder.Services.AddScoped<Services.IBookingService, Services.BookingService>();
            builder.Services.AddScoped<Services.IFileService, Services.FileService>();
            builder.Services.AddScoped<Services.IAuthService, Services.AuthService>();
            builder.Services.AddScoped<Services.ISupplierService, Services.SupplierService>();
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            builder.Services.AddScoped<FCMNotification.IFCMNotification, FCMNotification.FCMNotification>();
            builder.Services.AddMemoryCache(x =>
            {
                x.SizeLimit = 1024 * 1024 * 10; // 10 MB
            });
            
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
                options.LoginPath = "/Welcome/Index";
                options.AccessDeniedPath = "/Welcome/AccessDenied";
    });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/notificationHub");
                endpoints.MapHub<TrackingHub>("/trackinghub");
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Welcome}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
