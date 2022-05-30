using Concessionaire.Enums;
using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Data.Entities
{
    public class Reserve
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string Remarks { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public ICollection<ReserveDetail> ReserveDetails { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Líneas")]
        public int Lines => ReserveDetails == null ? 0 : ReserveDetails.Count;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Quantity => ReserveDetails == null ? 0 : ReserveDetails.Sum(rd => rd.Quantity);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => ReserveDetails == null ? 0 : ReserveDetails.Sum(rd => rd.Value);

    }
}
