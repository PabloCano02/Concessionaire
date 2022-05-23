using Concessionaire.Data;
using Concessionaire.Data.Entities;
using Concessionaire.Helpers;
using Concessionaire.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace Concessionaire.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehiclesController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;

        public VehiclesController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
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
                    _flashMessage.Confirmation("Vehículo adicionado con éxito.");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un vehículo con la misma placa.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }

            vehicleModel.Brands = await _combosHelper.GetComboBrandsAsync();
            vehicleModel.VehicleTypes = await _combosHelper.GetComboVehicleTypesAsync();
            return View(vehicleModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vehicle vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            EditVehicleViewModel vehicleModel = new()
            {
                Id = vehicle.Id,
                Plaque = vehicle.Plaque,
                Line = vehicle.Line,
                Model = vehicle.Model,
                Color = vehicle.Color,
                Description = vehicle.Description,
                Price = vehicle.Price,
                IsRent = vehicle.IsRent,
            };

            return View(vehicleModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditVehicleViewModel vehicleModel)
        {
            if (id != vehicleModel.Id)
            {
                return NotFound();
            }

            try
            {
                Vehicle vehicle = await _context.Vehicles.FindAsync(vehicleModel.Id);
                vehicle.Plaque = vehicleModel.Plaque;
                vehicle.Line = vehicleModel.Line;
                vehicle.Model = vehicleModel.Model;
                vehicle.Color = vehicleModel.Color;
                vehicle.Description = vehicleModel.Description;
                vehicle.Price = vehicleModel.Price;
                vehicle.IsRent = vehicleModel.IsRent;

                _context.Update(vehicle);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Vehículo editado con éxito.");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    _flashMessage.Danger("Ya existe un vehículo con la misma placa.");
                }
                else
                {
                    _flashMessage.Danger(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                _flashMessage.Danger(exception.Message);
            }

            return View(vehicleModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vehicle vehicle = await _context.Vehicles
                .Include(v => v.Brand)
                .Include(v => v.VehicleType)
                .Include(v => v.VehiclePhotos)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vehicle vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            AddVehiclePhotoViewModel model = new()
            {
                VehicleId = vehicle.Id,
            };

            return View(model);
        }

        //TODO: change "products" for blob "vehicles"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddVehiclePhotoViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                Vehicle vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
                VehiclePhoto vehiclePhoto = new()
                {
                    Vehicle = vehicle,
                    ImageId = imageId,
                };

                try
                {
                    _context.Add(vehiclePhoto);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Imagen adicionada con éxito.");
                    return RedirectToAction(nameof(Details), new { Id = vehicle.Id });
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VehiclePhoto vehiclePhoto = await _context.VehiclePhotos
                .Include(vp => vp.Vehicle)
                .FirstOrDefaultAsync(vp => vp.Id == id);
            if (vehiclePhoto == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(vehiclePhoto.ImageId, "products");
            _context.VehiclePhotos.Remove(vehiclePhoto);
            await _context.SaveChangesAsync();
            _flashMessage.Confirmation("Imagen removida con éxito.");
            return RedirectToAction(nameof(Details), new { Id = vehiclePhoto.Vehicle.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vehicle vehicle = await _context.Vehicles
                .Include(v => v.VehiclePhotos)
                .Include(v => v.VehicleType)
                .Include(v => v.Brand)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Vehicle deleteModel)
        {
            Vehicle vehicle = await _context.Vehicles
                .Include(v => v.VehiclePhotos)
                .Include(v => v.VehicleType)
                .Include(v => v.Brand)
                .FirstOrDefaultAsync(v => v.Id == deleteModel.Id);

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            _flashMessage.Confirmation("Vehículo borrado con éxito.");

            foreach (VehiclePhoto vehiclePhoto in vehicle.VehiclePhotos)
            {
                await _blobHelper.DeleteBlobAsync(vehiclePhoto.ImageId, "products");
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
