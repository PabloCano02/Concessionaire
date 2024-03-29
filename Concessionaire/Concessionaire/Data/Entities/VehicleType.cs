﻿using System.ComponentModel.DataAnnotations;

namespace Concessionaire.Data.Entities
{
    public class VehicleType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de vehículo")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

        [Display(Name = "Número de vehículos")]
        public int VehiclesNumber => Vehicles == null ? 0 : Vehicles.Count();
    }
}
