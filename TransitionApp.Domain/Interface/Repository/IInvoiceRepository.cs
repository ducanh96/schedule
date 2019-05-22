using OrderService.Domain.Commands.Invoices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.Model.Entity;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Invoice;

namespace TransitionApp.Domain.Interface.Repository
{
    public interface IInvoiceRepository
    {
        SearchInvoiceReadModel GetAll(int page, int pageSize, DateTime? fromTime);
        CustomerReadModel GetCustomer(int customerId);
        AddressReadModel GetAddress(int addressId);
        IEnumerable<ItemReadModel> GetItems(int invoiceId);

        Task Create(Invoice invoice);
        Task Update(Invoice invoice);

        CustomerReadModel GetCustomer(string customerCode);
        IEnumerable<InvoiceReadModel> GetInvoices(DateTime deliverTime, int driverId, int customerId);
        InvoiceReadModel UpdateVoice(int invoiceId, int status);
        InvoiceReadModel GetInvoice(string code);
        IEnumerable<InvoiceReadModel> GetInvoices(List<int> ids);
        Task UpdateInvoiceStatus(Invoice invoice);
    }
}
