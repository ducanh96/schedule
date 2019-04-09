using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.ResponseModel.Vehicle
{
    public class GetVehicleResponse
    {
        public StatusResponse Status { get; set; }
        public IEnumerable<Object> Vehicles { get; set; }
    }
}
