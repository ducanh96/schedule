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
            try
            {
                var ipValue = "192.168.43.51";
                var invoice = _invoiceRepository.UpdateVoice(invoiceId, status);
                if (invoice != null)
                {
                    IBus rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(
                     rabbit =>
                     {
                         IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri($"rabbitmq://{ipValue}/"), settings =>
                         {
                             settings.Username("tuandv");
                             settings.Password("tuandv");
                         });
                     }
                    );
                    rabbitBusControl.Publish<InvoiceReadModel>(invoice);
                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                return false;
            }


        }

        public InvoiceReadModel GetInvoice(string code)
        {
            return _invoiceRepository.GetInvoice(code);
        }

        public IEnumerable<InvoiceReadModel> GetInvoices(List<int> ids)
        {
            return _invoiceRepository.GetInvoices(ids);
        }
    }
}
