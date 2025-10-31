using Booking.Data;
using Booking.Data.Tables;
using Booking.Helper;
using Booking.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public interface IBookingService
    {
        public Task<string> CreateShipmentAsync(ShipmentViewModel shipmentViewModel, List<string> supplierIds, CancellationToken cancellationToken = default);

        public Task<List<Suppliers>> GetSuppliersAsync(CancellationToken cancellationToken = default);
    }

    public class BookingService : IBookingService
    {
        private readonly ILogger<BookingService> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _uploadFileService;

        public BookingService(ILogger<BookingService> logger, ApplicationDbContext dbContext, IFileService uploadFileService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _uploadFileService = uploadFileService;
        }
        public async Task<string> CreateShipmentAsync(ShipmentViewModel shipmentViewModel,List<string> supplierIds, CancellationToken cancellationToken = default)
        {
            var uploadedFileDetail = await _uploadFileService.UploadFileAsync(shipmentViewModel.InvoiceFile);
            Shipment shipment = new Shipment
            {
                Id = GuideHelper.GetGuide(),
                CreatedBy = "Admin", // This should be dynamic based on logged-in user  
                CreatedOn = DateTime.UtcNow,
                DestinationAddress = shipmentViewModel.DestinationAddress,
                DestinationPincode = shipmentViewModel.DestinationPincode,
                fileId = uploadedFileDetail.FileId,
                InvoiceNumber = shipmentViewModel.InvoiceNumber,
                InvoiceValue = shipmentViewModel.InvoiceValue,
                IsActive = true,
                SourceAddress =
                shipmentViewModel.SourceAddress,
                SourcePincode = shipmentViewModel.SourcePincode,
                UpdatedOn = DateTime.UtcNow,

            };
            ShipmentDetail shipmentDetail = new ShipmentDetail
            {
                ShipmentId = shipment.Id,
                Breadth = shipmentViewModel.Breadth,
                Height = shipmentViewModel.Height,
                Id = GuideHelper.GetGuide(),
                Length = shipmentViewModel.Length,
                NumberOfBoxes = shipmentViewModel.NumberOfBoxes,
                ProductCode = shipmentViewModel.ProductCode,
                TotalWeight = shipmentViewModel.TotalWeight,

            
            };

            foreach (var supplierId in supplierIds)
            {
                var buyerSupplier = new BuyerSupplierMapping()
                {
                    ShipmentId = shipment.Id,
                    SupplierId = supplierId,
                    CreatedOn = DateTime.UtcNow,
                };

                await _dbContext.AddAsync(buyerSupplier, cancellationToken);

            }

           
            await _dbContext.AddAsync(shipmentDetail);
            await _dbContext.AddAsync(shipment);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Creating shipment {ShipmentId}", shipment.Id);
            return shipment.Id;
            // Implementation to save shipment, details, and files to the database      
        }

        public async Task<List<Suppliers>> GetSuppliersAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Suppliers.Include(x =>  x.FcmDeviceToken).ToListAsync(cancellationToken);
        }
    }
}
