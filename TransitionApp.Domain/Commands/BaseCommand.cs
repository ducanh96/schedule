using FluentValidation.Results;
using MediatR;
using System;

namespace TransitionApp.Domain.Commands
{
    public abstract class BaseCommand : IRequest<object>
    {
        public ValidationResult ValidationResult { get; set; }
        public DateTime Timestamp { get; private set; }
        protected BaseCommand()
        { 
            Timestamp = DateTime.Now;
        }

        public abstract bool IsValid(Object obj = null);

    }
}
