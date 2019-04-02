using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Application.ResponseModel;
using TransitionApp.Domain.ReadModel;

namespace TransitionApp.Application.RequestModel.Vehicle
{
    public class SearchVehicleRequest:BaseRequest
    {
        public int vehicleTypeID { get; set; }
        public string name { get; set; }
        public string licensePlate { get; set; }
        public string code { get; set; }
    }
}
