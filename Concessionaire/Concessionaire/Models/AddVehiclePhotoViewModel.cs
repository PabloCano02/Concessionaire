﻿using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Models
{
    public class AddVehiclePhotoViewModel
    {
        public int VehicleId { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public IFormFile ImageFile { get; set; }
    }
}
