using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel;
using static TransitionApp.Application.Shared.Common;

namespace TransitionApp.Application.ResponseModel.Vehicle
{
    public class CreateVehicleResponse : BaseResponse
    {
        public VehicleReadModel Vehicle { get; set; }


        public CreateVehicleResponse(VehicleReadModel vehicleRead, string message, ResponseCode code = ResponseCode.Fail) : base(message, code)
        {
            Vehicle = vehicleRead;
        }
        public CreateVehicleResponse(VehicleReadModel task, BaseResponse baseResponse)
        {
            Vehicle = task;
            Message = baseResponse.Message;
            Code = baseResponse.Code;
        }
    }
}
