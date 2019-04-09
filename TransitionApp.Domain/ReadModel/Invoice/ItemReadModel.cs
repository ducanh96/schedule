using System;
using System.Collections.Generic;
using System.Text;

namespace TransitionApp.Domain.ReadModel.Invoice
{
    public class ItemReadModel
    {
        public int ID { get; set; }
        public bool Deliveried { get; set; }
        public int DeliveriedQuantity { get; set; }
        public bool IsExistedProduct { get; set; }
        public string Note { get; set; }
        public double Price { get; set; }
        public int ProductID{ get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public double Weight { get; set; }
        public int InvoiceId { get; set; }
    }
}
