using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Data.Entities
{
    public class TemporalReserve
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Vehicle Vehicle { get; set; }

        [Display(Name = "Fecha inicial")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime InitialDate { get; set; }

        [Display(Name = "Fecha final")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime FinalDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad Vehículos")]
        [Range(0.0000001, float.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la cantidad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => Vehicle == null ? 0 : (decimal)DaysRent * Vehicle.Price;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Días")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int DaysRent => (FinalDate - InitialDate).Days;
    }
}
