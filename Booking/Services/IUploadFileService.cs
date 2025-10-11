using Booking.Data;
using Booking.Data.Tables;
using Booking.Helper;

namespace Booking.Services
{
    public interface IUploadFileService
    {
        public Task<ShipmentFile> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
    }

    public class UploadFileService : IUploadFileService
    {
        private readonly ILogger<UploadFileService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _dbContext;
        public UploadFileService(ILogger<UploadFileService> logger, IWebHostEnvironment environment, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _environment = environment;
            _dbContext = applicationDbContext;
        }
        public async Task<ShipmentFile> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file provided for upload.");
                return null;
            }
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var filePath = Path.Combine(uploads, Path.GetRandomFileName() + Path.GetExtension(file.FileName));
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            _logger.LogInformation("File uploaded successfully: {FilePath}", filePath);

            var fileDetail = new ShipmentFile
            {
                FileId = GuideHelper.GetGuide(),                    // Unique ID for the file
                OriginalFileName = file.FileName,                       // Original file name
                StoredFileName = $"{GuideHelper.GetGuide()}{Path.GetExtension(file.FileName)}",  // Stored name with GUID
                FileExtension = Path.GetExtension(file.FileName),     // File extension
                FileSizeKB = file.Length / 1024,                       // Size in KB
                ContentType = file.ContentType,
                FilePath = filePath,
                IsActive = true,
                UploadedOn = DateTime.UtcNow,
                UploadedBy = "Admin" // You can set this dynamically based on the logged-in user    
            };
            await _dbContext.AddAsync(fileDetail, cancellationToken);
            return fileDetail; // Return the path where the file is stored
        }
    }
}
