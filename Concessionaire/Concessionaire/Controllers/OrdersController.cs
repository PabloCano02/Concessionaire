using Concessionaire.Data;
using Concessionaire.Data.Entities;
using Concessionaire.Enums;
using Concessionaire.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace Concessionaire.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;
        private readonly IOrderHelper _orderHelper;

        public OrdersController(DataContext context, IFlashMessage flashMessage, IOrderHelper orderHelper)
        {
            _context = context;
            _flashMessage = flashMessage;
            _orderHelper = orderHelper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reserves
                .Include(r => r.User)
                .Include(r => r.ReserveDetails)
                .ThenInclude(rd => rd.Vehicle)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reserve reserve = await _context.Reserves
                .Include(s => s.User)
                .Include(s => s.ReserveDetails)
                .ThenInclude(rd => rd.Vehicle)
                .ThenInclude(v => v.VehiclePhotos)
                .Include(s => s.ReserveDetails)
                .ThenInclude(rd => rd.Vehicle)
                .ThenInclude(v => v.Brand)
                .Include(s => s.ReserveDetails)
                .ThenInclude(rd => rd.Vehicle)
                .ThenInclude(v => v.VehicleType)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (reserve == null)
            {
                return NotFound();
            }

            return View(reserve);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reserve reserve = await _context.Reserves.FindAsync(id);
            if (reserve == null)
            {
                return NotFound();
            }

            if (reserve.OrderStatus != OrderStatus.Nuevo)
            {
                _flashMessage.Danger("Solo se pueden confirmar reservas que estén en estado 'Nuevo'.");
            }
            else
            {
                reserve.OrderStatus = OrderStatus.Confirmado;
                _context.Reserves.Update(reserve);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado de la reserva ha sido cambiado a 'confirmado'.");
            }

            return RedirectToAction(nameof(Details), new { Id = reserve.Id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reserve reserve = await _context.Reserves.FindAsync(id);
            if (reserve == null)
            {
                return NotFound();
            }

            if (reserve.OrderStatus == OrderStatus.Cancelado)
            {
                _flashMessage.Danger("No se puede cancelar una reservación que esté en estado 'cancelado'.");
            }
            else
            {
                await _orderHelper.CancelOrderAsync(reserve.Id);
                _flashMessage.Confirmation("El estado de la reserva ha sido cambiado a 'cancelado'.");
            }

            return RedirectToAction(nameof(Details), new { Id = reserve.Id });
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyOrders()
        {
            return View(await _context.Reserves
                .Include(r => r.User)
                .Include(r => r.ReserveDetails)
                .ThenInclude(rd => rd.Vehicle)
                .Where(s => s.User.UserName == User.Identity.Name)
                .ToListAsync());
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reserve reserve = await _context.Reserves
                .Include(s => s.User)
                .Include(s => s.ReserveDetails)
                .ThenInclude(rd => rd.Vehicle)
                .ThenInclude(v => v.VehiclePhotos)
                .Include(s => s.ReserveDetails)
                .ThenInclude(rd => rd.Vehicle)
                .ThenInclude(v => v.Brand)
                .Include(s => s.ReserveDetails)
                .ThenInclude(rd => rd.Vehicle)
                .ThenInclude(v => v.VehicleType)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (reserve == null)
            {
                return NotFound();
            }

            return View(reserve);
        }

    }
}
