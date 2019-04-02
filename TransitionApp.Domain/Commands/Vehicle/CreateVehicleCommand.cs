using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.Validations;

namespace TransitionApp.Domain.Commands.Vehicle
{
    public class CreateVehicleCommand : VehicleCommand
    {
        public override bool IsValid(Object obj=null)
        {
            ValidationResult = new CreateVehicleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}
