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
        private readonly IFileService _uploadFileService;

        public SupplierOnboardController(ApplicationDbContext applicationDbContext, IFileService uploadFileService)
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
            ModelState.Remove(nameof(dto.Id));
            if (!ModelState.IsValid)
                return View("Index", dto);

            ShipmentFile shipmentFile = null;
            if (dto.TruckImage != null && dto.TruckImage.Length > 0)
            {
                shipmentFile  =  await _uploadFileService.UploadFileAsync(dto.TruckImage,"Onboarding Phase", cancellationToken);
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
                TruckImagePath = shipmentFile.Id ?? null,
                Id = GuideHelper.GetGuide()
            };

            await _applicationDbContext.AddAsync(supplier, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            TempData["suppplier_success"] = "Registration succesfully Done!!";
            return RedirectToAction("index", "supplier");
        }
    }
}
