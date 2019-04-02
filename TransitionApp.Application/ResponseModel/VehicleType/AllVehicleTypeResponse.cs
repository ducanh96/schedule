using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.ReadModel.VehicleType;

namespace TransitionApp.Application.ResponseModel.VehicleType
{
    public class AllVehicleTypeResponse:VehicleTypeResponse
    {
        public IEnumerable<VehicleTypeReadModel> Vehicletypes { get; set; }
    }
}
