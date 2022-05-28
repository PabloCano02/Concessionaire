using Concessionaire.Data;
using Concessionaire.Data.Entities;
using Concessionaire.Helpers;
using Concessionaire.Models;
using Microsoft.AspNetCore.Authorization;
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
            List<Vehicle> vehicles = await _context.Vehicles
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

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            Vehicle vehicle = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.Brand)
                .Include(v => v.VehiclePhotos)
                .FirstOrDefaultAsync(v => v.Id == id);
            
            if (vehicle == null)
            {
                return NotFound();
            }

            AddVehicleToCartViewModel addModel = new()
            {
                VehicleId = vehicle.Id,
                Plaque = vehicle.Plaque,
                Brand = vehicle.Brand.Name,
                Line = vehicle.Line,
                Model = vehicle.Model,
                Color = vehicle.Color,
                Description = vehicle.Description,
                VehicleType = vehicle.VehicleType.Name,
                VehiclePhotos = vehicle.VehiclePhotos,
                Price = vehicle.Price,
                IsRent = vehicle.IsRent,
                Quantity = 1,
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now,
            };

            return View(addModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddVehicleToCartViewModel addModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    Vehicle vehicle = await _context.Vehicles.FindAsync(addModel.VehicleId);
                    if (vehicle == null)
                    {
                        return NotFound();
                    }

                    User user = await _userHelper.GetUserAsync(User.Identity.Name);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    TemporalReserve temporalSale = new()
                    {
                        Vehicle = vehicle,
                        Quantity = addModel.Quantity,
                        Remarks = addModel.Remarks,
                        InitialDate = addModel.InitialDate,
                        FinalDate = addModel.FinalDate,
                        User = user
                    };

                    _context.TemporalReserves.Add(temporalSale);
                    await _context.SaveChangesAsync();
                    _flashMessage.Danger("Se adicionó exitosamente el vehículo al carro de compras.");
                    return RedirectToAction(nameof(Index), new { Id = addModel.VehicleId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya se adicionó este vehículo al carro de compras.");
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
            return View(addModel);
        }

        public async Task<IActionResult> Details(int? id)
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

            AddVehicleToCartViewModel detailsModel = new()
            {
                VehicleId = vehicle.Id,
                VehicleType = vehicle.VehicleType.Name,
                Brand = vehicle.Brand.Name,
                Line = vehicle.Line,
                Model = vehicle.Model,
                Plaque = vehicle.Plaque,
                Color = vehicle.Color,
                Description = vehicle.Description,
                Price = vehicle.Price,
                IsRent = vehicle.IsRent,
                VehiclePhotos = vehicle.VehiclePhotos,
                Quantity = 1,
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now,
            };

            return View(detailsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(AddVehicleToCartViewModel detailsModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                _flashMessage.Info("Señor(a) usuario(a) para adicionar vehículos al carrito de compras, primero debe ingresar sus credenciales. En caso de olvidarlas, dar clic en la opción ¿Has olvidado tu contraseña; de lo contrario es necesario que se registre en nuestra base de datos dando clic en la opción Registrar Nuevo Usuario.");
                return RedirectToAction("Login", "Account");
            }

            Vehicle vehicle = await _context.Vehicles.FindAsync(detailsModel.Id);
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
                InitialDate = detailsModel.InitialDate,
                FinalDate = detailsModel.FinalDate,
                Remarks = detailsModel.Remarks,
                User = user
            };

            _context.TemporalReserves.Add(temporalReserve);
            await _context.SaveChangesAsync();
            _flashMessage.Confirmation("Se adicionó exitosamente el vehículo al carro de compras.");
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> ShowCart()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<TemporalReserve> temporalReserve = await _context.TemporalReserves
                .Include(tr => tr.Vehicle)
                .ThenInclude(v => v.VehiclePhotos)
                .Include(tr => tr.Vehicle)
                .ThenInclude(v => v.Brand)
                .Include(tr => tr.Vehicle)
                .ThenInclude(v => v.VehicleType)
                .Where(tr => tr.User.Id == user.Id)
                .ToListAsync();

            ShowCartViewModel cartModel = new()
            {
                User = user,
                TemporalReserves = temporalReserve,
            };

            return View(cartModel);
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