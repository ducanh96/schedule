using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.ReadModel;

namespace TransitionApp.Application.ResponseModel.Vehicle
{
    public class GetAllVehicleResponse
    {
        public List<VehicleReadModel> Vehicles { get; set; }
    }
}
