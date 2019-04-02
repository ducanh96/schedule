using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.ResponseModel;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Models.Vehicle;
using TransitionApp.Models.Vehicle.ObjectType;

namespace TransitionApp.Models
{
    public class TransitionAppQuery : ObjectGraphType
    {
        public TransitionAppQuery(IVehicleAppService vehicleAppService)
        {
            Field<VehicleType>(
                "vehicle",
                resolve: x => vehicleAppService.GetAsync()
                );
            Field<ListVehicleType>(
                "GetAllVehicle",
                resolve: x => new BaseResponse()
                );

        }
    }
}
