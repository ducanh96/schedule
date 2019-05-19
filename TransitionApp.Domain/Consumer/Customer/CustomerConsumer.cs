using MassTransit;
using OrderService.Domain.Commands.Customer;
using System;
using System.Threading.Tasks;
using TransitionApp.Domain.Interface.Repository;

namespace TransitionApp.Domain.Consumer.Customer
{
    public class CustomerConsumer : IConsumer<AddNewCustomerCommand>, IConsumer<UpdateCustomerCommand>
    {
        public ICustomerRepository _customerRepository;
        public CustomerConsumer(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Task Consume(ConsumeContext<AddNewCustomerCommand> context)
        {
            try
            {
                AddNewCustomerCommand newCustomer = context.Message;
                Model.ValueObject.Address address = new Model.ValueObject.Address(
                    newCustomer.City,
                    newCustomer.Country,
                    newCustomer.District,
                    newCustomer.Street,
                    newCustomer.StreetNumber,
                    newCustomer.Lat,
                    newCustomer.Lng
                    );
                Model.Entity.Customer customer = new Model.Entity.Customer(
                    address,
                    new Model.ValueObject.Code(newCustomer.Code),
                    new Model.ValueObject.Name(newCustomer.Name),
                    new Model.ValueObject.PhoneNumber(newCustomer.PhoneNumber)
                    );

                return _customerRepository.Create(customer);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
            
        }

        public Task Consume(ConsumeContext<UpdateCustomerCommand> context)
        {
            try
            {
                UpdateCustomerCommand newCustomer = context.Message;
                Model.ValueObject.Address address = new Model.ValueObject.Address(
                    newCustomer.City,
                    newCustomer.Country,
                    newCustomer.District,
                    newCustomer.Street,
                    newCustomer.StreetNumber,
                    newCustomer.Lat,
                    newCustomer.Lng
                    );
                Model.Entity.Customer customer = new Model.Entity.Customer(
                    address,
                    new Model.ValueObject.Code(newCustomer.Code),
                    new Model.ValueObject.Name(newCustomer.Name),
                    new Model.ValueObject.PhoneNumber(newCustomer.PhoneNumber)
                    );

                return _customerRepository.Update(customer);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
    }
}
