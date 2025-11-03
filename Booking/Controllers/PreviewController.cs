using Booking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreviewController : ControllerBase
    {
        private readonly IFileService _fileService;
        public PreviewController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet]
        public async Task<string> GetStringAsync()
        {
            return DateTime.UtcNow.ToString();
        }

        [HttpGet("{nodeId}")]
        public async Task<FileStreamResult> DownloadImage([FromRoute][Required] string nodeId,CancellationToken cancellationToken = default)
        {
            return await _fileService.DownloadFileAsync(nodeId, cancellationToken);
        }
    }
}
