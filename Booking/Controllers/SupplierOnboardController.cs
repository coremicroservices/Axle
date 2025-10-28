using Booking.Data;
using Booking.Data.Tables;
using Booking.Helper;
using Booking.Models;
using Booking.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    public class SupplierOnboardController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUploadFileService _uploadFileService;

        public SupplierOnboardController(ApplicationDbContext applicationDbContext, IUploadFileService uploadFileService)
        {
            _applicationDbContext = applicationDbContext;
            _uploadFileService = uploadFileService;
        }
        public IActionResult Index()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOnboarding(SupplierOnboardingDto dto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return View(dto);

            string imagePath = null;
            if (dto.TruckImage != null && dto.TruckImage.Length > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(dto.TruckImage.FileName);
                var extension = Path.GetExtension(dto.TruckImage.FileName);
                var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                var savePath = Path.Combine("wwwroot/images/trucks", newFileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await dto.TruckImage.CopyToAsync(stream);
                }

                imagePath = $"/images/trucks/{newFileName}";
            }

            var supplier = new Suppliers
            {
                CompanyName = dto.CompanyName,
                OwnerName = dto.OwnerName,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                TruckCount = dto.TruckCount,
                TruckTypes = string.Join(",", dto.TruckTypes ?? new List<string>()),
                BaseLocation = dto.BaseLocation,
                ServiceRegions = dto.ServiceRegions,
                TruckImagePath = imagePath,
                Id = GuideHelper.GetGuide()
            };

            await _applicationDbContext.AddAsync(supplier, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return RedirectToAction("index", "supplier");
        }
    }
}
