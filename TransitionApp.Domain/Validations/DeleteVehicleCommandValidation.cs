using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Commands.Vehicle;

namespace TransitionApp.Domain.Validations
{
    public class DeleteVehicleCommandValidation : VehicleValidation<DeleteVehicleCommand>
    {
        public DeleteVehicleCommandValidation()
        {
            ValidateId();

        }
    }
}
