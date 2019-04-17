using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.Interface;
using TransitionApp.Application.ResponseModel;
using TransitionApp.Domain.ReadModel;
using TransitionApp.Domain.ReadModel.Driver;

namespace TransitionApp.Models
{
    public class VehicleType : ObjectGraphType<VehicleResponse>
    {
        public VehicleType()
        {
            Field(x => x.Vehicle.Id);
            Field(x => x.Vehicle.LicensePlate);
            Field(x => x.Status);
            Field<ListGraphType<DriverType>>("driver",
                resolve: x => new List<DriverReadModel>
                {
                    new DriverReadModel
                    {
                        ID = 1
                      
                    },
                    new DriverReadModel
                    {
                        ID = 2
                       
                    }
                });
            
        }
    }
}
