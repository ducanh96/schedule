using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands;
using TransitionApp.Domain.Commands.Vehicle;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.Model.ValueObject;
using TransitionApp.Domain.Notifications;
using TransitionApp.Domain.ReadModel;

namespace TransitionApp.Domain.CommandHanders
{
    public class VehicleCommandHandler :
        CommandHandler,
        IRequestHandler<CreateVehicleCommand, object>,
        IRequestHandler<DeleteVehicleCommand, object>,
        IRequestHandler<EditVehicleCommand, object>,
        IRequestHandler<ImportVehicleCommand, object>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMediatorHandler _bus;

        public VehicleCommandHandler(IVehicleRepository vehicleRepository, IMediatorHandler bus) : base(bus)
        {
            _vehicleRepository = vehicleRepository;
            _bus = bus;

        }
        public Task<object> Handle(CreateVehicleCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return Task.FromResult(false as object);
            }
            Console.WriteLine("123456");
            Vehicle vehicle = new Vehicle(
                id: null,
                licensePlate: new LicensePlate(command.LicensePlate),
                driver: new Driver(new Identity(command.DriverID)),
                maxLoad: new MaxLoad(command.MaxLoad),
                name: new Name(command.Name),
                vehicleType: new VehicleType(command.TypeID),
                note: new Note(command.Note),
                code: new Code(command.Code),
                volume: new Volume(command.Volume)
                );
            var vehicleModel =  _vehicleRepository.Add(vehicle);
            _bus.RaiseEvent(new DomainNotification("", "Create successfully!!", TypeNotification.Success));
            return Task.FromResult(vehicleModel as object);

        }

        public Task<object> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return Task.FromResult(false as object);
            }
            Console.WriteLine("654321");
            return Task.FromResult(false as object);

        }

        public Task<object> Handle(EditVehicleCommand command, CancellationToken cancellationToken)
        {

            if (!command.IsValid(_vehicleRepository))
            {
                NotifyValidationErrors(command);
                return Task.FromResult(false as object);
            }

            Vehicle vehicle = new Vehicle(
              id: command.ID,
              licensePlate: new LicensePlate(command.LicensePlate),
              driver: new Driver(new Identity(command.DriverID)),
              maxLoad: new MaxLoad(command.MaxLoad),
              name: new Name(command.Name),
              vehicleType: new VehicleType(command.TypeID),
              note: new Note(command.Note),
              code: new Code(command.Code),
              volume: new Volume(command.Volume)
              
              );
            var vehicleModel = _vehicleRepository.Edit(vehicle);
            return Task.FromResult(vehicleModel as object);
        }

        public Task<object> Handle(ImportVehicleCommand request, CancellationToken cancellationToken)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            request.Vehicles.ForEach(x =>
            {
                var vehicle = new Vehicle(
                    id: x.ID,
                    licensePlate: new LicensePlate(x.LicensePlate),
                    driver: new Driver(new Identity(x.DriverID.Value)),
                    maxLoad: new MaxLoad(x.MaxLoad),
                    name: new Name(x.Name),
                    vehicleType: new VehicleType(x.TypeVehicleID.Value),
                    note: new Note(x.Note),
                    code: new Code(x.Code),
                    volume: new Volume(x.Volume)
                    );

                vehicles.Add(vehicle);
            });
            var result = _vehicleRepository.ImportExcel(vehicles);
            return Task.FromResult(result as object);
        }
    }
}
