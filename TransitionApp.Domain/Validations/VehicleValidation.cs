
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Commands;
using TransitionApp.Domain.Commands.Vehicle;
using TransitionApp.Domain.Interface.Repository;

namespace TransitionApp.Domain.Validations
{
    public abstract class VehicleValidation<T> : AbstractValidator<T> where T : VehicleCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.LicensePlate)
                .NotEmpty().WithMessage("Không dc để trống")
                .Length(2, 10).WithMessage("Not empty");
        }

        protected void ValidateId()
        {
            RuleFor(c => c.ID).NotEmpty();
        }

        protected void CheckExistId(IVehicleRepository _repository)
        {
            RuleFor(c => c.ID).Must(x => _repository.checkExist(x)).WithMessage("Not exist Vehicle");
        }

    }
}
