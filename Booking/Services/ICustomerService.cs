using Booking.Data;
using Booking.Data.Tables;
using Booking.DTO;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public interface ICustomerService
    {
        Task<List<ShipmentDto>> GetShipmentsAsync(string createdBy, CancellationToken cancellationToken = default);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<List<ShipmentDto>> GetShipmentsAsync(string createdBy, CancellationToken cancellationToken = default)
        {
           return await _dbContext.Shipments.Where(s => s.CreatedBy!.Equals(createdBy) && s.IsActive).Select(s => new ShipmentDto()
            {
                Id = s.Id,
                BookingId = s.BookingId,
                FileId = s.fileId,
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
                //ShipmentFile = s.ShipmentFile
            }).ToListAsync(cancellationToken);
        }
    }
}
