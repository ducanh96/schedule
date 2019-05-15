using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel.Account;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Driver;
using TransitionApp.Domain.ReadModel.Invoice;

namespace EchoBot.Model
{
    public class UserProfile
    {
        public string Name { get; set; }
        public DriverReadModel Driver { get; set; }
        public AccountReadModel Account { get; set; }
        public DataMoment Data { get; set; }
        public DateTime TimeDeliver { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public int? Age { get; set; }
        public string Date { get; set; }
        public bool IsStartConverstation { get; set; } = false;
        public bool IsStartTransition { get; set; } = false;
        public UserProfile()
        {
            Data = new DataMoment();
        }
    }
    public class DataMoment
    {
        public CustomerDetailReadModel CustomerDetail { get; set; }
        public List<InvoiceReadModel> Invoices { get; set; }
        

    }
}
