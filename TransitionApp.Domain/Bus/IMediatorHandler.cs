using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Commands;
using TransitionApp.Domain.Events;

namespace TransitionApp.Domain.Bus
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : BaseCommand;

        Task RaiseEvent<T>(T @event) where T : Event;

        Task SendCommandObj<T>(T command) where T : IRequest<object>;
    }
}
