using Concessionaire.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Models
{
    public class AddVehicleToCartViewModel
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }

        [Display(Name = "Placa")]
        [RegularExpression(@"[a-zA-Z]{3}[0-9]{2}[a-zA-Z0-9]", ErrorMessage = "Formato de placa no válido")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Plaque { get; set; }

        [Display(Name = "Linea")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Line { get; set; }

        [Display(Name = "Modelo")]
        [Range(1900, 3000, ErrorMessage = "Valor de modelo no válido")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Model { get; set; }

        [Display(Name = "Color")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Color { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

        [Display(Name = "Disponible")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public bool IsRent { get; set; }

        [Display(Name = "Fecha inicial")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime InitialDate { get; set; }

        [Display(Name = "Fecha final")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime FinalDate { get; set; }

        [Display(Name = "Marca")]
        public string Brand { get; set; }

        [Display(Name = "Marca")]
        public string VehicleType { get; set; }

        public ICollection<VehiclePhoto> VehiclePhotos { get; set; }

        [Display(Name = "Cantidad")]
        [Range(0.0000001, float.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la cantidad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string Remarks { get; set; }

    }
}
