using Concessionaire.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Concessionaire.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboBrandsAsync()
        {
            List<SelectListItem> list = await _context.Brands.Select(b => new SelectListItem
            {
                Text = b.Name,
                Value = b.Id.ToString()
            })
                .OrderBy(b => b.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione una marca...]", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
        {
            List<SelectListItem> list = await _context.Cities
                .Where(c => c.State.Id == stateId)
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione una ciudad...]", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCountriesAsync()
        {
            List<SelectListItem> list = await _context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
                .OrderBy(c => c.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un país...]", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId)
        {
            List<SelectListItem> list = await _context.States
                .Where(s => s.Country.Id == countryId)
                .Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            })
                .OrderBy(s => s.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un departamento/estado...]", Value = "0" });
            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboVehicleTypesAsync()
        {
            List<SelectListItem> list = await _context.VehicleTypes.Select(vt => new SelectListItem
            {
                Text = vt.Name,
                Value = vt.Id.ToString()
            })
                .OrderBy(vt => vt.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Seleccione un tipo de vehículo...]", Value = "0" });
            return list;
        }
    }
}
