using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Commands.Vehicle;

namespace TransitionApp.Domain.Validations
{
    public class CreateVehicleCommandValidation : VehicleValidation<CreateVehicleCommand>
    {
        public CreateVehicleCommandValidation()
        {
            ValidateName();
        }
    }
}
