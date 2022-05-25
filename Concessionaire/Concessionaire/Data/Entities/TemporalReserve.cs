using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Data.Entities
{
    public class TemporalReserve
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Vehicle Vehicle { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }
    }
}
