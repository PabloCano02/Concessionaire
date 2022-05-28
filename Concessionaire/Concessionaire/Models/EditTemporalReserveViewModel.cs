using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Models
{
    public class EditTemporalReserveViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        [Display(Name = "Fecha inicial")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime InitialDate { get; set; }

        [Display(Name = "Fecha final")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime FinalDate { get; set; }
    }
}
