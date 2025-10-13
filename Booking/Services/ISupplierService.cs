using Booking.Data;
using Booking.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public interface ISupplierService
    {
        public Task<SupplierModel> IsValidSupplierAsync(string contactno, string password, CancellationToken cancellationToken = default);
        public Task<int> IncomingBookingCount(string supplierId,CancellationToken cancellationToken = default);
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

        public async Task<SupplierModel> IsValidSupplierAsync(string contactno, string password, CancellationToken cancellationToken = default)
        {
            var result =  await _applicationDbContext.Suppliers
               .FirstOrDefaultAsync(s => s.ContactNo == contactno && s.Password == password, cancellationToken);
            
            return result is not null ? new SupplierModel
            {
                Id = result?.Id,
                SupplierName = result?.SupplierName,
                ContactNo = result?.ContactNo
            } : null;
        }
    }
}
