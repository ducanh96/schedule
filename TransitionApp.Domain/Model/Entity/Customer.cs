using System;
using System.Collections.Generic;
using System.Text;
using TransitionApp.Domain.Model.ValueObject;

namespace TransitionApp.Domain.Model.Entity
{
    public class Customer
    {
        public Customer(Identity iD, Address addressCustomer, string invoices, Status status, Weight weight)
        {
            ID = iD;
            AddressCustomer = addressCustomer;
            Invoices = invoices;
            Status = status;
            Weight = weight;
        }

        public Identity ID { get; set; }
        public Address AddressCustomer { get; set; }
        public string Invoices { get; set; }
        public Status Status { get; set; }
        public Weight Weight { get; set; }
    }
}
