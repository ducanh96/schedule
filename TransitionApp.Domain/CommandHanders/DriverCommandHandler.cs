using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands.Driver;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.Notifications;

namespace TransitionApp.Domain.CommandHanders
{
    public class DriverCommandHandler :
        CommandHandler,
        IRequestHandler<CreateDriverCommand, object>,
        IRequestHandler<EditDriverCommand, object>,
        IRequestHandler<ImportDriverCommand, object>,
        IRequestHandler<DeleteDriverCommand, object>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IMediatorHandler _bus;

        public DriverCommandHandler(IDriverRepository driverRepository, IMediatorHandler bus) : base(bus)
        {
            _driverRepository = driverRepository;
            _bus = bus;
        }
        public Task<object> Handle(CreateDriverCommand command, CancellationToken cancellationToken)
        {
            Driver driver = new Driver(
                id: null,
                name: new Model.ValueObject.Name(command.Name),
                code: new Model.ValueObject.Code(command.Code),
                phoneNumber: new Model.ValueObject.PhoneNumber(command.PhoneNumber),
                status: new Model.ValueObject.Status(command.Status),
                address: new Model.ValueObject.Address(
                    city: command.City,
                    country: command.Country,
                    district: command.District,
                    street: command.Street,
                    streetNumber: command.StreetNumber
                ),
                dateOfBirth: new Model.ValueObject.Day(Convert.ToDateTime(command.DoB)),
                cardNumber: new Model.ValueObject.CardNumber(command.IDCardNumber),
                note: new Model.ValueObject.Note(command.Note),
                sex: new Model.ValueObject.Sex(command.Sex),
                startDate: new Model.ValueObject.Day(Convert.ToDateTime(command.StartDate)),
                userID: new Model.ValueObject.Identity(command.UserID),
                vehicleTypeID: new Model.ValueObject.VehicleTypeID(String.Join(',', command.VehicleTypeIDs))
                );

            var driverModel = _driverRepository.Add(driver);
            _bus.RaiseEvent(new DomainNotification("", "Create successfully!!", TypeNotification.Success));
            return Task.FromResult(driverModel as object);
        }

        public Task<object> Handle(EditDriverCommand command, CancellationToken cancellationToken)
        {
            Driver driver = new Driver(
               id: command.Id,
               name: new Model.ValueObject.Name(command.Name),
               code: new Model.ValueObject.Code(command.Code),
               phoneNumber: new Model.ValueObject.PhoneNumber(command.PhoneNumber),
               status: new Model.ValueObject.Status(command.Status),
               address: new Model.ValueObject.Address(
                   city: command.City,
                   country: command.Country,
                   district: command.District,
                   street: command.Street,
                   streetNumber: command.StreetNumber
               ),
               dateOfBirth: new Model.ValueObject.Day(Convert.ToDateTime(command.DoB)),
               cardNumber: new Model.ValueObject.CardNumber(command.IDCardNumber),
               note: new Model.ValueObject.Note(command.Note),
               sex: new Model.ValueObject.Sex(command.Sex),
               startDate: new Model.ValueObject.Day(Convert.ToDateTime(command.StartDate)),
               userID: new Model.ValueObject.Identity(command.UserID),
               vehicleTypeID: new Model.ValueObject.VehicleTypeID(String.Join(',', command.VehicleTypeIDs))
               );

            var driverModel = _driverRepository.Update(driver);
            //_bus.RaiseEvent(new DomainNotification("", "Create successfully!!", TypeNotification.Success));
            return Task.FromResult(driverModel as object);
        }

        public Task<object> Handle(ImportDriverCommand command, CancellationToken cancellationToken)
        {
            List<Driver> drivers = new List<Driver>();
            command.Drivers.ForEach(x =>
            {
                var driver = new Driver(
                   id: null,
                   name: new Model.ValueObject.Name(x.Name),
                   code: new Model.ValueObject.Code(x.Code),
                   phoneNumber: new Model.ValueObject.PhoneNumber(x.PhoneNumber),
                   status: new Model.ValueObject.Status(x.Status),
                   dateOfBirth: new Model.ValueObject.Day(Convert.ToDateTime(x.DoB)),
                   cardNumber: new Model.ValueObject.CardNumber(x.IDCardNumber),
                   sex: new Model.ValueObject.Sex(x.Sex),
                   startDate: new Model.ValueObject.Day(Convert.ToDateTime(x.StartDate))
                    );

                drivers.Add(driver);
            });
            var result = _driverRepository.ImportExcel(drivers);
            return Task.FromResult(result as object);
        }

        public Task<object> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
        {
            //if (!command.IsValid())
            //{
            //    NotifyValidationErrors(command);
            //    return Task.FromResult(false as object);
            //}
            Console.WriteLine("654321");
            var result = _driverRepository.Delete(request.ID);
            return Task.FromResult(result as object);
        }
    }
}
