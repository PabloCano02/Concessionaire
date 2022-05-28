using Concessionaire.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Models
{
    public class ShowCartViewModel
    {
        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string Remarks { get; set; }

        public ICollection<TemporalReserve> TemporalReserves { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => TemporalReserves == null ? 0 : TemporalReserves.Sum(tr => tr.Value);

    }
}
