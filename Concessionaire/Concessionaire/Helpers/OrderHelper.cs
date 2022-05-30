using Concessionaire.Common;
using Concessionaire.Data;
using Concessionaire.Data.Entities;
using Concessionaire.Enums;
using Concessionaire.Models;

namespace Concessionaire.Helpers
{
    public class OrderHelper : IOrderHelper
    {
        private readonly DataContext _context;

        public OrderHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<Response> ProcessOrderAsync(ShowCartViewModel model)
        {
            Response response = await CheckInventoryAsync(model);
            if (!response.IsSuccess)
            {
                return response;
            }

            Reserve reserve = new()
            {
                Date = DateTime.UtcNow,
                User = model.User,
                Remarks = model.Remarks,
                ReserveDetails = new List<ReserveDetail>(),
                OrderStatus = OrderStatus.Nuevo
            };

            foreach (TemporalReserve item in model.TemporalReserves)
            {
                reserve.ReserveDetails.Add(new ReserveDetail
                {
                    Vehicle = item.Vehicle,
                    Quantity = item.Quantity,
                    InitialDate = item.InitialDate,
                    FinalDate = item.FinalDate,
                    Remarks = item.Remarks,
                });

                Vehicle vehicle = await _context.Vehicles.FindAsync(item.Vehicle.Id);
                if (vehicle != null)
                {
                    vehicle.IsRent = false;
                    _context.Vehicles.Update(vehicle);
                }

                _context.TemporalReserves.Remove(item);
            }

            _context.Reserves.Add(reserve);
            await _context.SaveChangesAsync();
            return response;
        }

        private async Task<Response> CheckInventoryAsync(ShowCartViewModel model)
        {
            Response response = new() { IsSuccess = true };
            foreach (TemporalReserve item in model.TemporalReserves)
            {
                Vehicle vehicle = await _context.Vehicles.FindAsync(item.Vehicle.Id);
                if (vehicle == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"El vehículo {item.Vehicle.Brand.Name} {item.Vehicle.Line}, no está disponible en este momento.";
                    return response;
                }
                if (vehicle.IsRent == false)
                {
                    response.IsSuccess = false;
                    response.Message = $"El vehículo {item.Vehicle.Brand.Name} {item.Vehicle.Line}, no está disponible en este momento. Por favor sustituirlo por otro.";
                    return response;
                }
            }
            return response;
        }
    }

}
