using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Application.ResponseModel.Invoice
{
    public class SearchInvoiceResponse
    {
        public string Message { get; set; }

        public Object Data { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
      
    }
}
