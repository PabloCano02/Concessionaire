namespace Concessionaire.Models
{
    public class HomeViewModel
    {
        public ICollection<VehiclesHomeViewModel> Vehicles { get; set; }

        public float Quantity { get; set; }
    }
}
