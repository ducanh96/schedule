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
    }
}
