using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands.Schedule;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.CommandHanders
{
    public class ScheduleCommandHandler :
        CommandHandler,
        IRequestHandler<CreateScheduleCommand, object>,
        IRequestHandler<DeleteScheduleCommand, object>
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IMediatorHandler _bus;
        public ScheduleCommandHandler(IScheduleRepository scheduleRepository, IMediatorHandler bus) : base(bus)
        {
            _scheduleRepository = scheduleRepository;
            _bus = bus;
        }
        public Task<object> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            List<Route> lstRoute = new List<Route>();
            // Map cac route ung vs 1 schedule
            foreach (var route in request.Routes)
            {
                List<Customer> customers = new List<Customer>();
                foreach (var customer in route.CustomerInfos)
                {
                    var customerData = new Customer(
                        new Identity(customer.CustomerID),
                        new Address(customer.Lat, customer.Lng),
                        customer.Invoices,
                        new Status(customer.Status),
                        new Weight(customer.Weight)
                        );
                    customers.Add(customerData);
                }
                Route dataRoute = new Route(
                    null,
                    new Identity(route.DriverID.Value),
                    new Distance(route.EstimatedDistance.Value),
                    new Duration(route.EstimatedDuration.Value),
                    new Status(route.Status.Value),
                    new Weight(route.Weight.Value),
                    new Address(route.DepotLat, route.DepotLng, route.DepotAddress),
                    null,
                    customers
                );

                lstRoute.Add(dataRoute);
            }

            Schedule schedule = new Schedule(
                new Distance(request.EstimatedDistance.Value),
                new Duration(request.EstimatedDuration.Value),
                new Name(request.Name),
                new Note(request.Note),
                new Number(request.NumberOfCustomers.Value),
                new Status(request.RouteManagerType.Value),
                new Status(request.Status.Value),
                new Weight(request.Weight.Value),
                new DeliveredAt(request.DeliveredAt),
                lstRoute
                );
            var result  = _scheduleRepository.Create(schedule);
            return Task.FromResult(result as object);
        }

        public Task<object> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            var result = _scheduleRepository.Delete(request.ID);
            return Task.FromResult(result as object);
        }
    }
}
