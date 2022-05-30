using Concessionaire.Common;
using Concessionaire.Models;

namespace Concessionaire.Helpers
{
    public interface IOrderHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);

        Task<Response> CancelOrderAsync(int id);
    }
}
