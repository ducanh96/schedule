﻿using System;
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
        CustomerReadModel GetCustomer(string customerCode);
        AddressReadModel GetAddress(int addressId);
        IEnumerable<ItemReadModel> GetItems(int invoiceId);
        IEnumerable<InvoiceReadModel> GetInvoices(DateTime deliverTime, int driverId, int customerId);
        IEnumerable<InvoiceReadModel> GetInvoices(List<int> ids);

        #region For Bot
        bool UpdateVoice(int invoiceId, int status);
        InvoiceReadModel GetInvoice(string code);
        #endregion
    }
}
