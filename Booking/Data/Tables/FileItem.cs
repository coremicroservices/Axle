using Booking.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("Files")]
    public class FileItem
    {
        [Key]
        public string Id { get; set; }

        public int Type { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public long Size { get; set; }        

        public string? Fingerprint { get; set; }

        [Required]
        public string ParentId { get; set; }

        public DateTime CreationDate { get; set; }

        [Required]
        public string Owner { get; set; }

        public string FolderPath { get; set; }
        public string NodeId { get; set; }

        public virtual ShipmentFile ShipmentFile { get; set; }  

    }
    public class FileItemConfiguration : IEntityTypeConfiguration<FileItem>
    {
        public void Configure(EntityTypeBuilder<FileItem> builder)
        {
            builder.HasOne(f => f.ShipmentFile)
                   .WithOne(s => s.FileItem)
                   .HasForeignKey<ShipmentFile>(s => s.Id);
        }
    }   
}
