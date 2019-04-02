using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Model.Entity;

namespace TransitionApp.Application.RequestModel
{
    public class CreateVehicleRequest
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public double Volume { get; set; }
        public TypeVehicle TypeVehicle { get; set; }
        public string Image { get; set; }
        public double Capacity { get; set; }
    }
}
