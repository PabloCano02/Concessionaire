using Concessionaire.Common;
using Concessionaire.Data.Entities;

namespace Concessionaire.Models
{
    public class HomeViewModel
    {
        public PaginatedList<Vehicle> Vehicles { get; set; }

        public ICollection<Brand> Brands { get; set; }
        
        public float Quantity { get; set; }
    }
}
