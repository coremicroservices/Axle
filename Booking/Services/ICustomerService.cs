using Booking.Data;
using Booking.Data.Tables;
using Booking.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 

namespace Booking.Services
{
    public interface ICustomerService
    {
        Task<List<ShipmentDto>> GetShipmentsAsync(string createdBy, CancellationToken cancellationToken = default);
        Task<FileStreamResult> DownloadFileAsync(string fileItemId, CancellationToken cancellationToken = default);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _fileService;

        public CustomerService(ApplicationDbContext dbContext, IFileService fileService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public  async Task<FileStreamResult> DownloadFileAsync(string fileItemId, CancellationToken cancellationToken = default)
        {
           var file =  await _dbContext.FileItems.FirstOrDefaultAsync(x => x.Id.Equals(fileItemId), cancellationToken);
            if(file is not null)
            {
                return await _fileService.DownloadFileAsync(file.NodeId, cancellationToken);
            }
            return null;
        }

        public async Task<List<ShipmentDto>> GetShipmentsAsync(string createdBy, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Shipments.Where(s => s.CreatedBy!.Equals(createdBy) && s.IsActive).Include(v => v.ShipmentFile).Select(s => new ShipmentDto()
            {
                Id = s.Id,
                BookingId = s.BookingId,
                ShipmentFileId = s.ShipmentFileId,
                SourcePincode = s.SourcePincode,
                SourceAddress = s.SourceAddress,
                DestinationPincode = s.DestinationPincode,
                DestinationAddress = s.DestinationAddress,
                InvoiceNumber = s.InvoiceNumber,
                InvoiceValue = s.InvoiceValue,
                CreatedBy = s.CreatedBy,
                CreatedOn = s.CreatedOn,
                UpdatedOn = s.UpdatedOn,
                IsActive = s.IsActive,
                BookingDate = s.BookingDate,
                ShipmentFile = s.ShipmentFile
            }).ToListAsync(cancellationToken);
        }
    }
}
