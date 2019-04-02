using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands;
using TransitionApp.Domain.Events;

namespace TransitionApp.Infrastructor.Bus
{
    public class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        public InMemoryBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task RaiseEvent<T>(T @event) where T : Event
        {
            return _mediator.Publish(@event);
        }

        public Task SendCommand<T>(T command) where T : BaseCommand
        {
            return _mediator.Send(command);
        }

        public Task SendCommandObj<T>(T command) where T : IRequest<object>
        {
            return _mediator.Send(command);
        }
    }
}
