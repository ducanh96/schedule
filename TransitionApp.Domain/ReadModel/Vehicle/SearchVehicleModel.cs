using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Vehicle
{
    public class SearchVehicleModel
    {
        public string Code { get; set; }
        public string LicensePlate { get; set; }
        public int VehicleType { get; set; }
        public string Name { get; set; }
    }
}
