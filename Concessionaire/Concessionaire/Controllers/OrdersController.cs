using Concessionaire.Data;
using Concessionaire.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Concessionaire.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly DataContext _context;

        public OrdersController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Reserves
                .Include(r => r.User)
                .Include(r => r.ReserveDetails)
                .ThenInclude(rd => rd.Vehicle)
                .ToListAsync());
        }

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

    }
}
