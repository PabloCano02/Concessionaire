using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Data.Entities
{
    public class Brand
    {
        public int Id { get; set; }

        [Display(Name = "Marca")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

        [Display(Name = "Número de vehículos")]
        public int VehiclesNumber => Vehicles == null ? 0 : Vehicles.Count();

    }
}
