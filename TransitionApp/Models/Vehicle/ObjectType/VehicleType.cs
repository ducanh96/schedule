using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel;

namespace TransitionApp.Models.Vehicle.ObjectType
{
    public class VehicleType : ObjectGraphType<VehicleReadModel>
    {
        public VehicleType()
        {
            Field(x => x.LicensePlate);
            Field(x => x.Volume);
            Field(x => x.Capacity);
        }
    }
}
