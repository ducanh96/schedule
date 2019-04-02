using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Application.RequestModel;
using TransitionApp.Application.RequestModel.Vehicle;
using TransitionApp.Domain.ReadModel;

namespace TransitionApp.Application.ResponseModel.Vehicle
{
    public class SearchVehicleResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public StatusResponse Status { get; set; }
        public int Total { get; set; }
        //public IEnumerable<VehicleReadModel> Vehicles { get; set; }
        public IEnumerable<Object> Vehicles { get; set; }

    }
}
