using Concessionaire.Data.Entities;

namespace Concessionaire.Models
{
    public class HomeViewModel
    {
        public ICollection<Vehicle> Vehicles { get; set; }

        public float Quantity { get; set; }
    }
}
