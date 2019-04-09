using System;

namespace TransitionApp.Domain.ReadModel.Invoice
{
    public class InvoiceReadModel
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public bool Served { get; set; }
        public DateTime ServerTime { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public double TotalPrice { get; set; }
        public double WeightTotal { get; set; }
        public int CustomerId { get; set; }
        public int Status { get; set; }
    }
}
