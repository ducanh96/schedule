using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Invoice;

namespace TransitionApp.Service.Interface
{
    public interface IInvoiceService
    {
        SearchInvoiceReadModel GetAll(int page, int pageSize, DateTime? FromDate);
        CustomerReadModel GetCustomer(int customerId);
        AddressReadModel GetAddress(int addressId);
        IEnumerable<ItemReadModel> GetItems(int invoiceId);
    }
}
