using Booking.Data;
using Booking.Data.Tables;
using Booking.Helper;
using CG.Web.MegaApiClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;


namespace Booking.Services
{
    public abstract class MegaBase
    {
        private const string MegaEmail = "infosharadv@gmail.com";
        private const string MegaPassword = "Pass1234$";
        protected string TargetFolderName = "AxleFiles";
        protected readonly MegaApiClient _megaApiClient;
        protected readonly IMemoryCache _memoryCache;
        
        protected MegaBase(IMemoryCache memoryCache)
        {  
            _megaApiClient = new MegaApiClient();
            _memoryCache = memoryCache;
        }
        protected async Task MegaLoginAsync()
        {
            await _megaApiClient.LoginAsync(MegaEmail, MegaPassword);
        }
        protected void MegaLogout()
        {
            _megaApiClient.Logout();
        }
    }   
    public interface IFileService
    {
        public Task<ShipmentFile> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
        public Task<FileStreamResult> DownloadFileAsync(string fileId, CancellationToken cancellationToken = default);
    }

    public record DownloadedFile(Stream stream,string contentType,string fileName);

    public class FileService : MegaBase, IFileService
    {        
        

        private readonly ILogger<FileService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _dbContext;
        public FileService(ILogger<FileService> logger, IWebHostEnvironment environment, ApplicationDbContext applicationDbContext, IMemoryCache memoryCache):base(memoryCache)
        {
            _logger = logger;
            _environment = environment;
            _dbContext = applicationDbContext;
        }
        public FileStreamResult GetImage(string key)
        {
            return _memoryCache.TryGetValue(key,out FileStreamResult imageData) ? imageData : null;
        }


        public async Task<FileStreamResult> DownloadFileAsync(string nodeId, CancellationToken cancellationToken = default)
        {

            var cacheImage = GetImage(nodeId);
            if (cacheImage is not null)
            {
                return cacheImage;
            }
            await MegaLoginAsync();

            // Get all nodes (files/folders)
            var nodes = _megaApiClient.GetNodes();

            // Find the node by ID
            var node = nodes.FirstOrDefault(n => n.Id == nodeId);
            if (node is null)
            {
                MegaLogout();
                var notfoudnImagePath = Path.Combine(_environment.WebRootPath, "Template/image_not_found.avif");
                if(Path.Exists(notfoudnImagePath))
                {
                    var notfoundStream = new FileStream(notfoudnImagePath, FileMode.Open, FileAccess.Read);
                    return new FileStreamResult(notfoundStream, "image/avif")
                    {
                        FileDownloadName = "image_not_found.avif"
                    };
                }   
            }

         
            // Download the file as a stream
            var stream = _megaApiClient.Download(node,cancellationToken);
            /// string fileName = node.Name;

             MegaLogout();

            var entry = _memoryCache.CreateEntry(nodeId);
            var filestream = new FileStreamResult(stream, $"{ContentTypeHelper.MimeTypes[Path.GetExtension(node.Name)]}")
            {
                FileDownloadName = node.Name
            };
            entry.SetValue(filestream);
            return filestream;


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
                await MegaLoginAsync();
                // Get all nodes (files and folders) from the logged-in Mega account
                var nodes = await _megaApiClient.GetNodesAsync();
                TargetFolderName = Path.Combine(TargetFolderName, "customer");

                // Find the target folder by name
                var targetFolder = nodes.FirstOrDefault(n => n.Type == NodeType.Directory && n.Name == TargetFolderName);

                // If the folder doesn't exist, create it
                if (targetFolder == null)
                {
                    var rootNode = nodes.FirstOrDefault(n => n.Type == NodeType.Root);
                    targetFolder = await _megaApiClient.CreateFolderAsync(TargetFolderName, rootNode);
                }

                // Create a MemoryStream from the uploaded file
                using (var memoryStream = new MemoryStream())
                {
                    await file.OpenReadStream().CopyToAsync(memoryStream, cancellationToken);
                    memoryStream.Position = 0; // Reset the stream position for the upload

                    // Upload the file to the target folder
                    var response = await _megaApiClient.UploadAsync(memoryStream, file.FileName, targetFolder);

                    fileitem = new FileItem
                    {
                        Id = GuideHelper.GetGuide(),
                        CreationDate = DateTimeOffset.UtcNow,
                    
                        Fingerprint = response.Fingerprint,
                 
                        Name = response.Name,
                        Owner = response.Owner,
                        ParentId = response.ParentId,
                        Size = response.Size,
                        Type = (int)response.Type,
                        FolderPath = TargetFolderName,
                        NodeId = response.Id
                    };
                   await _dbContext.FileItems.AddAsync(fileitem, cancellationToken);
                }

                MegaLogout();

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
