using Booking.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Data.Tables
{
    [Table("ShipmentFiles")]
    public class ShipmentFile
    {
        [Key]
        [StringLength(32)]
        public string FileId { get; set; } = GuideHelper.GetGuide();

         
        [Required]
        [StringLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string StoredFileName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? FileExtension { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? FileSizeKB { get; set; }

        [StringLength(100)]
        public string? ContentType { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(100)]
        public string? UploadedBy { get; set; }

        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public string fileItemId { get; set; }

        public virtual FileItem FileItem { get; set; }
    }
    public class ShipmentFileConfiguration : IEntityTypeConfiguration<ShipmentFile>
    {
        public void Configure(EntityTypeBuilder<ShipmentFile> builder)
        {
            builder.HasOne(s => s.FileItem)
                   .WithOne(f => f.ShipmentFile)
                   .HasForeignKey<ShipmentFile>(s => s.FileId);
        }
    }   
}
