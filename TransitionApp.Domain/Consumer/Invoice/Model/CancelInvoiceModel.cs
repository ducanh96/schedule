using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Invoice
{
    public class CancelInvoiceModel
    {
        public int Status { get; set; }
        public string Code { get; set; }
    }
}
