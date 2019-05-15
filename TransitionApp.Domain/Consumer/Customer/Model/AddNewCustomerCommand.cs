using System;

namespace OrderService.Domain.Commands.Customer
{
    public class AddNewCustomerCommand
    {
       
        public string Name { get;  set; }
        public string PhoneNumber { get;  set; }
        public string Code { get;  set; }


        //Address
        public string City { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
    }
}
