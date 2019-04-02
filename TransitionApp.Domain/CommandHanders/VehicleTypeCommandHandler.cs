using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands.VehicleType;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.CommandHanders
{
    public class VehicleTypeCommandHandler :
        CommandHandler,
        IRequestHandler<CreateVehicleTypeCommand, object>,
        IRequestHandler<DeleteVehicleTypeCommand, object>
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IMediatorHandler _bus;

        public VehicleTypeCommandHandler(IVehicleTypeRepository vehicleTypeRepository, IMediatorHandler bus) : base(bus)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
            _bus = bus;

        }
        public async Task<object> Handle(CreateVehicleTypeCommand command, CancellationToken cancellationToken)
        {
            //if (!command.IsValid())
            //{
            //    NotifyValidationErrors(command);
            //    return Task.FromResult(false as object);
            //}
            VehicleType vehicle = new VehicleType(new Code(command.Code), new Name(command.Name));
            var result = await _vehicleTypeRepository.Add(vehicle);
          
            return await Task.FromResult(result as object);
        }

        public async Task<object> Handle(DeleteVehicleTypeCommand command, CancellationToken cancellationToken)
        {
            var result = await _vehicleTypeRepository.Delete(command.Id);
            return await Task.FromResult(result as object);
        }
    }
}
