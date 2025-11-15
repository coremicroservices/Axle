using Booking.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // EnableDetailedErrors call should be enabled only for debugging.  
            // It may cause small performance overhead during execution.
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-0ROG1KC\\DEVELOPER;Initial Catalog=axle;Persist Security Info=True;User ID=sa;Password=Pass1234$;Trust Server Certificate=True")
                .LogTo(Console.WriteLine)
                .EnableDetailedErrors();
        }


        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<ShipmentDetail> ShipmentDetails { get; set; }
        public virtual DbSet<ShipmentFile> ShipmentFiles { get; set; }

        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<UserModel> Users { get; set; } 
        public virtual DbSet<BuyerSupplierMapping> BuyerSupplierMappings { get; set; }
        public virtual DbSet<FcmDeviceToken> FcmDeviceTokens { get; set; }
        public virtual DbSet<FileItem> FileItems { get; set; }
        public virtual DbSet<Biding> Bidings { get; set; }  
    }
}
