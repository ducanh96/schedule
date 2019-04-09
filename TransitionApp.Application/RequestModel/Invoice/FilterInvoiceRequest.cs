using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.RequestModel.Invoice
{
    public class FilterInvoiceRequest
    {
        public DateTime? from_time { get; set; }
        public int page { get; set; }
        public int page_size { get; set; }
    }
}
