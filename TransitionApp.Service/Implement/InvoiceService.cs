using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Collections.Generic;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Invoice;
using TransitionApp.Service.Interface;

namespace TransitionApp.Service.Implement
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public CustomerReadModel GetCustomer(int customerId)
        {
            return _invoiceRepository.GetCustomer(customerId);
        }

        public SearchInvoiceReadModel GetAll(int page, int pageSize, DateTime? fromDate)
        {
            return _invoiceRepository.GetAll(page, pageSize, fromDate);
        }

        public AddressReadModel GetAddress(int addressId)
        {
            return _invoiceRepository.GetAddress(addressId);
        }

        public IEnumerable<ItemReadModel> GetItems(int invoiceId)
        {
            return _invoiceRepository.GetItems(invoiceId);
        }

        public CustomerReadModel GetCustomer(string customerCode)
        {
            return _invoiceRepository.GetCustomer(customerCode);
        }

        public IEnumerable<InvoiceReadModel> GetInvoices(DateTime deliverTime, int driverId, int customerId)
        {
            return _invoiceRepository.GetInvoices(deliverTime, driverId, customerId);
        }

        public bool UpdateVoice(int invoiceId, int status)
        {
            var ipValue = "192.168.1.32";
            var invoice = _invoiceRepository.UpdateVoice(invoiceId, status);
            if (status != invoice.Status)
            {
                IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(
              rabbit =>
              {
                  IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri($"rabbitmq://{ipValue}/"), settings =>
                  {
                  });
              }
            );
                rabbitBusControl.Start();

                rabbitBusControl.Publish<InvoiceReadModel>(new InvoiceReadModel
                {
                    Status = invoice.Status,
                    Code = invoice.Code
                });
                rabbitBusControl.StopAsync();

                return true;
            }
            return false;
        }
    }
}
