﻿using Concessionaire.Common;
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
        private readonly IOrderHelper _orderHelper;

        public HomeController(ILogger<HomeController> logger, DataContext context, IUserHelper userHelper, IFlashMessage flashMessage, IOrderHelper orderHelper)
        {
            _logger = logger;
            _context = context;
            _userHelper = userHelper;
            _flashMessage = flashMessage;
            _orderHelper = orderHelper;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["BrandSortParm"] = String.IsNullOrEmpty(sortOrder) ? "BrandDesc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "PriceDesc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            IQueryable<Vehicle> query = _context.Vehicles
                .Include(v => v.VehiclePhotos)
                .Include(v => v.Brand)
                .Include(v => v.VehicleType)
                .Where(v => v.IsRent == true);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(v => (v.Brand.Name.ToLower().Contains(searchString.ToLower()) ||
                                          v.VehicleType.Name.ToLower().Contains(searchString.ToLower())) &&
                                          v.IsRent == true);
            }
            else
            {
                query = query.Where(v => v.IsRent == true);
            }


            switch (sortOrder)
            {
                case "BrandDesc":
                    query = query.OrderByDescending(v => v.Brand.Name);
                    break;
                case "Price":
                    query = query.OrderBy(v => v.Price);
                    break;
                case "PriceDesc":
                    query = query.OrderByDescending(v => v.Price);
                    break;
                default:
                    query = query.OrderBy(v => v.Brand.Name);
                    break;
            }

            int pageSize = 8;

            HomeViewModel homeModel = new()
            {
                Vehicles = await PaginatedList<Vehicle>.CreateAsync(query, pageNumber ?? 1, pageSize),
                Brands = await _context.Brands.ToListAsync(),
            };

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                homeModel.Quantity = await _context.TemporalReserves
                    .Where(tr => tr.User.Id == user.Id)
                    .SumAsync(tr => tr.Quantity);
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
                _flashMessage.Info("Señor(a) usuario(a) para adicionar vehículos al carrito de compras, primero debe ingresar sus credenciales. En caso de olvidarlas, dar clic en la opción ¿Has olvidado tu contraseña?; de lo contrario es necesario que se registre en nuestra base de datos dando clic en la opción Registrar Nuevo Usuario.");
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
                    _flashMessage.Confirmation("Se adicionó exitosamente el vehículo al carro de compras.");
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
                _flashMessage.Info("Señor(a) usuario(a) para adicionar vehículos al carrito de compras, primero debe ingresar sus credenciales. En caso de olvidarlas, dar clic en la opción ¿Has olvidado tu contraseña?; de lo contrario es necesario que se registre en nuestra base de datos dando clic en la opción Registrar Nuevo Usuario.");
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

            if (user != null)
            {
                detailsModel.Quantity = await _context.TemporalReserves
                    .Where(tr => tr.User.Id == user.Id)
                    .SumAsync(tr => tr.Quantity);
            }

            TemporalReserve temporalReserve = new()
            {
                Vehicle = vehicle,
                Quantity = detailsModel.Quantity,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowCart(ShowCartViewModel model)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            model.User = user;
            model.TemporalReserves = await _context.TemporalReserves
                .Include(tr => tr.Vehicle)
                .ThenInclude(v => v.VehiclePhotos)
                .Where(tr => tr.User.Id == user.Id)
                .ToListAsync();
            Response response = await _orderHelper.ProcessOrderAsync(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(OrderSuccess));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalReserve temporalReserve = await _context.TemporalReserves.FindAsync(id);
            if (temporalReserve == null)
            {
                return NotFound();
            }

            _context.TemporalReserves.Remove(temporalReserve);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Se borró exitosamente el vehículo del carro de compras.");
            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalReserve temporalReserve = await _context.TemporalReserves.FindAsync(id);
            if (temporalReserve == null)
            {
                return NotFound();
            }

            EditTemporalReserveViewModel model = new()
            {
                Id = temporalReserve.Id,
                InitialDate = temporalReserve.InitialDate,
                FinalDate = temporalReserve.FinalDate,
                Remarks = temporalReserve.Remarks,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTemporalReserveViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TemporalReserve temporalReserve = await _context.TemporalReserves.FindAsync(id);
                    temporalReserve.InitialDate = model.InitialDate;
                    temporalReserve.FinalDate = model.FinalDate;
                    temporalReserve.Remarks = model.Remarks;
                    _context.Update(temporalReserve);
                    await _context.SaveChangesAsync();
                    _flashMessage.Info("Los datos de la reserva se han modificado con éxito.");
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                    return View(model);
                }

                return RedirectToAction(nameof(ShowCart));
            }

            return View(model);
        }

        [Authorize]
        public IActionResult OrderSuccess()
        {
            return View();
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