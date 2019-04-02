using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Commands.Vehicle;
using TransitionApp.Domain.Interface.Repository;

namespace TransitionApp.Domain.Validations
{
    public class EditVehicleCommandValidation : VehicleValidation<EditVehicleCommand>
    {
       

        public EditVehicleCommandValidation(IVehicleRepository _repository)
        {
            //ValidateName();
            CheckExistId(_repository);

        }
     
    }
}
