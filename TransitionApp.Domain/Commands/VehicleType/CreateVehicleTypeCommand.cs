using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.Commands.VehicleType
{
    public class CreateVehicleTypeCommand : VehicleTypeCommand
    {
        public override bool IsValid(object obj = null)
        {
            // khoi tao command validate
            throw new NotImplementedException();
        }
    }
}
