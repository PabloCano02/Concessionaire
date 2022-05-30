using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Data.Entities
{
    public class ReserveDetail
    {
        public int Id { get; set; }

        public Reserve Reserve { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string Remarks { get; set; }

        public Vehicle Vehicle { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [Display(Name = "Fecha inicial")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime InitialDate { get; set; }

        [Display(Name = "Fecha final")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime FinalDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => Vehicle == null ? 0 : (decimal)DaysRent * Vehicle.Price;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Días")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int DaysRent => (FinalDate - InitialDate).Days;

    }
}
