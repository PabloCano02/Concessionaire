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
    public class BrandsController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public BrandsController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands
                .Include(b=>b.Vehicles)
                .ToListAsync());
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? id)
        {
            Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            try
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar la marca porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Brand());
            }
            else
            {
                Brand brand = await _context.Brands.FindAsync(id);
                if (brand == null)
                {
                    return NotFound();
                }

                return View(brand);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Brand brand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0)
                    {
                        _context.Add(brand);
                        await _context.SaveChangesAsync();
                        _flashMessage.Confirmation("Registro creado.");
                    }
                    else
                    {
                        _context.Update(brand);
                        await _context.SaveChangesAsync();
                        _flashMessage.Confirmation("Registro actualizado.");
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una marca con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                    return View(brand);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllBrands", _context.Brands.Include(b => b.Vehicles).ToList()) });

            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", brand) });
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Brand brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

    }
}
