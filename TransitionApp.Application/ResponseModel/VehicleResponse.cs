using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Driver;

namespace TransitionApp.Application.ResponseModel
{
    public class VehicleResponse
    {
        public VehicleReadModel Vehicle { get; set; }
        public List<DriverReadModel> Driver { get; set; }
        public int Status { get; set; }
    }
}
