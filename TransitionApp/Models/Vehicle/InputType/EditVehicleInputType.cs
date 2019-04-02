using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.RequestModel;

namespace TransitionApp.Models.Vehicle.InputType
{
    public class EditVehicleInputType: InputObjectGraphType<CreateVehicleRequest>
    {
        public readonly string AliasVolume = "volume";
        public readonly string AliasLicensePlate = "licensePlate";
        public readonly string AliasCapacity = "capacity";
        public readonly string AliasId = "id";

        public EditVehicleInputType()
        {
            Field(AliasId, x => x.Id);
            Field(AliasLicensePlate, x => x.LicensePlate, nullable: true);
            Field(AliasCapacity, x => x.Capacity, nullable: true);
            Field(AliasVolume, x => x.Volume, nullable: true);
        }
    }
}
