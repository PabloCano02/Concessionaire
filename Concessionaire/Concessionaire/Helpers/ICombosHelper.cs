using Microsoft.AspNetCore.Mvc.Rendering;

namespace Concessionaire.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboBrandAsync();

        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId);

        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();

        Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId);

        Task<IEnumerable<SelectListItem>> GetComboVehicleTypesAsync();
    }
}
