using Concessionaire.Data;
using Concessionaire.Data.Entities;
using Concessionaire.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using static Concessionaire.Helpers.ModalHelper;

namespace Concessionaire.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehiclesTypesController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public VehiclesTypesController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehicleTypes
                .Include(vt => vt.Vehicles)
                .ToListAsync());
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new VehicleType());
            }
            else
            {
                VehicleType vehicleType = await _context.VehicleTypes.FindAsync(id);
                if (vehicleType == null)
                {
                    return NotFound();
                }

                return View(vehicleType);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0)
                    {
                        _context.Add(vehicleType);
                        await _context.SaveChangesAsync();
                        _flashMessage.Confirmation("Registro creado.");
                    }
                    else
                    {
                        _context.Update(vehicleType);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro actualizado.");
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un tipo de vehículo con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                    return View(vehicleType);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllVehicleTypes", _context.VehicleTypes.Include(vt => vt.Vehicles).ToList()) });
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", vehicleType) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? id)
        {
            VehicleType vehicleType = await _context.VehicleTypes.FirstOrDefaultAsync(vt => vt.Id == id);
            try
            {
                _context.VehicleTypes.Remove(vehicleType);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar la marca porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}