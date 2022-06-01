using Concessionaire.Data;
using Concessionaire.Enums;
using Concessionaire.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Concessionaire.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public DashboardController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.VehiclesCount = _context.Vehicles.Count();
            ViewBag.NewOrdersCount = _context.Reserves.Where(o => o.OrderStatus == OrderStatus.Nuevo).Count();
            ViewBag.ConfirmedOrdersCount = _context.Reserves.Where(o => o.OrderStatus == OrderStatus.Confirmado).Count();
            ViewBag.CanceledOrdersCount = _context.Reserves.Where(o => o.OrderStatus == OrderStatus.Cancelado).Count();

            return View(await _context.TemporalReserves
                    .Include(tr => tr.User)
                    .Include(tr => tr.Vehicle)
                    .ToListAsync());
        }
    }
}
