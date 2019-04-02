using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Validations;

namespace TransitionApp.Domain.Commands.Vehicle
{
    public class DeleteVehicleCommand : VehicleCommand
    {
        public override bool IsValid(Object obj =null)
        {
            ValidationResult = new DeleteVehicleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
