using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Validations;

namespace TransitionApp.Domain.Commands.Vehicle
{
    public class EditVehicleCommand : VehicleCommand
    {
       

       
        public override bool IsValid(Object obj = null)
        {
            ValidationResult = new EditVehicleCommandValidation(obj as IVehicleRepository).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
