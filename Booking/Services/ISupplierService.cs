using Booking.Data;
using Booking.Data.Tables;
using Booking.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public interface ISupplierService
    {
        public Task<SupplierOnboardingDto> IsValidSupplierAsync(string contactno, string password, CancellationToken cancellationToken = default);
        public Task<int> IncomingBookingCount(string supplierId,CancellationToken cancellationToken = default);

        public Task<string> AddDeviceTokenAsync(FcmDeviceToken fcmDeviceToken, CancellationToken cancellationToken = default);
    }
    public class SupplierService : ISupplierService
    {
     private readonly ApplicationDbContext _applicationDbContext;
        public SupplierService(ApplicationDbContext applicationDbContex)
        { 
                _applicationDbContext = applicationDbContex;
        }

        public async Task<int> IncomingBookingCount(string supplierId, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.BuyerSupplierMappings.CountAsync(x => x.SupplierId.Equals(supplierId), cancellationToken);
        }

        public async Task<SupplierOnboardingDto> IsValidSupplierAsync(string contactno, string password, CancellationToken cancellationToken = default)
        {
            var result =  await _applicationDbContext.Suppliers
               .FirstOrDefaultAsync(s => s.ContactNumber == contactno, cancellationToken);
            
            return result is not null ? new SupplierOnboardingDto
            {
                Id = result.Id,
                OwnerName = result.OwnerName,
                CompanyName = result.CompanyName               
            } : null;
        }

        public void DisposeAsync()
        {
            _applicationDbContext?.DisposeAsync();
        }   
        public void Dispose()
        {
            _applicationDbContext?.Dispose();
        }

        public async Task<string> AddDeviceTokenAsync(FcmDeviceToken fcmDeviceToken, CancellationToken cancellationToken = default)
        {
            var fcm = await _applicationDbContext.FcmDeviceTokens.FirstOrDefaultAsync(x => x.UserId.Equals(fcmDeviceToken.UserId), cancellationToken);
            if (fcm is null)
            {
                await _applicationDbContext.FcmDeviceTokens.AddAsync(fcmDeviceToken, cancellationToken);
            }
            else if (fcm is not null)
            {
                fcm.DeviceToken = fcmDeviceToken.DeviceToken;
                fcm.UpdatedAt = DateTime.UtcNow;
            }
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return fcmDeviceToken.Id;
        }
    }
}
