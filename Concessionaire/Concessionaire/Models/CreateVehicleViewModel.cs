using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Concessionaire.Models
{
    public class CreateVehicleViewModel : EditVehicleViewModel
    {
        [Display(Name = "Marca")]
        [Range(1, int.MaxValue, ErrorMessage = "[Debes seleccionar una categoría...]")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int BrandId { get; set; }

        public IEnumerable<SelectListItem> Brands { get; set; }

        [Display(Name = "Tipo de Vehículo")]
        [Range(1, int.MaxValue, ErrorMessage = "[Debes seleccionar un tipo de vehículo...]")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int VehicleTypeId { get; set; }

        public IEnumerable<SelectListItem> VehicleTypes { get; set; }

        [Display(Name = "Foto")]
        public IFormFile? ImageFile { get; set; }
    }
}
