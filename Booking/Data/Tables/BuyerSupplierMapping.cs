using Booking.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("buyersuppliermapping")]
    public class BuyerSupplierMapping
    {
        public string Id { get; set; } = GuideHelper.GetGuide();
        public string ShipmentId { get; set; }
        public string SupplierId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public virtual ICollection<Shipment> Shipment { get; set; }  

    }
   public class BuyerSupplierMappingConfiguration : IEntityTypeConfiguration<BuyerSupplierMapping>
    {
        public void Configure(EntityTypeBuilder<BuyerSupplierMapping> builder)
        {
            builder.HasKey(bsm => bsm.Id);
            builder.Property(bsm => bsm.Id).HasMaxLength(32);
            builder.Property(bsm => bsm.ShipmentId).IsRequired().HasMaxLength(32);
            builder.Property(bsm => bsm.SupplierId).IsRequired().HasMaxLength(32);
            builder.Property(bsm => bsm.CreatedOn).IsRequired();
            builder.HasMany(bsm => bsm.Shipment)
                   .WithOne()
                   .HasForeignKey(s => s.Id);                   
        }
    }   
}
