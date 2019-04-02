using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Application.RequestModel;

namespace TransitionApp.Models.Vehicle
{
    public class VehicleInputType : InputObjectGraphType<CreateVehicleRequest>
    {
        public readonly string AliasLicensePlate = "licensePlate";
        public readonly string AliasCapacity = "capacity";
        public readonly string AliasVolume = "volume";



        public VehicleInputType()
        {
            Name = "VehicleInput";
            Field(AliasLicensePlate, x => x.LicensePlate);
            Field(AliasCapacity, x => x.Capacity, nullable: true);
            Field(AliasVolume, x => x.Volume, nullable: true);
        }

    }

}
