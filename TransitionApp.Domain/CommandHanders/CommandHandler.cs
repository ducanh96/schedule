using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands;
using TransitionApp.Domain.Notifications;

namespace TransitionApp.Domain.CommandHanders
{
    public class CommandHandler
    {
        private readonly IMediatorHandler _bus;
        public CommandHandler(IMediatorHandler bus)
        {
            _bus = bus;
        }

        protected void NotifyValidationErrors(BaseCommand message)
        {
            List<string> errorInfo = new List<string>();
            foreach (var error in message.ValidationResult.Errors)
            {
  
                _bus.RaiseEvent(new DomainNotification("", error.ErrorMessage, TypeNotification.Fail));
            }

        }
    }
}
