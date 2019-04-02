using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.RequestModel;
using TransitionApp.Application.ResponseModel;
using TransitionApp.Models.Vehicle;
using TransitionApp.Models.Vehicle.InputType;
using TransitionApp.Models.Vehicle.ObjectType;

namespace TransitionApp.Models
{
    public class TransitionAppMutation : ObjectGraphType
    {

        public TransitionAppMutation(IVehicleAppService vehicleAppService)
        {
            Name = "Mutation";
            //Field<BaseResponseType>(
            //    "createVehicle",
            //    arguments: new QueryArguments(
            //        new QueryArgument<NonNullGraphType<VehicleInputType>>{
            //            Name = "item"
            //        }
            //        ),
            //    resolve: x =>
            //    {
            //        var licensePlate = x.GetArgument<CreateVehicleRequest>("item");
            //        var vehicleRequest = new CreateVehicleRequest
            //        {
            //            LicensePlate = licensePlate.LicensePlate,
            //            Capacity = licensePlate.Capacity,
            //            Volume = licensePlate.Volume

            //        };     
            //        return vehicleAppService.Create(vehicleRequest);
            //    }
            //    );

            Field<BaseResponseType>(
               "editVehicle",
               arguments: new QueryArguments(
                   new QueryArgument<EditVehicleInputType>
                   {
                       Name = "itemEdit"
                   }
                   ),
               resolve: x =>
               {
                   var licensePlate = x.GetArgument<CreateVehicleRequest>("itemEdit");
                   var vehicleRequest = new CreateVehicleRequest
                   {
                       Id = licensePlate.Id,
                       LicensePlate = licensePlate.LicensePlate,
                       Capacity = licensePlate.Capacity,
                       Volume = licensePlate.Volume

                   };
                   return vehicleAppService.Edit(vehicleRequest);
               }
               );
            Field<BaseResponseType>(
              "deleteVehicle",
              arguments: new QueryArguments(
                  new QueryArgument<EditVehicleInputType>
                  {
                      Name = "item"
                  }
                  ),
              resolve: x =>
              {
                  var licensePlate = x.GetArgument<CreateVehicleRequest>("item");
                  var vehicleRequest = new CreateVehicleRequest
                  {
                      Id = licensePlate.Id,
                      LicensePlate = licensePlate.LicensePlate,
                      Capacity = licensePlate.Capacity,
                      Volume = licensePlate.Volume

                  };
                  return vehicleAppService.Edit(vehicleRequest);
              }
              );
            //Field<BaseResponseType>(
            //    "deleteVehicle",
            //    arguments: new QueryArguments(
            //        new QueryArgument<NonNullGraphType><VehicleInputType>
            //    )
        }
    }
}
