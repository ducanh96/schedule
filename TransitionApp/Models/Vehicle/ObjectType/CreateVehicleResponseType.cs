using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.ResponseModel.Vehicle;

namespace TransitionApp.Models.Vehicle.ObjectType
{
    public class CreateVehicleResponseType : ObjectGraphType<CreateVehicleResponse>
    {
        public CreateVehicleResponseType()
        {
            Field(x => x.Message);
            Field(x => x.Code);
            
        }

    }
}
