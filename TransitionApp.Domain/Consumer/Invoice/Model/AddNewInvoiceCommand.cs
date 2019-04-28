using System;
using System.Collections.Generic;

namespace OrderService.Domain.Commands.Invoices
{
    public class AddNewInvoiceCommand
    {
        public string CustomerCode { get; set; }
        public string Note { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool Served { get; set; } = false;
        public int Status { get; set; }
        public decimal WeightTotal { get; set; }
        public int TotalPrice { get; set; }
        public string Code { get; set; }
        public IEnumerable<InvoiceItemModel> Items { get; set; }
    }
    public class InvoiceItemModel
    {

        public bool Deliveried { get; set; } = false;
        public int DeliveriedQuantity { get; set; } = 0;
        public string Note { get; set; }
        public int Price { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public string UnitName { get; set; }
        public decimal Weight { get; set; }

    }

}
