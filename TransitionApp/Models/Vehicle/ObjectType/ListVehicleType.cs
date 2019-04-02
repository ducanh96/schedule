using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.ResponseModel;
using TransitionApp.Application.ResponseModel.Vehicle;
using TransitionApp.Domain.ReadModel;

namespace TransitionApp.Models.Vehicle.ObjectType
{
    public class ListVehicleType : ObjectGraphType<BaseResponse>
    {
        public ListVehicleType(IVehicleAppService vehicleAppService)
        {

            //Field<ListGraphType<VehicleType>>("vehicles",
            //    resolve: x => vehicleAppService.GetAll()
            //    );
        }
    }
}
