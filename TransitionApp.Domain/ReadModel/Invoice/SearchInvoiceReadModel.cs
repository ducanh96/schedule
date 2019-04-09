using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Invoice
{
    public class SearchInvoiceReadModel
    {
        public IEnumerable<InvoiceReadModel> Invoices { get; set; }
        public PagingReadModel PageInfo { get; set; }
    }
}
