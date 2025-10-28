using Booking.Data;
using Booking.Data.Tables;
using Booking.Helper;
using CG.Web.MegaApiClient;
 

namespace Booking.Services
{
    public interface IUploadFileService
    {
        public Task<ShipmentFile> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
    }

    public class UploadFileService : IUploadFileService
    {

        private const string MegaEmail = "infosharadv@gmail.com";
        private const string MegaPassword = "Pass1234$";
        private const string TargetFolderName = "AxleFiles";


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

            FileItem fileitem = null;
            string filePath = string.Empty;
            try
            {
                var client = new MegaApiClient();
                await client.LoginAsync(MegaEmail, MegaPassword);

                // Get all nodes (files and folders) from the logged-in Mega account
                var nodes = await client.GetNodesAsync();

                // Find the target folder by name
                var targetFolder = nodes.FirstOrDefault(n => n.Type == NodeType.Directory && n.Name == TargetFolderName);

                // If the folder doesn't exist, create it
                if (targetFolder == null)
                {
                    var rootNode = nodes.FirstOrDefault(n => n.Type == NodeType.Root);
                    targetFolder = await client.CreateFolderAsync(TargetFolderName, rootNode);
                }

                // Create a MemoryStream from the uploaded file
                using (var memoryStream = new MemoryStream())
                {
                    await file.OpenReadStream().CopyToAsync(memoryStream, cancellationToken);
                    memoryStream.Position = 0; // Reset the stream position for the upload

                    // Upload the file to the target folder
                    var response = await client.UploadAsync(memoryStream, file.FileName, targetFolder);

                    fileitem = new FileItem
                    {
                        Id = GuideHelper.GetGuide(),
                        CreationDate = DateTimeOffset.UtcNow,
                    
                        Fingerprint = response.Fingerprint,
                 
                        Name = response.Name,
                        Owner = response.Owner,
                        ParentId = response.ParentId,
                        Size = response.Size,
                        Type = (int)response.Type
                    };
                   await _dbContext.FileItems.AddAsync(fileitem, cancellationToken);
                }

                client.Logout();

            }
            catch (Exception ex)
            {

                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                filePath = Path.Combine(uploads, GuideHelper.GetGuide() + Path.GetExtension(file.FileName));
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                _logger.LogInformation("File uploaded successfully: {FilePath}", filePath);


            }
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
                fileItemId = fileitem?.Id,
                UploadedBy = "Admin" // You can set this dynamically based on the logged-in user    
            };
            await _dbContext.AddAsync(fileDetail, cancellationToken);
            return fileDetail; // Return the path where the file is stored
        }
    }
}
