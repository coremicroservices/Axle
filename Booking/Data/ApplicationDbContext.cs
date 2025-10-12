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


        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<ShipmentDetail> ShipmentDetails { get; set; }
        public virtual DbSet<ShipmentFile> ShipmentFiles { get; set; }

        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<UserModel> Users { get; set; } 
    }
}
