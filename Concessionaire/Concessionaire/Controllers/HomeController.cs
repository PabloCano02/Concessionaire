using Concessionaire.Data;
using Concessionaire.Data.Entities;
using Concessionaire.Helpers;
using Concessionaire.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Vereyon.Web;

namespace Concessionaire.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IFlashMessage _flashMessage;

        public HomeController(ILogger<HomeController> logger, DataContext context, IUserHelper userHelper, IFlashMessage flashMessage)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            List<Vehicle>? vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.Brand)
                .Include(v => v.VehiclePhotos)
                .OrderBy(v => v.Brand.Name)
                .ToListAsync();

            HomeViewModel homeModel = new() { Vehicles = vehicles };
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                homeModel.Quantity = await _context.TemporalReserves
                    .Where(ts => ts.User.Id == user.Id)
                    .SumAsync(ts => ts.Quantity);
            }

            return View(homeModel);
        }

        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                _flashMessage.Info("Señor(a) usuario(a) para adicionar vehículos al carrito de compras, primero debe ingresar sus credenciales. En caso de olvidarlas, dar clic en la opción ¿Has olvidado tu contraseña; de lo contrario es necesario que se registre en nuestra base de datos dando clic en la opción Registrar Nuevo Usuario.");
                return RedirectToAction("Login", "Account");
            }

            Vehicle vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            TemporalReserve temporalReserve = new()
            {
                Vehicle = vehicle,
                Quantity = 1,
                User = user
            };

            _context.TemporalReserves.Add(temporalReserve);
            await _context.SaveChangesAsync();
            _flashMessage.Confirmation("Se ha adicionado un vehículo al carrito de compras.");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}