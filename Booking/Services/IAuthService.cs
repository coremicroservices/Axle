using Booking.Data;
using Booking.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace Booking.Services
{
    public interface IAuthService
    {
        public Task<string> RegisterUserAsync(UserModel userModel,CancellationToken cancellationToken = default);
        public Task<UserModel> LoginUserAsync(string userName,string password, CancellationToken cancellationToken = default);
    }
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AuthService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;   
        }
        public async Task<UserModel> LoginUserAsync(string userName, string password, CancellationToken cancellationToken = default)
        {
            var userModel =  await _applicationDbContext.Users.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(u=>u.Email == userName && u.PasswordHash == password, cancellationToken);
            return userModel is not null ? new UserModel()
            {
                Name = userModel!.Name,
                CreatedAt = userModel.CreatedAt,
                Email = userModel.Email

            } : null;
        }

        public async Task<string> RegisterUserAsync(UserModel userModel, CancellationToken cancellationToken = default)
        {
            await _applicationDbContext.AddAsync(userModel, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return userModel.Id;
        }
    }
}
