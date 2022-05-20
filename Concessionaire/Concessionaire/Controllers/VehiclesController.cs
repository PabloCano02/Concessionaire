using Concessionaire.Data;
using Concessionaire.Data.Entities;
using Concessionaire.Helpers;
using Concessionaire.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Concessionaire.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehiclesController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;

        public VehiclesController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicles
                .Include(v => v.VehiclePhotos)
                .Include(v => v.VehicleType)
                .Include(v => v.Brand)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            CreateVehicleViewModel model = new()
            {
                Brands = await _combosHelper.GetComboBrandsAsync(),
                VehicleTypes = await _combosHelper.GetComboVehicleTypesAsync(),
            };

            return View(model);
        }

        //TODO Change in Blob "products" for "Vehicles"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVehicleViewModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (vehicleModel.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(vehicleModel.ImageFile, "products");
                }

                Vehicle vehicle = new()
                {
                    Plaque = vehicleModel.Plaque,
                    Line = vehicleModel.Line,
                    Model = vehicleModel.Model,
                    Color = vehicleModel.Color,
                    Description = vehicleModel.Description,
                    Price = vehicleModel.Price,
                    IsRent = vehicleModel.IsRent,
                    Brand = await _context.Brands.FindAsync(vehicleModel.BrandId),
                    VehicleType = await _context.VehicleTypes.FindAsync(vehicleModel.VehicleTypeId)
                };

                if (imageId != Guid.Empty)
                {
                    vehicle.VehiclePhotos = new List<VehiclePhoto>()
                    {
                        new VehiclePhoto { ImageId = imageId }
                    };
                }

                try
                {
                    _context.Add(vehicle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un vehículo con la misma placa.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            vehicleModel.Brands = await _combosHelper.GetComboBrandsAsync();
            vehicleModel.VehicleTypes = await _combosHelper.GetComboVehicleTypesAsync();
            return View(vehicleModel);
        }
    }
}
