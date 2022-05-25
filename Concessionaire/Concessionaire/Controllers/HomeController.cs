using Concessionaire.Data;
using Concessionaire.Data.Entities;
using Concessionaire.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Concessionaire.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Vehicle>? vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.Brand)
                .Include(v => v.VehiclePhotos)
                .OrderBy(v => v.Brand.Name)
                .ToListAsync();

            List<VehiclesHomeViewModel> vehiclesHome = new() { new VehiclesHomeViewModel() };
            int i = 1;
            foreach (Vehicle? product in vehicles)
            {
                if (i == 1)
                {
                    vehiclesHome.LastOrDefault().Vehicle1 = product;
                }
                if (i == 2)
                {
                    vehiclesHome.LastOrDefault().Vehicle2 = product;
                }
                if (i == 3)
                {
                    vehiclesHome.LastOrDefault().Vehicle3 = product;
                }
                if (i == 4)
                {
                    vehiclesHome.LastOrDefault().Vehicle4 = product;
                    vehiclesHome.Add(new VehiclesHomeViewModel());
                    i = 0;
                }
                i++;
            }

            return View(vehiclesHome);
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