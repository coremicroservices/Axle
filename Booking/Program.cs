using Axle.Hubs;
using Booking.Data;
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
            builder.Services.AddControllersWithViews();
            string connetionString = builder.Configuration.GetConnectionString("DefaultConnection");   
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connetionString)); 
            builder.Services.AddScoped<Services.IBookingService, Services.BookingService>();
            builder.Services.AddScoped<Services.IUploadFileService, Services.UploadFileService>();
            builder.Services.AddScoped<Services.IAuthService, Services.AuthService>();
            builder.Services.AddScoped<Services.ISupplierService, Services.SupplierService>();
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Supplier/Login";
        options.AccessDeniedPath = "/Supplier/AccessDenied";
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
